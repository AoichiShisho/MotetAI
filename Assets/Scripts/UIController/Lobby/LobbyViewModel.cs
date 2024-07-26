using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyViewModel : MonoBehaviourPunCallbacks
{
    public GameObject MasterParent;
    public GameObject GuestParent;

    void Start()
    {
        UpdateUI();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        bool isMaster = PhotonNetwork.IsMasterClient;
        MasterParent.SetActive(isMaster);
        GuestParent.SetActive(!isMaster);
    }
}
