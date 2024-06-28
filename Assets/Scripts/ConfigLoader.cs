using System.IO;
using UnityEngine;

class ConfigLoader : MonoBehaviour
{
    public Config config;
    public bool isConfigLoaded { get; private set; } = false;

    void Start()
    {
        LoadConfig();
    }

    private void LoadConfig()
    {
        string relativePath = "../chatgpt_config.json";
        string filePath = Path.Combine(Application.dataPath, relativePath);

        filePath = Path.GetFullPath(filePath);

        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                config = JsonUtility.FromJson<Config>(json);
                isConfigLoaded = true;
                Debug.Log("Config file loaded successfully.");
                Debug.Log("Loaded Config: " + JsonUtility.ToJson(config)); // 読み込んだ設定内容をデバッグ表示
            }
            else
            {
                Debug.LogError($"Config file not found at path: {filePath}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load config file at path: {filePath}. Exception: {ex.Message}");
        }
    }
}
