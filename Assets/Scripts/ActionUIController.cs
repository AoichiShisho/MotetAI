using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI promptText;
    public TMP_InputField actionInputField;
    public Button submitActionButton;
    public TextMeshProUGUI textAmount;

    [Header("Parent Objects")]
    public GameObject actionParent;
    public GameObject revealParent;

    private PromptEditor promptEditor;
    private ChatGPTInteraction chatGPTInteraction;
    private RevealUIController revealUIController;

    void Start()
    {
        promptEditor = GetComponent<PromptEditor>();
        chatGPTInteraction = GetComponent<ChatGPTInteraction>();
        revealUIController = GetComponent<RevealUIController>();

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
        string scenario = promptEditor.GetCurrentPrompt();
        string action = actionInputField.text;
        string prompt = $"{scenario}\nプレイヤーの行動: {action}\n結果:";

        chatGPTInteraction.SendQuestion(prompt, DisplayResult);

        revealUIController.SetActionText(action);

        actionParent.SetActive(false);
        revealParent.SetActive(true);
    }

    void DisplayResult(string result)
    {
        Debug.Log(result);
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
