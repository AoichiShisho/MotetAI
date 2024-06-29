using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyUIController : MonoBehaviourPunCallbacks
{
    public GameObject playerListParent;
    public GameObject playerListItemPrefab;
    public TMP_Text lobbyIDText;
    public Button startGameButton;

    void Start()
    {
        UpdatePlayerList();
        UpdateLobbyID();
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    void UpdateLobbyID()
    {
        if (PhotonNetwork.CurrentRoom != null)
            lobbyIDText.text = $"ロビーID: {PhotonNetwork.CurrentRoom.Name}";
    }

    void UpdatePlayerList()
    {
        // 既存のリストアイテムを削除する
        foreach (Transform child in playerListParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddPlayerToList(player);
        }
    }

    void AddPlayerToList(Player player)
    {
        GameObject listItem = Instantiate(playerListItemPrefab, playerListParent.transform);
        listItem.GetComponentInChildren<TMP_Text>().text = $"{player.NickName}";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerToList(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    void OnStartGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartGame", RpcTarget.All);
        }
        else
        {
            Debug.Log("ゲームマスターじゃないと始められません");
        }
    }

    [PunRPC]
    void StartGame()
    {
        PhotonNetwork.LoadLevel("Main");
    }
}
