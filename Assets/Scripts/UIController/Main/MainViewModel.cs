using System;
using System.Collections.Generic;
using UnityEngine;

public class MainViewModel : MonoBehaviour
{
    public event Action<string> OnSetActionText;
    public event Action<bool> OnInitializeUI;

    private Dictionary<string, string> playerActions = new Dictionary<string, string>();
    private int currentActionIndex = 0;

    public void SubmitAction(string playerName, string action)
    {
        playerActions[playerName] = action;
        CheckAllActionsSubmitted();
    }

    private void CheckAllActionsSubmitted()
    {
        if (playerActions.Count == Photon.Pun.PhotonNetwork.PlayerList.Length)
        {
            DisplayNextAction();
        }
    }

    public void DisplayNextAction()
    {
        if (currentActionIndex < Photon.Pun.PhotonNetwork.PlayerList.Length)
        {
            string playerName = Photon.Pun.PhotonNetwork.PlayerList[currentActionIndex].NickName;
            string action = playerActions[playerName];
            OnSetActionText?.Invoke(action);
            currentActionIndex++;
        }
        else
        {
            Debug.Log("All actions have been displayed.");
        }
    }

    public void OnProceedButtonClicked()
    {
        DisplayNextAction();
    }

    public void NotifyOtherPlayers(string playerName, string prompt)
    {
        OnInitializeUI?.Invoke(Photon.Pun.PhotonNetwork.IsMasterClient);
    }
}
