using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string lobbySceneName = "Lobby";
    [SerializeField] private GameObject errorImage;
    [SerializeField] private TextMeshProUGUI errorText;

    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();    
    }

    public async void SetPlayerName(string playerName)
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            PhotonNetwork.NickName = playerName;
        }
        else 
        {
            Debug.LogError("名前がない");
            errorImage.SetActive(true);
            errorText.text = "名前を入力してください。";

            await UniTask.Delay(1000);
            errorImage.SetActive(false);
        }
    }

    public void OnCreateLobbyButtonClicked(string playerName)
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            string roomName = GenerateUniqueRoomID();
            PhotonNetwork.CreateRoom(roomName, new RoomOptions());
        }
    }

    public async void OnJoinButtonClicked(string roomId)
    {
        bool isExistName = !string.IsNullOrEmpty(roomId);
        bool isExistRoom = PhotonNetwork.JoinRoom(roomId);

        if (isExistRoom) return;

        if (!isExistName)
        {
            Debug.LogError("ルームIDが入力されていません");
            errorImage.SetActive(true);
            errorText.text = "ルームIDを入力してください。";

            await UniTask.Delay(1000);
            errorImage.SetActive(false);
        }
    }

    public override async void OnJoinRoomFailed (short returnCode, string roomId)
    {
        Debug.LogError($"RoomId: { roomId }は存在しません");
        errorImage.SetActive(true);
        errorText.text = "ルームIDが存在しません。";

        await UniTask.Delay(1000);
        errorImage.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
        customProperties["accountName"] = PhotonNetwork.NickName;
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

        PhotonNetwork.LoadLevel(lobbySceneName);
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

    public void ReturnToTitle()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Title");
    }
}
