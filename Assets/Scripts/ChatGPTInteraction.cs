using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHATGPT.OpenAI;
using Cysharp.Threading.Tasks;
using System.Text.RegularExpressions;

public class ChatGPTInteraction : MonoBehaviour {
    [SerializeField] private string openAIApiKey;
    [SerializeField] private string modelVersion = "gpt-3.5-turbo";
    [SerializeField] private int maxTokens = 150;
    [SerializeField] private float temperature = 0.5f;
    [TextArea]
    [SerializeField] private string initialSystemMessage = "これから恋愛のシナリオと、ユーザーの行動を渡します。これらのシナリオを考えた時、ユーザーが取る行動からみて恋愛のシナリオは成功するか考えてください。また、返答としてはそのシナリオの続きを考え、最後にそのユーザーが恋愛が成功したか失敗したかを返してください。";

    private ChatGPTConnection chatGPTConnection;
    private const string FaceTagPattern = @"\[face:([^\]_]+)_?(\d*)\]";
    private const string InterestTagPattern = @"\[interest:(\d)\]";

    void Start() {
        chatGPTConnection = new ChatGPTConnection(openAIApiKey, initialSystemMessage, modelVersion, maxTokens, temperature);
    }

    public async void SendQuestion(string prompt, System.Action<string> callback) {
        var response = await chatGPTConnection.RequestAsync(prompt);
        string responseContent = response.choices[0].message.content;

        // 関心タグを抽出
        var interestMatch = Regex.Match(responseContent, InterestTagPattern);
        int interestLevel = -1; // 関心レベルの初期値を無効値に
        if (interestMatch.Success) {
            interestLevel = int.Parse(interestMatch.Groups[1].Value);
            Debug.Log($"関心レベル: {interestLevel}");
        }

        // 関心レベルが0の場合、返答を括弧で囲む(読み上げしない)
        if (interestLevel == 0) {
            responseContent = $"({responseContent})";
        }

        // 返答からタグ類を削除して純粋な返答のみにする
        string cleanedResponse = ExtractAndLogFaceTags(responseContent, interestLevel);
        callback(cleanedResponse);
    }

    private string ExtractAndLogFaceTags(string input, int interestLevel) {
        var matches = Regex.Matches(input, FaceTagPattern);
        var uniqueTags = new HashSet<string>();

        foreach (Match match in matches) {
            if (uniqueTags.Add(match.Value)) {
                Debug.Log("表情タグ全部: " + match.Value);
                string emotionTag = match.Groups[1].Value;
                string emotionIntensityString = match.Groups[2].Value;
                if (int.TryParse(emotionIntensityString, out int emotionIntensity)) {
                    Debug.Log($"表情: {emotionTag}, 強度: {emotionIntensity}");
                } else {
                    Debug.LogWarning($"表情の強度 '{emotionIntensityString}' を整数に変換できませんでした。");
                }
            }
        }

        var tempInput = Regex.Replace(input, FaceTagPattern, "");
        var cleanedInput = Regex.Replace(tempInput, InterestTagPattern, "");

        Debug.Log("ChatGPTの返答（表情タグ除去）: " + cleanedInput);
        return cleanedInput;
    }
}
