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
        actionInputField.onValueChanged.AddListener(UpdateTextAmount);

        UpdateTextAmount(actionInputField.text);
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

    void UpdateTextAmount(string text)
    {
        int currentLength = text.Length;
        int maxChars = actionInputField.characterLimit;

        textAmount.text = $"{currentLength}/{maxChars}";

        if (currentLength >= maxChars)
            textAmount.color = new Color32(255, 87, 87, 255); // FF5757
        else
            textAmount.color = new Color32(87, 87, 87, 255); // 575757
    }
}
