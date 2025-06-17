using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class TabEntry
{
    public Button Button;
    public GameObject Panel;
}

public class TabsHandler : MonoBehaviour
{
    [SerializeField] private List<TabEntry> _tabs;
    [SerializeField] private RectTransform _indicator;
    [SerializeField] private float _moveDuration = 0.25f;
    [SerializeField] private float _panelMoveDuration = 0.3f;
    [SerializeField] private Ease _easeType = Ease.OutQuart;

    private int _currentTabIndex = 0;
    private bool _indicatorSized = false;

    public void OpenTabByButton(Button clickedButton)
    {
        int newIndex = _tabs.FindIndex(tab => tab.Button == clickedButton);
        if (newIndex == -1 || newIndex == _currentTabIndex)
            return;

        AnimateTabTransition(_currentTabIndex, newIndex);
        MoveIndicatorTo(_tabs[newIndex].Button.GetComponent<RectTransform>());
        _currentTabIndex = newIndex;
    }

    private void AnimateTabTransition(int oldIndex, int newIndex)
    {
        var oldPanel = _tabs[oldIndex].Panel.GetComponent<RectTransform>();
        var newPanel = _tabs[newIndex].Panel.GetComponent<RectTransform>();

        // Анимации: отключить после выезда, включить перед въездом
        newPanel.gameObject.SetActive(true);

        float width = ((RectTransform)oldPanel.parent).rect.width;
        float direction = newIndex > oldIndex ? 1 : -1;

        // Ставим новую панель за пределы экрана
        newPanel.anchoredPosition = new Vector2(direction * width, 0);

        // Анимация старой панели
        oldPanel.DOAnchorPosX(-direction * width, _panelMoveDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => oldPanel.gameObject.SetActive(false));

        // Анимация новой панели
        newPanel.DOAnchorPosX(0, _panelMoveDuration)
            .SetEase(Ease.InOutQuad);
    }

    private void MoveIndicatorTo(RectTransform buttonRect)
    {
        Canvas.ForceUpdateCanvases();

        Vector3[] corners = new Vector3[4];
        buttonRect.GetWorldCorners(corners);
        Vector3 worldLeft = corners[0];
        Vector3 worldRight = corners[3];
        Vector3 worldCenter = (worldLeft + worldRight) / 2f;

        Vector3 localCenter = _indicator.parent.InverseTransformPoint(worldCenter);

        _indicator.DOAnchorPosX(localCenter.x, _moveDuration).SetEase(_easeType);

        if (!_indicatorSized)
        {
            float width = Vector3.Distance(
                _indicator.parent.InverseTransformPoint(worldLeft),
                _indicator.parent.InverseTransformPoint(worldRight)
            );

            _indicator.DOSizeDelta(new Vector2(width, _indicator.sizeDelta.y), _moveDuration)
                .SetEase(_easeType);

            _indicatorSized = true;
        }
    }
}
