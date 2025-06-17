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
    [SerializeField] private Ease _easeType = Ease.OutQuart;

    private bool _indicatorSized = false;

    public void OpenTabByButton(Button clickedButton)
    {
        foreach (var tab in _tabs)
        {
            bool isActive = tab.Button == clickedButton;
            tab.Panel.SetActive(isActive);

            if (isActive)
                MoveIndicatorTo(tab.Button.GetComponent<RectTransform>());
        }
    }

    private void MoveIndicatorTo(RectTransform buttonRect)
    {
        // Получаем мировую позицию центра кнопки
        Vector3[] corners = new Vector3[4];
        buttonRect.GetWorldCorners(corners);
        Vector3 worldLeft = corners[0];
        Vector3 worldRight = corners[3];
        Vector3 worldCenter = (worldLeft + worldRight) / 2f;

        // Переводим в локальные координаты родителя индикатора
        Vector3 localCenter = _indicator.parent.InverseTransformPoint(worldCenter);

        // Двигаем индикатор по X
        _indicator.DOAnchorPosX(localCenter.x, _moveDuration).SetEase(_easeType);

        // Ширину можно задать один раз, если кнопки одинаковые
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