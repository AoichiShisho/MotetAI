using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromptEditor : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI promptText;
    public TMP_InputField inputField;
    
    [Header("Buttons")]
    public Button changeTextButton;
    public Button editButton;
    public Button confirmTextButton;
    
    [Header("Prompts")]
    public string[] prompts;

    private int currentPromptIndex = 0;
    private bool isEditing = false;

    void Start()
    {
        if (prompts.Length > 0)
            promptText.text = prompts[currentPromptIndex];
        
        changeTextButton.onClick.AddListener(ChangePrompt);
        editButton.onClick.AddListener(EditPrompt);
        confirmTextButton.onClick.AddListener(ConfirmEdit);

        editButton.onClick.AddListener(EditPrompt);
        inputField.gameObject.SetActive(false);
    }

    void ChangePrompt()
    {
        if (prompts.Length > 0) {
            currentPromptIndex = (currentPromptIndex + 1) % prompts.Length;
            promptText.text = prompts[currentPromptIndex];
        }
    }

    void EditPrompt()
    {
        if (!isEditing) {
            inputField.text = promptText.text;
            inputField.gameObject.SetActive(true);
            confirmTextButton.gameObject.SetActive(true);
            changeTextButton.gameObject.SetActive(false);
            editButton.gameObject.SetActive(false);
            isEditing = true;
        }
    }

    void ConfirmEdit()
    {
        if (isEditing) {
            promptText.text = inputField.text;
            prompts[currentPromptIndex] = inputField.text;
            inputField.gameObject.SetActive(false);
            confirmTextButton.gameObject.SetActive(false);
            changeTextButton.gameObject.SetActive(true);
            editButton.gameObject.SetActive(true);
            isEditing = false;
        }
    }
}