using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultViewModel : MonoBehaviour
{
    [SerializeField] private GameObject playerList;
    [SerializeField] private TMP_Text playerResultTextPrefab;
    private readonly Dictionary<string, PlayerActionResult> _playerActionResults = new();
    
    private void Start()
    {
        _playerActionResults.Add("urassh1", new PlayerActionResult { Action = "action", Result = "モテる" });
        _playerActionResults.Add("urassh2", new PlayerActionResult { Action = "action", Result = "モテる" });
        _playerActionResults.Add("urassh3", new PlayerActionResult { Action = "action", Result = "モテない" });

        ShowPlayerList();
    }
    
    private void ShowPlayerList()
    {
        foreach (var variable in _playerActionResults)
        {
            TMP_Text listItem = Instantiate(playerResultTextPrefab, playerList.transform);
            listItem.text = $"{variable.Key} は {variable.Value.Result}";
        }
    }
}
