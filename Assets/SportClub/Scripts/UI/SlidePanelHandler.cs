using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlidePanelHandler : MonoBehaviour
{
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private Ease _easeIn = Ease.OutCubic;
    [SerializeField] private Ease _easeOut = Ease.InCubic;

    private Dictionary<RectTransform, Vector2> _originalPositions = new();

    private void Awake()
    {
        foreach (var panel in GetComponentsInChildren<RectTransform>(true))
        {
            if (panel.gameObject != gameObject && panel.gameObject.activeSelf == false)
            {
                _originalPositions[panel] = panel.anchoredPosition;
            }
        }
    }

    public void OpenPanel(RectTransform panel)
    {
        if (!_originalPositions.ContainsKey(panel))
            _originalPositions[panel] = panel.anchoredPosition;

        panel.gameObject.SetActive(true);

        Vector2 targetPos = _originalPositions[panel];
        float width = ((RectTransform)panel.parent).rect.width;

        panel.anchoredPosition = new Vector2(targetPos.x + width, targetPos.y);

        panel.DOAnchorPos(targetPos, _duration)
            .SetEase(_easeIn);
    }

    public void ClosePanel(RectTransform panel)
    {
        if (!_originalPositions.ContainsKey(panel))
        {
            panel.gameObject.SetActive(false);
            return;
        }

        Vector2 targetPos = _originalPositions[panel];
        float width = ((RectTransform)panel.parent).rect.width;

        panel.DOAnchorPos(new Vector2(targetPos.x + width, targetPos.y), _duration)
            .SetEase(_easeOut)
            .OnComplete(() => panel.gameObject.SetActive(false));
    }
}