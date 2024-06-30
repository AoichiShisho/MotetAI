using System.IO;
using UnityEngine;

class ConfigLoader
{
    public static Config Load()
    {
        string json = File.ReadAllText("Assets/Plugins/ChatGPT/Configs/config.json");
        return JsonUtility.FromJson<Config>(json);
    }
}
