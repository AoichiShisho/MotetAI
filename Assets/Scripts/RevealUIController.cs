using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class RevealUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI actionText;

    [Header("Button")]
    public Button proceedButton;

    [Header("Parent Objects")]
    public GameObject revealParent;
    public GameObject answerParent;

    void Start()
    {
        proceedButton.onClick.AddListener(ProceedToAnswer);
    }

    public void SetActionText(string action)
    {
        actionText.text = action;
    }

    void ProceedToAnswer()
    {
        FadeOut(revealParent);

        revealParent.SetActive(false);
        answerParent.SetActive(true);

        FadeIn(answerParent);
    }

    private async UniTask FadeOut(GameObject fadeOutObject)
    {
        CanvasGroup fadeOutCanvasGroup = fadeOutObject.GetComponent<CanvasGroup>();
        if (fadeOutCanvasGroup == null)
        {
            fadeOutCanvasGroup = fadeOutObject.AddComponent<CanvasGroup>();
        }

        // フェードアウトのアニメーション
        await fadeOutCanvasGroup.DOFade(0f, 1f).SetEase(Ease.InOutQuad).ToUniTask();
    }

    private async UniTask FadeIn(GameObject fadeInObject)
    {
        CanvasGroup fadeInCanvasGroup = fadeInObject.GetComponent<CanvasGroup>();
        if (fadeInCanvasGroup == null)
        {
            fadeInCanvasGroup = fadeInObject.AddComponent<CanvasGroup>();
        }

        fadeInCanvasGroup.alpha = 0f;

        // フェードインのアニメーション
        await fadeInCanvasGroup.DOFade(1f, 1f).SetEase(Ease.InOutQuad).ToUniTask();
    }
}
