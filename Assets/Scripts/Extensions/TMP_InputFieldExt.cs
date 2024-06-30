using TMPro;
using UnityEngine;

public static class TMP_InputFieldExtensions
{
    public static void AddTextAmountUpdater(this TMP_InputField inputField, TextMeshProUGUI textAmount)
    {
        inputField.onValueChanged.AddListener((text) => UpdateTextAmount(inputField, textAmount, text));
    }
    
    public static void AddTextAmountUpdater(this TMP_InputField inputField, TMP_Text textAmount)
    {
        inputField.onValueChanged.AddListener((text) => UpdateTextAmount(inputField, textAmount, text));
    }

    private static void UpdateTextAmount(TMP_InputField inputField, TextMeshProUGUI textAmount, string text)
    {
        int currentLength = text.Length;
        int maxChars = inputField.characterLimit;

        textAmount.text = $"{currentLength}/{maxChars}";

        if (currentLength >= maxChars)
            textAmount.color = new Color32(255, 87, 87, 255); // FF5757
        else
            textAmount.color = new Color32(87, 87, 87, 255); // 575757
    }
    
    private static void UpdateTextAmount(TMP_InputField inputField, TMP_Text textAmount, string text)
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