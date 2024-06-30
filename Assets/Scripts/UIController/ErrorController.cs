using UnityEngine;
using DG.Tweening;

public class ErrorController : MonoBehaviour
{
    [SerializeField] private RectTransform errorImageRect;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float displayDuration = 1.0f;
    [SerializeField] private float moveDistance = 150f;

    private Vector2 originalPosition;
    private Tweener currentTween;

    void Start()
    {
        originalPosition = errorImageRect.anchoredPosition;
    }

    public void ShowError()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill(false);
        }

        currentTween = errorImageRect.DOAnchorPos(new Vector2(originalPosition.x, originalPosition.y - moveDistance), animationDuration).OnComplete(() =>
        {
            DOVirtual.DelayedCall(displayDuration, () =>
            {
                currentTween = errorImageRect.DOAnchorPos(originalPosition, animationDuration);
            });
        });
    }
}
