using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RevealUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI actionText;
    public Button proceedButton;

    [Header("Parent Objects")]
    public GameObject revealParent;
    public GameObject answerParent;

    void Start()
    {
        proceedButton.onClick.AddListener(OnProceedButtonClicked);
    }

    public void SetActionText(string action)
    {
        actionText.text = action;
    }

    void OnProceedButtonClicked()
    {
        revealParent.SetActive(false);
        answerParent.SetActive(true);
    }
}
