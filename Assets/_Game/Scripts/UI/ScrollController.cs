using System.Collections; 
using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
  [SerializeField] private ScrollRect scrollRect;
  [SerializeField] private Button leftButton;
  [SerializeField] private Button rightButton;

  private GridLayoutGroup gridLayoutGroup;
  private int currentIndex;
  private bool isScrolling;

  private void Start()
  {
    leftButton.onClick.AddListener(ScrollToPreviousItem);
    rightButton.onClick.AddListener(ScrollToNextItem);

    gridLayoutGroup = scrollRect.content.GetComponent<GridLayoutGroup>();
    
    scrollRect.horizontal = false;
    scrollRect.vertical = false;
  }

  private void ScrollToPreviousItem()
  {
    if (!isScrolling)
      StartCoroutine(ScrollToItemCoroutine(currentIndex - 1));
  }

  private void ScrollToNextItem()
  {
    if (!isScrolling)
      StartCoroutine(ScrollToItemCoroutine(currentIndex + 1));
  }

  private IEnumerator ScrollToItemCoroutine(int index)
  {
    if (index < 0 || index >= scrollRect.content.childCount)
    {
      yield break;
    }

    isScrolling = true;
    currentIndex = index;

    float targetPosition = (currentIndex * (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x)) / (scrollRect.content.rect.width - scrollRect.viewport.rect.width);
    Vector2 startPosition = scrollRect.normalizedPosition;
    float elapsedTime = 0f;
    float duration = 0.5f;

    while (elapsedTime < duration)
    {
      elapsedTime += Time.deltaTime;
      scrollRect.normalizedPosition = Vector2.Lerp(startPosition, new Vector2(targetPosition, 0), elapsedTime / duration);
      yield return null;
    }

    scrollRect.normalizedPosition = new Vector2(targetPosition, 0);
    isScrolling = false;
  }
}