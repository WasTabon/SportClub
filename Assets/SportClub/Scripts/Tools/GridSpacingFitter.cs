using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridSpacingFitter : MonoBehaviour
{
    public int elementCount = 5;
    public float elementWidth = 170f;

    private RectTransform _rectTransform;
    private GridLayoutGroup _grid;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _grid = GetComponent<GridLayoutGroup>();
        UpdateSpacing();
    }

    private void UpdateSpacing()
    {
        float totalWidth = _rectTransform.rect.width;
        float spacing = (totalWidth - elementCount * elementWidth) / (elementCount - 1);
        spacing = Mathf.Max(spacing, 0);

        _grid.spacing = new Vector2(spacing, _grid.spacing.y);
        _grid.cellSize = new Vector2(elementWidth, _grid.cellSize.y);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying == false && _rectTransform != null && _grid != null)
        {
            UpdateSpacing();
        }
    }
#endif

    private void OnRectTransformDimensionsChange()
    {
        UpdateSpacing();
    }
}