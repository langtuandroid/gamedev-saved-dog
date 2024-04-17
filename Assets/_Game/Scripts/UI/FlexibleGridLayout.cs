using UnityEngine;
using UnityEngine.UI;
public class FlexibleGridLayout : LayoutGroup
{
  [SerializeField]
  public bool VerticalSizeFit;
  [SerializeField]
  public bool HorizontalSizeFit;
  [SerializeField]
  private GridElementsSize _elementSize;
  [SerializeField]
  private GridFitType _fitType;
  [SerializeField]
  private Vector2 _spacing;
  [SerializeField]
  private int _rows;
  [SerializeField]
  private int _columns;
  [SerializeField]
  private Vector2 _cellSize;
  [SerializeField]
  private float _height;

  private bool _fitX;
  private bool _fitY;
  
  public override void CalculateLayoutInputHorizontal()
  {
    base.CalculateLayoutInputHorizontal();

    CalculateColumnsRowsAmount();

    CalculateElementSize();

    SetElementsPosition();

    if (VerticalSizeFit)
    {
      VerticalPanelFit();
    }
    else if (HorizontalSizeFit)
    {
      HorizontalPanelFit();
    }
  }

  public override void CalculateLayoutInputVertical()
  {}

  public override void SetLayoutHorizontal()
  {}

  public override void SetLayoutVertical()
  {}

  private void VerticalPanelFit()
  {
    rectTransform.pivot = new Vector2(.5f, 1f);

    //float height = _rows * _cellSize.y + _rows * _spacing.y + padding.top /(float) 2;
    float height = _height;

    Vector2 currentSize = rectTransform.sizeDelta;
    currentSize.y = height;

    rectTransform.sizeDelta = currentSize;
  }

  private void HorizontalPanelFit()
  {
    rectTransform.pivot = new Vector2(0f, .5f);

    float width = _columns * _cellSize.x + _columns * _spacing.x + padding.left /(float) 2;

    Vector2 currentSize = rectTransform.sizeDelta;
    currentSize.x = width;

    rectTransform.sizeDelta = currentSize;
  }
  
  private void CalculateColumnsRowsAmount()
  {
    _fitX = false;
    _fitY = false;
    
    if (_fitType != GridFitType.FixedColumns && _fitType!= GridFitType.FixedRows)
    {
      float sqrRt = Mathf.Sqrt(transform.childCount);
      _rows = Mathf.CeilToInt(sqrRt);
      _columns = Mathf.CeilToInt(sqrRt);
      
      _fitX = true;
      _fitY = true;
    }

    if (_fitType is GridFitType.Width or GridFitType.FixedColumns)
    {
      _rows = Mathf.CeilToInt(transform.childCount / (float)_columns);
    }
    else if (_fitType is GridFitType.Height or GridFitType.FixedRows)
    {
      _columns = Mathf.CeilToInt(transform.childCount / (float)_rows);
    }
  }

  private void CalculateElementSize()
  {
    Vector2 parentSize = rectTransform.rect.size;

    float cellWidth = parentSize.x / _columns - _spacing.x / _columns * (_columns - 1) - padding.left / (float)_columns - padding.right / (float)_columns;
    float cellHeight = parentSize.y / _rows - _spacing.y / _rows * (_rows - 1) - padding.top / (float)_rows - padding.bottom / (float)_rows;

    _cellSize.x = _fitX ? cellWidth : _cellSize.x;
    _cellSize.y = _fitY ? cellHeight : _cellSize.y;

    switch (_elementSize)
    {
      case GridElementsSize.SameAsX:
      {
        _cellSize.y = _cellSize.x;
        break;
      }
      case GridElementsSize.SameAsY:
      {
        _cellSize.x = _cellSize.y;
        break;
      }
    }
  }

  private void SetElementsPosition()
  {
    int columntCount = 0;
    int rowCount = 0;

    for (int i = 0; i < rectChildren.Count; i++)
    {
      rowCount = i / _columns;
      columntCount = i % _columns;

      RectTransform item = rectChildren[i];

      float xPos = _cellSize.x * columntCount + _spacing.x * columntCount + padding.left;
      float yPos = _cellSize.y * rowCount + _spacing.y * rowCount + padding.top;

      SetChildAlongAxis(item, 0, xPos, _cellSize.x);
      SetChildAlongAxis(item, 1, yPos, _cellSize.y);
    }
  }

  private enum GridFitType
  {
    Uniform,
    Width,
    Height,
    FixedRows,
    FixedColumns
  }
  
  private enum GridElementsSize
  {
    None,
    SameAsX,
    SameAsY
  }
}