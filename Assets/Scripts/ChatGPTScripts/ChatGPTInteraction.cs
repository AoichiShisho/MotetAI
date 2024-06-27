using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHATGPT.OpenAI;
using System.Text.RegularExpressions;

public class ChatGPTInteraction : MonoBehaviour {
    [SerializeField] private ConfigLoader configLoader;
    [TextArea]
    [SerializeField] private string initialSystemMessage = "これから、あるゲームのシナリオとプレイヤーの行動を渡します。これらのシナリオからプレイヤーの行動を考慮して、その後の物語がどのように展開し、プレイヤーが勝利するか敗北するかを決めてください。また、勝利の場合は最後に「モテる！」と記述してください。以下の詳細に従って出力してください。\n\n## ゲーム内容\n- 引き渡したシナリオは、異性の身の安全が危ぶまれる状況です。\n- ゲームの内容は、引き渡したシナリオにおけるプレイヤーの行動で、その後異性と付き合うことができるかを決め、付き合えたら勝利、付き合えなかったら敗北とする、というものです。\n\n## 指示\n- 渡されたシナリオとプレイヤーの行動から、そのシナリオからどのような行動結果になるか考えてください。\n- そのプレイヤーが最終的に好きな人からどのようなリアクションや返答などが得られるかについて考えてください。\n- 考えるシナリオは少し突飛なものであっても構わないものとする。\n- プレイヤーから行動を取得した時、基本的にはフラれること前提で出力してしまって構いません。本当にモテるやつ、もしくはユーモアの塊みたいなケースにのみモテる、という結果の出力になるような確率にしてください。\n\n## 出力フォーマット\n- アドバイスなどはせず、その行動からどのような物語になるかのみ、出力してください。\n- 勝利の場合、文章の最後に「モテる！」と出力してください。";

    private ChatGPTConnection chatGPTConnection;
    private const string FaceTagPattern = @"\[face:([^\]_]+)_?(\d*)\]";
    private const string InterestTagPattern = @"\[interest:(\d)\]";

    [SerializeField] private AnswerUIController answerUIController;

    void Start() {
        chatGPTConnection = new ChatGPTConnection(configLoader.config, initialSystemMessage);
        answerUIController = GetComponent<AnswerUIController>();
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

        // ChatGPTの返答を表示
        answerUIController.DisplayAnswer(cleanedResponse);
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
