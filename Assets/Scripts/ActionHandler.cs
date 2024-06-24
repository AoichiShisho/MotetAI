using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionHandler : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI promptText;
    public TMP_InputField actionInputField;
    public TextMeshProUGUI resultText;
    public Button submitActionButton;

    private PromptEditor promptEditor;
    private ChatGPTClient chatGPTClient;

    void Start()
    {
        promptEditor = FindObjectOfType<PromptEditor>();
        chatGPTClient = FindObjectOfType<ChatGPTClient>();

        submitActionButton.onClick.AddListener(SubmitAction);
        // resultText.gameObject.SetActive(false);
    }

    public void SetPrompt(string prompt)
    {
        promptText.text = prompt;
    }

    void SubmitAction()
    {
        string scenario = promptEditor.GetCurrentPrompt();
        string action = actionInputField.text;
        // string prompt = $"{scenario}\nプレイヤーの行動: {action}\n結果:";

        // StartCoroutine(chatGPTClient.SendRequest(prompt, DisplayResult));
    }

    void DisplayResult(string result)
    {
        resultText.text = result;
        resultText.gameObject.SetActive(true);
    }
}
