using System.Collections.Generic;
using ChatGPT;
using UnityEngine;

/// <summary>
/// APIリクエストの内容を定義(オプション)
/// </summary>

[System.Serializable]
class Options {
    public string model;
    public List<Message> messages;
    public int max_tokens;
    public float temperature;

    public string ToJson() {
        return JsonUtility.ToJson(this);
    }
}
