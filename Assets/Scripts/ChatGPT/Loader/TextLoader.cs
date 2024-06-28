using System.IO;
using UnityEngine;

class TextLoader
{
    public static string Load()
    {
        return File.ReadAllText("./Assets/Scripts/ChatGPT/Configs/init_prompt.txt");
    }
}
