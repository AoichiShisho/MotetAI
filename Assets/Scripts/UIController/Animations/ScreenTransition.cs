using UnityEngine;
using DG.Tweening;

public class ScreenTransition : MonoBehaviour
{
    public float duration;
    public float waitTime;

    private RectTransform rectTransform;
    private float initialPositionX;
    private Vector2 initialPosition;

    private void Start()
    {
        InitializeRectTransform();
    }

    private void InitializeRectTransform()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform component is not found.");
        }
    }

    private void EnsureRectTransformInitialized()
    {
        if (rectTransform == null)
        {
            InitializeRectTransform();
        }
    }

    public void SetInitialLeftPosition()
    {
        EnsureRectTransformInitialized();
        initialPositionX = -rectTransform.rect.width / 2;
        rectTransform.anchoredPosition = new Vector2(initialPositionX, 0);
    }

    public void SetInitialCenterPosition()
    {
        EnsureRectTransformInitialized();
        initialPosition = new Vector2(rectTransform.rect.width/2 - 100, 0);
        rectTransform.anchoredPosition = initialPosition;
    }

    public Sequence EnterTransition()
    {
        EnsureRectTransformInitialized();
        Sequence mySequence = DOTween.Sequence();

        // 左から中央へ
        mySequence.Append(rectTransform.DOAnchorPosX(rectTransform.rect.width / 2 - 100, duration).SetEase(Ease.InOutQuad));
        // 中央で待機
        mySequence.AppendInterval(waitTime);

        mySequence.Play();
        return mySequence;
    }

    public void ExitTransition()
    {
        EnsureRectTransformInitialized();
        Sequence mySequence = DOTween.Sequence();
        // 中央から右へ
        mySequence.Append(rectTransform.DOAnchorPosX(rectTransform.rect.width * 3/2 - 100, duration).SetEase(Ease.InOutQuad));

        mySequence.Play();
    }
}
