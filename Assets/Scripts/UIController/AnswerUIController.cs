using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI answerSceneText;
    public TextMeshProUGUI resultText;

    public void DisplayAnswer(string answer)
    {
        answerSceneText.text = answer;

        if (answer.Contains("モテる"))
            resultText.text = "プレイヤーはモテる!"; // 編集必要
        else
            resultText.text = "プレイヤーはモテない...";
    }
}
