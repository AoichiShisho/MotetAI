using System.Collections.Generic;
using System.IO;

class ScenarioLoader
{
    public static List<string>  Load()
    {
        string fullText = File.ReadAllText("Assets/Scripts/UIController/Main/scenario_pattern.txt");
        
        return SplitStringByNewLine(fullText);
    }
    static List<string> SplitStringByNewLine(string input)
    {
        // '\n' で文字列を分割し、Listに変換
        string[] splitArray = input.Split(new[] { '\n' }, System.StringSplitOptions.None);
        return new List<string>(splitArray);
    }
}