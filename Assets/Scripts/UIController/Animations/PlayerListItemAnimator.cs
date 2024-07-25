using UnityEngine;
using DG.Tweening;

public class PlayerListItemAnimator : MonoBehaviour
{
    private RectTransform rectTransform;
    private int endValue = 225;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void AnimateIn()
    {
        rectTransform.anchoredPosition = new Vector2(Screen.width, rectTransform.anchoredPosition.y);

        rectTransform.DOAnchorPosX(endValue, 0.5f).SetEase(Ease.OutCubic);
    }
}
