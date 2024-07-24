using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RevealUIController : FadeController
{
    [Header("UI Elements")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI actionText;

    [Header("Button")]
    public Button proceedButton;

    [Header("Parent Objects")]
    public GameObject revealParent;

    public void SetActionText(string action, string playerName)
    {
        titleText.text = $"{playerName}の試みた行動は...";
        actionText.text = action;
    }
}
