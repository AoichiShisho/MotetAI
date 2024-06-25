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
}
