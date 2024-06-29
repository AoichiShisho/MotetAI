using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RevealUIController : FadeController
{
    [Header("UI Elements")]
    public TextMeshProUGUI actionText;

    [Header("Button")]
    public Button proceedButton;

    [Header("Parent Objects")]
    public GameObject revealParent;
    public GameObject answerParent;

    public void SetActionText(string action)
    {
        actionText.text = action;
    }

    public async void ProceedToAnswer()
    {
        await FadeOut(revealParent);

        revealParent.SetActive(false);
        answerParent.SetActive(true);

        await FadeIn(answerParent);
    }
}
