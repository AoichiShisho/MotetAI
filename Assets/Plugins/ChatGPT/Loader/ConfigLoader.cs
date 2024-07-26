using System.IO;
using UnityEngine;

class ConfigLoader
{
    public static Config Load()
    {
        TextAsset configText = Resources.Load<TextAsset>("Configs/config");
        return JsonUtility.FromJson<Config>(configText.text);
    }
}

// using UnityEngine;
//
// class ConfigLoader
// {
//     public static Config Load()
//     {
//         string path = Path.Combine(Application.streamingAssetsPath, "Configs/config.json");
//         string json = File.ReadAllText(path);
//         return JsonUtility.FromJson<Config>(json);
//     }
// }
