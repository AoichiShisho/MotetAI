using System.IO;
using UnityEngine;

class ConfigLoader
{
    public static Config LoadConfig()
    {
        string json = File.ReadAllText("Assets/Scripts/ChatGPT/Configs/config.json");
        return JsonUtility.FromJson<Config>(json);
    }
}
