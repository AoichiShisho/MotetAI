using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CHATGPT.OpenAI {
    public class ChatGPTConnection {
        private readonly Config config;
        private readonly List<Message> _messageList = new();

        public ChatGPTConnection() {
            this.config = ConfigLoader.Load();
            var initialMessage = TextLoader.Load();
            _messageList.Add(Message.FromUser(initialMessage));
        }

        // メッセージを送信して応答を受け取る非同期メソッド
        public async UniTask<Response> RequestAsync(string userMessage) {
            _messageList.Add(Message.FromUser(userMessage));
            var headers = new Dictionary<string, string> {
                {"Authorization", "Bearer " + config.API_KEY},
                {"Content-type", "application/json"},
                {"X-Slack-No-Retry", "1"}
            };
            Options options = new() {
                model = config.MODEL,
                messages = _messageList,
                max_tokens = config.MAX_TOKENS,
                temperature = config.TEMPERATURE
            };
            var jsonOptions = options.ToJson();

            using var request = new UnityWebRequest(config.API_URL, "POST") {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            foreach (var header in headers) {
                request.SetRequestHeader(header.Key, header.Value);
            }
            await request.SendWebRequest();
            if (request.IsError()) {
                Debug.LogError("UnityWebRequest Error: " + request.error);
                throw new Exception(request.error);
            } else {
                var responseString = request.downloadHandler.text;
                var responseObject = JsonUtility.FromJson<Response>(responseString);
                _messageList.Add(responseObject.choices[0].message);
                return responseObject;
            }
        }
    }    
}
