using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

[System.Serializable]
public class TabEntry
{
    public Button Button;
    public GameObject Panel;
}

public class TabsHandler : MonoBehaviour
{
    [SerializeField] private Scrollbar _verticalScrollbar;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _scrollContentToClear;
    [SerializeField] private Button _mainMenuButton;
    
    [SerializeField] private List<TabEntry> _tabs;
    [SerializeField] private RectTransform _indicator;
    [SerializeField] private float _moveDuration = 0.25f;
    [SerializeField] private float _panelMoveDuration = 0.3f;
    [SerializeField] private Ease _easeType = Ease.OutQuart;

    private int _currentTabIndex = 0;
    private bool _indicatorSized = false;

    private void Start()
    {
        Debug.Log($"<color=yellow> Tabs Handler: All tabs exept first one should be disabled when game starts</color>");
    }

    public void FinishDay()
    {
        OpenTabByButton(_mainMenuButton);
    
        float totalWait = Mathf.Max(_panelMoveDuration, _moveDuration);

        DOVirtual.DelayedCall(totalWait, () =>
        {
            ScrollToTop(() =>
            {
                ClearContentWithAnimation();
            });
        });
    }
    
    private Tween AnimateScrollbarSize(float targetSize, float duration = 0.3f)
    {
        return DOTween.To(
            () => _verticalScrollbar.size,
            value => _verticalScrollbar.size = value,
            targetSize,
            duration
        ).SetEase(Ease.OutQuad);
    }
    
    private void ClearContentWithAnimation()
    {
        var children = new List<Transform>();
        foreach (Transform child in _scrollContentToClear)
            children.Add(child);

        Sequence sequence = DOTween.Sequence();

        float stepDuration = 0.8f;
        float postRebuildDelay = 0.1f; // пауза после удаления перед началом следующего

        for (int i = 0; i < children.Count; i++)
        {
            Transform child = children[i];

            // Ensure CanvasGroup exists
            CanvasGroup cg = child.GetComponent<CanvasGroup>();
            if (cg == null) cg = child.gameObject.AddComponent<CanvasGroup>();

            // Reset transforms
            child.localScale = Vector3.one;
            child.localRotation = Quaternion.identity;
            cg.alpha = 1;

            sequence.AppendCallback(() =>
            {
                // Анимация элемента
                Sequence childSeq = DOTween.Sequence();

                childSeq.Join(cg.DOFade(0, stepDuration));
                childSeq.Join(child.DOScale(Vector3.zero, stepDuration).SetEase(Ease.InBack));
                childSeq.Join(child.DOLocalMoveY(child.localPosition.y - 20f, stepDuration).SetEase(Ease.InQuad));
                childSeq.Join(child.DORotate(new Vector3(0, 0, Random.Range(-30f, 30f)), stepDuration).SetEase(Ease.OutCubic));

                childSeq.OnComplete(() =>
                {
                    // Удаляем объект
                    Destroy(child.gameObject);

                    // Принудительно обновляем layout
                    LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollContentToClear);
                    
                    float viewportHeight = _scrollRect.viewport.rect.height;
                    float contentHeight = _scrollContentToClear.rect.height;
                    float newSize = Mathf.Clamp01(viewportHeight / contentHeight);

                    AnimateScrollbarSize(newSize, 0.5f);
                });
            });

            // Ждём анимацию + паузу перед следующим элементом
            sequence.AppendInterval(stepDuration + postRebuildDelay);
            sequence.OnComplete((() =>
            {
                UIManager.Instance.ShowPanelFinishDay();
            }));
        }
    }
    
    private void ScrollToTop(Action onComplete)
    {
        float start = _scrollRect.verticalNormalizedPosition;
        float duration = 0.5f;

        DOTween.To(() => _scrollRect.verticalNormalizedPosition, 
                x => _scrollRect.verticalNormalizedPosition = x, 
                1f, duration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => onComplete?.Invoke());
    }
    
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
        
        newPanel.gameObject.SetActive(true);

        float width = ((RectTransform)oldPanel.parent).rect.width;
        float direction = newIndex > oldIndex ? 1 : -1;

        newPanel.anchoredPosition = new Vector2(direction * width, 0);

        oldPanel.DOAnchorPosX(-direction * width, _panelMoveDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => oldPanel.gameObject.SetActive(false));

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
