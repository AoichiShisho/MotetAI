namespace ChatGPT
{
    [System.Serializable]
    public class Response {
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
    