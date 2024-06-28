using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromptUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI promptText;
    public TMP_InputField inputField;
    public TextMeshProUGUI textAmount;

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

    private ActionUIController actionUIController;

    void Start()
    {
        if (prompts.Length > 0)
            promptText.text = prompts[currentPromptIndex];

        actionUIController = GetComponent<ActionUIController>();
        if (actionUIController == null)
            Debug.LogError("ActionUIController is missing!");

        changePromptButton.onClick.AddListener(ChangePrompt);
        editButton.onClick.AddListener(EditPrompt);
        confirmChangeButtom.onClick.AddListener(ConfirmEdit);
        confirmPromptButton.onClick.AddListener(ConfirmPrompt);

        inputField.interactable = true;
        inputField.textComponent.enableWordWrapping = true;
        inputField.onValueChanged.AddListener(UpdateTextAmount);
        inputField.gameObject.SetActive(false);
        actionParent.SetActive(false);

        UpdateTextAmount(inputField.text);
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
            promptText.gameObject.SetActive(false);
            inputField.text = promptText.text;
            inputField.gameObject.SetActive(true);
            confirmChangeButtom.gameObject.SetActive(true);
            changePromptButton.gameObject.SetActive(false);
            editButton.gameObject.SetActive(false);
            confirmPromptButton.gameObject.SetActive(false);
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
            promptText.gameObject.SetActive(true);
            confirmPromptButton.gameObject.SetActive(true);
            isEditing = false;
        }
    }

    void ConfirmPrompt()
    {
        actionUIController.SetPrompt(promptText.text);

        promptParent.SetActive(false);
        actionParent.SetActive(true);
    }

    public string GetCurrentPrompt()
    {
        return promptText.text;
    }

    void UpdateTextAmount(string text)
    {
        int currentLength = text.Length;
        int maxChars = inputField.characterLimit;

        textAmount.text = $"{currentLength}/{maxChars}";

        if (currentLength >= maxChars)
            textAmount.color = new Color32(255, 87, 87, 255); // FF5757
        else
            textAmount.color = new Color32(87, 87, 87, 255); // 575757
    }
}
