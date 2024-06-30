using UnityEngine;
using DG.Tweening;

public class ErrorController : MonoBehaviour
{
    [SerializeField] private RectTransform errorImageRect;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float displayDuration = 1.0f;

    private Vector2 offScreenPosition;
    private Vector2 onScreenPosition;
    private Tweener currentTween;

    void Start()
    {
        offScreenPosition = new Vector2(errorImageRect.anchoredPosition.x, Screen.height + errorImageRect.rect.height);
        onScreenPosition = errorImageRect.anchoredPosition;

        errorImageRect.anchoredPosition = offScreenPosition;
    }

    public void ShowError()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        currentTween = errorImageRect.DOAnchorPos(onScreenPosition, animationDuration).OnComplete(() =>
        {
            DOVirtual.DelayedCall(displayDuration, () =>
            {
                currentTween = errorImageRect.DOAnchorPos(offScreenPosition, animationDuration);
            });
        });
    }
}
