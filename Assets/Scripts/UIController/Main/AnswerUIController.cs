using UnityEngine;
using TMPro;

public class AnswerUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI answerSceneText;
    public TextMeshProUGUI resultText;

    public void DisplayAnswer(string actionText, string result)
    {
        answerSceneText.text = actionText;
        resultText.text = result;
    }
}
