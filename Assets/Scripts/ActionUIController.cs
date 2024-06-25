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

    private PromptEditor promptEditor;
    private ChatGPTInteraction chatGPTInteraction;

    void Start()
    {
        promptEditor = GetComponent<PromptEditor>();
        chatGPTInteraction = GetComponent<ChatGPTInteraction>();

        submitActionButton.onClick.AddListener(SubmitAction);
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
    }

    void DisplayResult(string result)
    {
        Debug.Log(result);
    }
}
