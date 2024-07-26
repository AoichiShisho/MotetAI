using System.IO;
using UnityEngine;

class TextLoader
{
    public static string Load()
    {
        TextAsset promptText = Resources.Load<TextAsset>("Configs/init_prompt");
        return promptText.text;
    }
}
