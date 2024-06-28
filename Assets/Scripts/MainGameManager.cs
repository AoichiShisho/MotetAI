using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MainGameManager : MonoBehaviourPunCallbacks
{
    public PromptUIController promptUIController;
    public TextMeshProUGUI waitingText;
    public GameObject waitingParent;
    public GameObject promptParent;

    void Start()
    {
        Debug.Log("MainGameManager Start called");
        photonView.RPC("SetupUI", RpcTarget.All);
    }

    [PunRPC]
    void SetupUI()
    {
        Debug.Log($"SetupUI called. IsMasterClient: {PhotonNetwork.IsMasterClient}");
        promptUIController.InitializeUI();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("NotifyOtherPlayers", RpcTarget.Others, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    void NotifyOtherPlayers(string playerName)
    {
        waitingText.text = $"{playerName}がシナリオを考え中...";
        waitingParent.SetActive(true);
    }
}
