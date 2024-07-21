using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class LobbyViewModel : MonoBehaviourPunCallbacks
{
    public GameObject MasterParent;
    public GameObject GuestParent;

    void Start()
    {
        MasterParent.SetActive(PhotonNetwork.IsMasterClient);
        GuestParent.SetActive(!PhotonNetwork.IsMasterClient);
    }
}
