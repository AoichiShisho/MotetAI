using TMPro;
using UnityEngine;

public sealed class LobbyViewModel : CanvasManager<LobbyUIState> {
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField roomIdInputField;
    [SerializeField] private TMP_Text textAmount;
    [SerializeField] private PhotonManager photonManager;
    private JoinMode joinMode = JoinMode.EMPTY;

    protected override void Start()
    {
        base.Start();
        nameInputField.AddTextAmountUpdater(textAmount);
    }

    public void OnLobbySelectButtonClicked()
    {
        SetUIState(LobbyUIState.SELECT);
    }

    public void NavigateInputNameUI()
    {
        SetUIState(LobbyUIState.INPUT_NAME);
    }

    public void NavigateInputIdUI()
    {
        string playerName = nameInputField.text;
        photonManager.SetPlayerName(playerName);

        if (!string.IsNullOrEmpty(playerName))
        {
            SetUIState(LobbyUIState.INPUT_ID);
        }
    }

    public void SetJoinMode()
    {
        joinMode = JoinMode.JOIN;
    }

    public void SetCreateMode()
    {
        joinMode = JoinMode.CREATE;
    }

    public void OnConfirmButtonClicked()
    {
        Debug.Log(joinMode);

        switch (joinMode)
        {
            case JoinMode.CREATE:
                ConfirmCreateLobby();
                break;
            case JoinMode.JOIN:
                NavigateInputIdUI();
                break;
        }
    }

    void ConfirmCreateLobby()
    {
        string playerName = nameInputField.text;

        photonManager.SetPlayerName(playerName);
        photonManager.OnCreateLobbyButtonClicked(playerName);
    }

    public void ConfirmJoinLobby()
    {
        string roomId = roomIdInputField.text;

        photonManager.OnJoinButtonClicked(roomId);
    }
}
