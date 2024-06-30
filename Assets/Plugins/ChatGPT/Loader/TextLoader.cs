using System.IO;
using UnityEngine;

class TextLoader
{
    public static string Load()
    {
        return File.ReadAllText("Assets/Plugins/ChatGPT/Configs/init_prompt.txt");
    }
}
