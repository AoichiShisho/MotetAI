using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CHATGPT.OpenAI {
    public class ChatGPTConnection {
        private readonly ChatGptConfig config;
        private readonly List<ChatGPTMessageModel> _messageList = new();// ユーザーとシステムのメッセージリスト

        public ChatGPTConnection(ChatGptConfig config, string initialMessage) {
            this.config = config;
            _messageList.Add(new ChatGPTMessageModel() { role = "system", content = initialMessage });
        }

 // メッセージを送信して応答を受け取る非同期メソッド
        public async UniTask<ChatGPTResponseModel> RequestAsync(string userMessage) {
            _messageList.Add(new ChatGPTMessageModel { role = "user", content = userMessage });
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
            using var request = new UnityWebRequest(config.API_URL, "POST") {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            foreach (var header in headers) {
                request.SetRequestHeader(header.Key, header.Value);
            }
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError(request.error);
                throw new Exception();
            } else {
                var responseString = request.downloadHandler.text;
                var responseObject = JsonUtility.FromJson<ChatGPTResponseModel>(responseString);
                _messageList.Add(responseObject.choices[0].message);
                return responseObject;
            }
        }
    }
 // メッセージの役割と内容を定義
    [Serializable]
    public class ChatGPTMessageModel {
        public string role;
        public string content;
    }

    // APIリクエストの内容を定義

    [Serializable]
    public class ChatGPTCompletionRequestModel {
        public string model;
        public List<ChatGPTMessageModel> messages;
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
            public ChatGPTMessageModel message;
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
