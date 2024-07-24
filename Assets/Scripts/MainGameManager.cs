using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public static class PlayerActionResultStore
{
    public static Dictionary<string, PlayerActionResult> shared = new();
}

public class MainGameManager : MonoBehaviourPunCallbacks
{
    public PromptUIController promptUIController;
    public ActionUIController actionUIController;
    public RevealUIController revealUIController;
    public AnswerUIController answerUIController;
    public ChatGPTInteraction chatGPTInteraction;
    public TextMeshProUGUI waitingText;
    public GameObject waitingParent;
    public GameObject actionParent;
    public GameObject actionWaitingParent;
    public GameObject disconnectPanel;
    public Button nextResultButton;
    public Button returnToTitleButton;

    private int currentActionIndex;

    void Start()
    {
        Debug.Log("MainGameManager Start called");
        currentActionIndex = 0;
        photonView.RPC(nameof(SetupUI), RpcTarget.All, PhotonNetwork.NickName);
        nextResultButton.onClick.AddListener(OnNextResultButtonClicked);
        returnToTitleButton.onClick.AddListener(ReturnToTitle);
        disconnectPanel.SetActive(false);
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
            photonView.RPC(nameof(NotifyOtherPlayers), RpcTarget.All, PhotonNetwork.NickName, promptUIController.GetCurrentPrompt());
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC(nameof(HandlePlayerLeft), RpcTarget.All);
    }

    [PunRPC]
    void HandlePlayerLeft()
    {
        disconnectPanel.SetActive(true);
    }

