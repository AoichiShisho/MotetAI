using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultViewModel : MonoBehaviour
{
    [SerializeField] private GameObject playerList;
    [SerializeField] private TMP_Text playerResultTextPrefab;
    [SerializeField] private ScreenTransition screenTransition;
    
    private void Start()
    {
        ShowPlayerList();
        screenTransition.SetInitialLeftPosition();
    }
    
    private void ShowPlayerList()
    {
        foreach (var variable in PlayerActionResultStore.shared)
        {
            TMP_Text listItem = Instantiate(playerResultTextPrefab, playerList.transform);
            listItem.text = $"{variable.Key} は {variable.Value.Result}";
        }
    }

    public void NavigateToLobby()
    {
        screenTransition.EnterTransition().OnComplete(LoadLobbyScene);
    }

    private void LoadLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}