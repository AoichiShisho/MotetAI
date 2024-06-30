using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public static class PlayerSctionResultStore
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
    public GameObject promptParent;
    public GameObject actionParent;
    public GameObject actionWaitingParent;
    public GameObject disconnectPanel;
    public Button nextResultButton;
    public Button returnToTitleButton;
    private Dictionary<string, PlayerActionResult> playerActionResults = new Dictionary<string, PlayerActionResult>();

    private int currentActionIndex = 0;

    void Start()
    {
        Debug.Log("MainGameManager Start called");
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
        photonView.RPC(nameof(RPC_SubmitAction), RpcTarget.All, PhotonNetwork.NickName, action);
        ShowWaitingUI();
    }

    [PunRPC]
    void RPC_SubmitAction(string playerName, string action)
    {
        if (!PlayerSctionResultStore.shared.ContainsKey(playerName))
        {
            PlayerSctionResultStore.shared[playerName] = new PlayerActionResult { Action = action, Result = "" };
        }
        CheckAllActionsSubmitted();
    }

    void CheckAllActionsSubmitted()
    {
        if (PlayerSctionResultStore.shared.Count == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC(nameof(HideActionWaitingParents), RpcTarget.All);
            photonView.RPC(nameof(DisplayNextAction), RpcTarget.All);
        }
    }

    [PunRPC]
    void DisplayNextAction()
    {
        if (currentActionIndex < PhotonNetwork.PlayerList.Length)
        {
            string playerName = PhotonNetwork.PlayerList[currentActionIndex].NickName;
            string action = PlayerSctionResultStore.shared[playerName].Action;
            revealUIController.SetActionText(action);
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
        string action = PlayerSctionResultStore.shared[playerName].Action;
        string prompt = promptUIController.GetCurrentPrompt();
        string fullPrompt = $"{prompt}\nプレイヤーの行動: {action}\n結果:";

        chatGPTInteraction.SendQuestion(fullPrompt, result => {
            string personalizedResult = result.Replace("プレイヤー", playerName);
            string finalResult;
            
            Debug.Log(result);
            
            
            //FIXME: このresult判定のテキストうまく行ってない。
            if (result.Contains("モテる"))
            {
                finalResult = $"{playerName}はモテる！";
            }
            else if (result.Contains("モテない"))
            {
                finalResult = $"{playerName}はモテない...";
            }
            else
            {
                finalResult = "判定不可";
            }

            photonView.RPC(nameof(DisplayResult), RpcTarget.All, personalizedResult, finalResult);

            PlayerSctionResultStore.shared[playerName].Result = finalResult;

            photonView.RPC(nameof(DisplayResult), RpcTarget.All, actionText, finalResult);
        });
    }

    [PunRPC]
    void DisplayResult(string actionText, string result)
    {
        Debug.Log($"Displaying action text: {actionText}");
        Debug.Log($"Displaying result: {result}");
        answerUIController.DisplayAnswer(actionText, result);
        answerUIController.resultText.transform.parent.gameObject.SetActive(true);
        nextResultButton.gameObject.SetActive(true);
    }

    void OnNextResultButtonClicked()
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
            SceneManager.LoadScene("Result");
            currentActionIndex = 0; // リセット
        }
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