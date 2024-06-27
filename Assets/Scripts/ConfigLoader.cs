using System.IO;
using UnityEngine;


class ConfigLoader : MonoBehaviour
{
    public ChatGptConfig config;

    void Start()
    {
        string json = File.ReadAllText("chatgpt_config.json");

        Debug.Log(json != null);

        config = JsonUtility.FromJson<ChatGptConfig>(json);
        
        Debug.Log(config.API_URL);
        Debug.Log(config.API_KEY);
        Debug.Log(config.MODEL);
        Debug.Log(config.MAX_TOKENS);
        Debug.Log(config.TEMPERATURE);
    }
}


