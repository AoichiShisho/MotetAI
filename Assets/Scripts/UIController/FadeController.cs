using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    protected async UniTask FadeOut(GameObject fadeOutObject)
    {
        CanvasGroup fadeOutCanvasGroup = fadeOutObject.GetComponent<CanvasGroup>();
        if (fadeOutCanvasGroup == null)
        {
            fadeOutCanvasGroup = fadeOutObject.AddComponent<CanvasGroup>();
        }
        
        await fadeOutCanvasGroup.DOFade(0f, 1f).SetEase(Ease.InOutQuad).ToUniTask();
    }

    protected async UniTask FadeIn(GameObject fadeInObject)
    {
        CanvasGroup fadeInCanvasGroup = fadeInObject.GetComponent<CanvasGroup>();
        if (fadeInCanvasGroup == null)
        {
            fadeInCanvasGroup = fadeInObject.AddComponent<CanvasGroup>();
        }

        fadeInCanvasGroup.alpha = 0f;

        await fadeInCanvasGroup.DOFade(1f, 1f).SetEase(Ease.InOutQuad).ToUniTask();
    }
}
