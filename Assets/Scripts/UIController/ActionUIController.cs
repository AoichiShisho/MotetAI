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
    public GameObject revealParent;

    private PromptUIController promptUIController;
    private ChatGPTInteraction chatGPTInteraction;
    private RevealUIController revealUIController;

    void Start()
    {
        promptUIController = GetComponent<PromptUIController>();
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

    async void SubmitAction()
    {
        string scenario = promptUIController.GetCurrentPrompt();
        string action = actionInputField.text;
        string prompt = $"{scenario}\nプレイヤーの行動: {action}\n結果:";

        chatGPTInteraction.SendQuestion(prompt, DisplayResult);

        revealUIController.SetActionText(action);

        await FadeOut(actionParent);

        await FadeOut(actionWaitingParent);

        actionParent.SetActive(false);
        actionWaitingParent.SetActive(false);
        revealParent.SetActive(true);

        await FadeIn(revealParent);
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
