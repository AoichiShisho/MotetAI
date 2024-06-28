using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        revealParent.SetActive(false);
        answerParent.SetActive(true);
    }
}