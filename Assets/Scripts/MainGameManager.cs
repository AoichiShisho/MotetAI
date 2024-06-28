using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MainGameManager : MonoBehaviourPunCallbacks
{
    public PromptUIController promptUIController;
    public ActionUIController actionUIController;
    public TextMeshProUGUI waitingText;
    public GameObject waitingParent;
    public GameObject promptParent;
    public GameObject actionParent;

    void Start()
    {
        Debug.Log("MainGameManager Start called");
        photonView.RPC("SetupUI", RpcTarget.All, PhotonNetwork.NickName);
    }

    [PunRPC]
    void SetupUI(string playerName)
    {
        Debug.Log($"SetupUI called. IsMasterClient: {PhotonNetwork.IsMasterClient}");
        promptUIController.InitializeUI();
        waitingText.text = $"{playerName}がシナリオを考え中...";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("NotifyOtherPlayers", RpcTarget.All, PhotonNetwork.NickName);
        }
    }

    [PunRPC]
    void NotifyOtherPlayers(string playerName, string prompt)
    {
        Debug.Log($"NotifyOtherPlayers called with playerName: {playerName} and prompt: {prompt}");
        waitingText.text = $"{playerName}がシナリオを考え中...";
        waitingParent.SetActive(true);
        actionParent.SetActive(true); 

        actionUIController.SetPrompt(prompt);
    }
}
