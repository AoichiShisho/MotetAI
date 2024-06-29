using System;
using TMPro;
using UnityEngine;

[Serializable]
public enum LobbyUIState {
    SELECT,
    INPUT_ID,
    INPUT_NAME,
}

public enum JoinMode {
    EMPTY,
    CREATE,
    JOIN,
}

public sealed class LobbyViewModel : CanvasManager<LobbyUIState> {
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField roomIdInputField;
    [SerializeField] private TMP_Text textAmount;
    [SerializeField] private PhotonManager photonManager;
    private JoinMode joinMode = JoinMode.EMPTY;

    protected override void Start()
    {
        base.Start();
        nameInputField.onValueChanged.AddListener(UpdateTextAmount);
    }

    public void OnLobbySelectButtonClicked()
    {
        SetUIState(LobbyUIState.SELECT);
    }

    public void NavigateInputNameUI()
    {
        joinMode = JoinMode.CREATE;
        SetUIState(LobbyUIState.INPUT_NAME);
    }

    public void NavigateInputIdUI()
    {
        joinMode = JoinMode.JOIN;
        SetUIState(LobbyUIState.INPUT_ID);
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
                ConfirmJoinLobby();
                break;
            default:
                break;
        }
    }

    void ConfirmCreateLobby()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName)) return;

        photonManager.SetPlayerName(playerName);
        photonManager.OnCreateLobbyButtonClicked();
    }

    void ConfirmJoinLobby()
    {
        string playerName = nameInputField.text;
        string roomId = roomIdInputField.text;
        if (string.IsNullOrEmpty(roomId)) return;
        if (string.IsNullOrEmpty(playerName)) return;

        photonManager.SetPlayerName(playerName);
        photonManager.OnJoinButtonClicked(roomId);
    }

    void UpdateTextAmount(string text)
    {
        int currentLength = text.Length;
        int maxChars = nameInputField.characterLimit;

        textAmount.text = $"{currentLength}/{maxChars}";

        if (currentLength >= maxChars)
            textAmount.color = new Color32(255, 87, 87, 255); // FF5757
        else
            textAmount.color = new Color32(87, 87, 87, 255); // 575757
    }
}
