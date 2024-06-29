using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string lobbySceneName = "Lobby";

    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();    
    }

    public void SetPlayerName(string playerName)
    {
        PhotonNetwork.NickName = playerName;
    }

    public void OnCreateLobbyButtonClicked()
    {
        string roomName = GenerateUniqueRoomID();
        PhotonNetwork.CreateRoom(roomName, new RoomOptions());
    }

    public void OnJoinButtonClicked(string roomId)
    {
        if (!string.IsNullOrEmpty(roomId)) {
            PhotonNetwork.JoinRoom(roomId);
        } else {
            Debug.LogError("Room name is empty!");
            // ここにUIを表示する処理を追加
        }
    }

    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["accountName"] = PhotonNetwork.NickName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

        PhotonNetwork.LoadLevel(lobbySceneName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room: " + message);
        // ここにUIを表示する処理を追加
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel(lobbySceneName);
    }

    private string GenerateUniqueRoomID()
    {
        string roomId;
        do {
            roomId = Random.Range(1000, 9999).ToString();
        }
        while (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name == roomId);
    
        return roomId;
    }
}
