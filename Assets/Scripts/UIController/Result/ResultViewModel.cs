using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultViewModel : MonoBehaviour
{
    [SerializeField] private GameObject playerList;
    [SerializeField] private TMP_Text playerResultTextPrefab;
    
    private void Start()
    {
        ShowPlayerList();
    }
    
    private void ShowPlayerList()
    {
        foreach (var variable in PlayerSctionResultStore.shared)
        {
            TMP_Text listItem = Instantiate(playerResultTextPrefab, playerList.transform);
            listItem.text = $"{variable.Key} は {variable.Value.Result}";
        }
    }
}
