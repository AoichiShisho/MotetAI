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
    }
}


