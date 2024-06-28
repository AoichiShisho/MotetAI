[System.Serializable]
public class Message {
    public string role;
    public string content;

    public static Message FromUser(string content) {
        return new Message { role = "user", content = content };
    }

    public static Message FromSystem(string content) {
        return new Message { role = "system", content = content };
    }
}
