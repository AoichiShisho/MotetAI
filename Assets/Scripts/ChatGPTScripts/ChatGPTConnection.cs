using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CHATGPT.OpenAI {
    public class ChatGPTConnection {
        private readonly Config config;
        private readonly List<Message> _messageList = new();// ユーザーとシステムのメッセージリスト

        public ChatGPTConnection() {
            this.config = ConfigLoader.Load();
            var initialMessage = TextLoader.Load();
            _messageList.Add(Message.FromUser(initialMessage));
        }

        // メッセージを送信して応答を受け取る非同期メソッド
        public async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage) {
            _messageList.Add(Message.FromUser(userMessage));
            var headers = new Dictionary<string, string> {
                {"Authorization", "Bearer " + config.API_KEY},
                {"Content-type", "application/json"},
                {"X-Slack-No-Retry", "1"}
            };
            var options = new ChatGPTCompletionRequestModel() {
                model = config.MODEL,
                messages = _messageList,
                max_tokens = config.MAX_TOKENS,
                temperature = config.TEMPERATURE
            };
            var jsonOptions = JsonUtility.ToJson(options);
            Debug.Log("自分:" + userMessage);
            Debug.Log("API URL: " + config.API_URL); // ここでAPI URLをデバッグ表示
            Debug.Log("Request Payload: " + jsonOptions); // リクエストペイロードをデバッグ表示

            using var request = new UnityWebRequest(config.API_URL, "POST") {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            foreach (var header in headers) {
                request.SetRequestHeader(header.Key, header.Value);
            }
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("UnityWebRequest Error: " + request.error); // エラー詳細をデバッグ表示
                throw new Exception(request.error);
            } else {
                var responseString = request.downloadHandler.text;
                var responseObject = JsonUtility.FromJson<ChatGPTResponseModel>(responseString);
                _messageList.Add(responseObject.choices[0].message);
                return responseObject;
            }
        }
    }

    // APIリクエストの内容を定義
    [Serializable]
    public class ChatGPTCompletionRequestModel {
        public string model;
        public List<Message> messages;
        public int max_tokens;
        public float temperature;
    }

    // API応答の内容を定義
    [System.Serializable]
    public class ChatGPTResponseModel {
        public string id;
        public string @object;
        public int created;
        public Choice[] choices;
        public Usage usage;

        [System.Serializable]
        public class Choice {
            public int index;
            public Message message;
            public string finish_reason;
        }

        [System.Serializable]
        public class Usage {
            public int prompt_tokens;
            public int completion_tokens;
            public int total_tokens;
        }
    }
}
