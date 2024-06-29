using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections.Generic;

public class MainGameManager : MonoBehaviourPunCallbacks
{
    public PromptUIController promptUIController;
    public ActionUIController actionUIController;
    public RevealUIController revealUIController;
    public AnswerUIController answerUIController;
    public ChatGPTInteraction chatGPTInteraction;
    public TextMeshProUGUI waitingText;
    public GameObject waitingParent;
    public GameObject promptParent;
    public GameObject actionParent;
    public GameObject actionWaitingParent;

    private Dictionary<string, string> playerActions = new Dictionary<string, string>();
    private int currentActionIndex = 0;

    void Start()
    {
        Debug.Log("MainGameManager Start called");
        photonView.RPC(nameof(SetupUI), RpcTarget.All, PhotonNetwork.NickName);
    }

    [PunRPC]
    void SetupUI(string playerName)
    {
        Debug.Log($"SetupUI called. IsMasterClient: {PhotonNetwork.IsMasterClient}");
        promptUIController.InitializeUI();
        playerName = promptUIController.masterClientAccountName;
        waitingText.text = $"{playerName}がシナリオを考え中...";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("NotifyOtherPlayers", RpcTarget.All, PhotonNetwork.NickName, promptUIController.GetCurrentPrompt());
        }
    }

    public void SubmitAction(string action)
    {
        photonView.RPC("RPC_SubmitAction", RpcTarget.All, PhotonNetwork.NickName, action);
        ShowWaitingUI();
    }

    [PunRPC]
    void RPC_SubmitAction(string playerName, string action)
    {
        playerActions[playerName] = action;
        CheckAllActionsSubmitted();
    }

    void CheckAllActionsSubmitted()
    {
        if (playerActions.Count == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("DisplayNextAction", RpcTarget.All);
        }
    }

    [PunRPC]
    void DisplayNextAction()
    {
        if (currentActionIndex < PhotonNetwork.PlayerList.Length)
        {
            string playerName = PhotonNetwork.PlayerList[currentActionIndex].NickName;
            string action = playerActions[playerName];
            revealUIController.SetActionText(action);
            revealUIController.proceedButton.onClick.RemoveAllListeners();
            revealUIController.proceedButton.onClick.AddListener(OnProceedButtonClicked);
            revealUIController.revealParent.SetActive(true);
            currentActionIndex++;
        }
        else
        {
            Debug.Log("All actions have been displayed.");
        }
    }

    void ShowWaitingUI()
    {
        actionParent.SetActive(false);
        actionWaitingParent.SetActive(true);
    }

    void OnProceedButtonClicked()
    {
        revealUIController.revealParent.SetActive(false);

        if (currentActionIndex <= PhotonNetwork.PlayerList.Length)
        {
            string playerName = PhotonNetwork.PlayerList[currentActionIndex - 1].NickName;
            string action = playerActions[playerName];
            string prompt = promptUIController.GetCurrentPrompt();
            string fullPrompt = $"{prompt}\nプレイヤーの行動: {action}\n結果:";

            chatGPTInteraction.SendQuestion(fullPrompt, DisplayResult);
        }
        else
        {
            Debug.Log("All actions have been displayed.");
        }
    }

    void DisplayResult(string result)
    {
        answerUIController.DisplayAnswer(result);

        if (currentActionIndex < PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("DisplayNextAction", RpcTarget.All);
        }
        else
        {
            revealUIController.proceedButton.onClick.RemoveListener(OnProceedButtonClicked);
        }
    }

    public void NotifyOtherPlayersRPC(string playerName, string prompt)
    {
        photonView.RPC("NotifyOtherPlayers", RpcTarget.All, playerName, prompt);
    }

    [PunRPC]
    void NotifyOtherPlayers(string playerName, string prompt)
    {
        /*Debug.Log($"NotifyOtherPlayers called with playerName: {playerName} and prompt: {prompt}");
        waitingText.text = $"{playerName}がシナリオを考え中...";*/
        waitingParent.SetActive(false);
        actionParent.SetActive(true);
        actionUIController.SetPrompt(prompt);
    }
}
