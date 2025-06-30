using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridSpacingFitter : MonoBehaviour
{
    public int elementCount = 5;
    public float elementSize = 80f;

    public float leftPadding = 125f;
    public float rightPadding = 125f;
    public float bottomPadding = 55f;

    private RectTransform _rectTransform;
    private GridLayoutGroup _grid;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _grid = GetComponent<GridLayoutGroup>();

        ApplyFitting();
    }

    private void OnRectTransformDimensionsChange()
    {
        ApplyFitting();
    }

    private void ApplyFitting()
    {
        if (_rectTransform == null || _grid == null) return;

        // Safe area Y
        Rect safeArea = Screen.safeArea;
        float safeAreaTop = safeArea.yMax;
        float safeAreaBottom = safeArea.yMin;

        float canvasHeight = _rectTransform.rect.height;
        float screenToCanvasRatio = canvasHeight / Screen.height;

        float topPadding = (Screen.height - safeAreaTop) * screenToCanvasRatio;
        float bottom = (safeAreaBottom * screenToCanvasRatio) + bottomPadding;

        // Расчёт доступной ширины с учётом левого и правого отступов
        float availableWidth = _rectTransform.rect.width - leftPadding - rightPadding;
        float spacing = (availableWidth - elementCount * elementSize) / (elementCount - 1);
        spacing = Mathf.Max(spacing, 0);

        _grid.cellSize = new Vector2(elementSize, elementSize);
        _grid.spacing = new Vector2(spacing, _grid.spacing.y);
        _grid.padding = new RectOffset((int)leftPadding, (int)rightPadding, (int)topPadding, (int)bottom);
    }
}