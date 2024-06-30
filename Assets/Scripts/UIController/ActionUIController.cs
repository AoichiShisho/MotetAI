using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionUIController : FadeController
{
    [Header("UI Elements")]
    public TextMeshProUGUI promptText;
    public TMP_InputField actionInputField;
    public Button submitActionButton;
    public TextMeshProUGUI textAmount;

    [Header("Parent Objects")]
    public GameObject actionParent;
    public GameObject actionWaitingParent;
    
    private MainGameManager mainGameManager;

    void Start()
    {
        mainGameManager = FindObjectOfType<MainGameManager>();

        submitActionButton.onClick.AddListener(SubmitAction);
        actionInputField.AddTextAmountUpdater(textAmount);
    }

    public void SetPrompt(string prompt)
    {
        promptText.text = prompt;
    }

    void SubmitAction()
    {
        string action = actionInputField.text;

        actionParent.SetActive(false);
        actionWaitingParent.SetActive(true);
        Debug.Log("ShowWaiting");

        mainGameManager.SubmitAction(action);
    }
}
