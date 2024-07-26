using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Cysharp.Threading.Tasks;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string lobbySceneName = "Lobby";
    [SerializeField] private ErrorController errorController;
    [SerializeField] private TextMeshProUGUI errorText;
    
    private string pendingRoomName;
    private System.Action pendingAction;

    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void SetPlayerName(string playerName)
    {
            PhotonNetwork.NickName = playerName;
    }

    public async void PlayerNameError()
    {
        Debug.LogError("名前がない");
        errorText.text = "名前を入力してください。";
        errorController.ShowError();

        await UniTask.Delay(1000);
    }

    public void OnCreateLobbyButtonClicked(string playerName)
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            pendingRoomName = GenerateUniqueRoomID();
            pendingAction = TryCreateRoom;
            if (PhotonNetwork.IsConnectedAndReady)
            {
                pendingAction.Invoke();
            }
        }
    }

    private void TryCreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.CreateRoom(pendingRoomName, new RoomOptions());
            pendingRoomName = null;
            pendingAction = null;
        }
        else
        {
            Debug.LogError("Photon is not connected and ready.");
        }
    }

    public void JoinRoom(string roomId)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRoom(roomId);
        }
        else
        {
            Debug.LogError("Photon is not connected and ready.");
        }
    }

    public async void RoomIdError()
    {
        Debug.LogError("ルームIDが入力されていません");
        errorText.text = "ルームIDを入力してください。";
        errorController.ShowError();

        await UniTask.Delay(1000);
    }

    public override async void OnJoinRoomFailed(short returnCode, string roomId)
    {
        Debug.LogError($"RoomId: { roomId }は存在しません");
        errorText.text = "ルームIDが存在しません。";
        errorController.ShowError();

        await UniTask.Delay(1000);
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

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master");
        pendingAction?.Invoke();
    }

    private string GenerateUniqueRoomID()
    {
        string roomId;
        do
        {
            roomId = Random.Range(1000, 9999).ToString();
        } while (PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.Name == roomId);

        return roomId;
    }

    public void ReturnToTitle()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Title");
    }
}