    void ReturnToTitle()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Title");
    }

    public void SubmitAction(string action)
    {
        ShowWaitingUI();
        photonView.RPC(nameof(RPC_SubmitAction), RpcTarget.MasterClient, PhotonNetwork.NickName, action);
    }

    [PunRPC]
    void RPC_SubmitAction(string playerName, string action)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        Debug.Log($"RPC_SubmitAction called by {playerName} with action: {action}");

        LogPlayerActions();
        photonView.RPC(nameof(UpdateAllClients), RpcTarget.All, playerName, action);
    }

    [PunRPC]
    void UpdateAllClients(string playerName, string action)
    {
        if (!PlayerActionResultStore.shared.ContainsKey(playerName))
        {
            PlayerActionResultStore.shared.Add(playerName, new PlayerActionResult { Action = action, Result = "" });
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            PlayerActionResultStore.shared[playerName].Action = action;
        }
        
        LogPlayerActions();
        CheckAllActionsSubmitted();
    }

    void CheckAllActionsSubmitted()
    {
        if (PlayerActionResultStore.shared.Count == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC(nameof(HideActionWaitingParents), RpcTarget.All);
            photonView.RPC(nameof(DisplayNextAction), RpcTarget.All);
        }
        else
        {
            Debug.Log("Not all actions have been submitted yet.");
        }
    }

    void LogPlayerActions()
    {
        string logMessage = "Current PlayerActionResultStore:\n";
        foreach (var kvp in PlayerActionResultStore.shared)
        {
            logMessage += $"Player: {kvp.Key}, Action: {kvp.Value.Action}, Result: {kvp.Value.Result}\n";
        }
        Debug.Log(logMessage);
    }

    [PunRPC]
    void DisplayNextAction()
    {
        Debug.Log("currentActionIndex: " + currentActionIndex);
        if (currentActionIndex < PhotonNetwork.PlayerList.Length)
        {
            string playerName = PhotonNetwork.PlayerList[currentActionIndex].NickName;
            string action = PlayerActionResultStore.shared[playerName].Action;
            revealUIController.SetActionText(action, playerName);
            revealUIController.proceedButton.onClick.RemoveAllListeners();
            revealUIController.proceedButton.onClick.AddListener(OnProceedButtonClicked);
            photonView.RPC(nameof(ShowRevealParent), RpcTarget.All);
        }
        else
        {
            Debug.Log("All actions have been displayed.");
        }
    }

    [PunRPC]
    void ShowRevealParent()
    {
        revealUIController.revealParent.SetActive(true);
        answerUIController.resultText.transform.parent.gameObject.SetActive(false);
        nextResultButton.gameObject.SetActive(false);
    }

    [PunRPC]
    void HideRevealParent()
    {
        revealUIController.revealParent.SetActive(false);
    }

    [PunRPC]
    void HideActionWaitingParents()
    {
        actionWaitingParent.SetActive(false);
    }

    void ShowWaitingUI()
    {
        actionParent.SetActive(false);
        actionWaitingParent.SetActive(true);
    }

    void OnProceedButtonClicked()
    {
        photonView.RPC(nameof(HideRevealParent), RpcTarget.All);

        string playerName = PhotonNetwork.PlayerList[currentActionIndex].NickName;
        string action = PlayerActionResultStore.shared[playerName].Action;
        string prompt = promptUIController.GetCurrentPrompt();
        string fullPrompt = $"{prompt}\nプレイヤーの行動: {action}\n結果:";

        chatGPTInteraction.SendQuestion(fullPrompt, result => {
            string replacedNameInJpn = result.Replace("プレイヤー", playerName);
            string replacedNameInEng = replacedNameInJpn.Replace("Player", playerName);
            string finalReplace = replacedNameInEng.Replace("player", playerName);
            
            string finalResult;

            Debug.Log(finalReplace);

            if (finalReplace.Contains("モテる"))
            {
                finalResult = "モテる！";
            }
            else if (finalReplace.Contains("モテない"))
            {
                finalResult = "モテない...";
            }
            else
            {
                finalResult = "判定不可";
            }

            photonView.RPC(nameof(DisplayResult), RpcTarget.All, finalReplace, finalResult);
            photonView.RPC(nameof(UpdatePlayerResult), RpcTarget.All, playerName, finalResult);
        });
    }
    
    [PunRPC]
    void UpdatePlayerResult(string playerName, string result)
    {
        if (PlayerActionResultStore.shared.ContainsKey(playerName))
        {
            PlayerActionResultStore.shared[playerName].Result = result;
        }
    }

    [PunRPC]
    void DisplayResult(string actionText, string result)
    {
        Debug.Log($"Displaying action text: {actionText}");
        Debug.Log($"Displaying result: {result}");
        answerUIController.DisplayAnswer(actionText, result);
        answerUIController.resultText.transform.parent.gameObject.SetActive(true);
        nextResultButton.gameObject.SetActive(true);

        SoundManager.Instance.PlayResultSound(result);
    }

    [PunRPC]
    void OnNextResultButtonClicked()
    {
        photonView.RPC(nameof(RPC_OnNextResultButtonClicked), RpcTarget.All);
    }

    [PunRPC]
    void RPC_OnNextResultButtonClicked()
    {
        answerUIController.resultText.transform.parent.gameObject.SetActive(false);
        nextResultButton.gameObject.SetActive(false);

        if (currentActionIndex < PhotonNetwork.PlayerList.Length - 1)
        {
            currentActionIndex++;
            photonView.RPC(nameof(DisplayNextAction), RpcTarget.All);
        }
        else
        {
            Debug.Log("All actions have been displayed.");
            photonView.RPC(nameof(LoadResultScene), RpcTarget.All);
            currentActionIndex = 0; // リセット
        }
    }

    [PunRPC]
    void LoadResultScene()
    {
        SceneManager.LoadScene("Result");
    }

    public void NotifyOtherPlayersRPC(string playerName, string prompt)
    {
        photonView.RPC(nameof(NotifyOtherPlayers), RpcTarget.All, playerName, prompt);
    }

    [PunRPC]
    void NotifyOtherPlayers(string playerName, string prompt)
    {
        waitingParent.SetActive(false);
        actionParent.SetActive(true);
        actionUIController.SetPrompt(prompt);
    }
}

public class PlayerActionResult
{
    public string Action { get; set; }
    public string Result { get; set; }
}
