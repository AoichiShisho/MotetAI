using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromptEditor : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI promptText;
    public TMP_InputField inputField;

    [Header("Buttons")]
    public Button changePromptButton;
    public Button editButton;
    public Button confirmChangeButtom;
    public Button confirmPromptButton;

    [Header("Prompts")]
    public string[] prompts;

    [Header("Parent Objects")]
    public GameObject promptParent;
    public GameObject actionParent;

    private int currentPromptIndex = 0;
    private bool isEditing = false;

    private ActionHandler actionHandler;

    void Start()
    {
        if (prompts.Length > 0)
            promptText.text = prompts[currentPromptIndex];

        actionHandler = GetComponent<ActionHandler>();
        if (actionHandler == null)
            Debug.LogError("ActionHandlerが見つかりません。");

        changePromptButton.onClick.AddListener(ChangePrompt);
        editButton.onClick.AddListener(EditPrompt);
        confirmChangeButtom.onClick.AddListener(ConfirmEdit);
        confirmPromptButton.onClick.AddListener(ConfirmPrompt); // ConfirmPromptButtonのリスナーを追加

        inputField.gameObject.SetActive(false);
        actionParent.SetActive(false);
    }

    void ChangePrompt()
    {
        if (prompts.Length > 0)
        {
            currentPromptIndex = (currentPromptIndex + 1) % prompts.Length;
            promptText.text = prompts[currentPromptIndex];
        }
    }

    void EditPrompt()
    {
        if (!isEditing)
        {
            inputField.text = promptText.text;
            inputField.gameObject.SetActive(true);
            confirmChangeButtom.gameObject.SetActive(true);
            changePromptButton.gameObject.SetActive(false);
            editButton.gameObject.SetActive(false);
            isEditing = true;
        }
    }

    void ConfirmEdit()
    {
        if (isEditing)
        {
            promptText.text = inputField.text;
            prompts[currentPromptIndex] = inputField.text;
            inputField.gameObject.SetActive(false);
            confirmChangeButtom.gameObject.SetActive(false);
            changePromptButton.gameObject.SetActive(true);
            editButton.gameObject.SetActive(true);
            isEditing = false;
        }
    }

    void ConfirmPrompt()
    {
        actionHandler.SetPrompt(promptText.text);

        promptParent.SetActive(false);
        actionParent.SetActive(true);
    }

    public string GetCurrentPrompt()
    {
        return promptText.text;
    }
}
