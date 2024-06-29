using System;
using TMPro;
using UnityEngine;

[Serializable]
public enum LobbyUIState {
    SELECT,
    JOIN,
    CREATE
}

public sealed class LobbyViewModel : CanvasManager<LobbyUIState> {
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField roomIdInputField;
    [SerializeField] private TMP_Text textAmount;
    [SerializeField] private PhotonManager photonManager;

    protected override void Start()
    {
        base.Start();
        photonManager = GetComponent<PhotonManager>();
        nameInputField.onValueChanged.AddListener(UpdateTextAmount);
    }

    public void OnLobbySelectButtonClicked()
    {
        SetUIState(LobbyUIState.SELECT);
    }

    public void OnLobbyCreateButtonClicked()
    {
        SetUIState(LobbyUIState.CREATE);
    }

    public void OnLobbyJoinButtonClicked()
    {
        SetUIState(LobbyUIState.JOIN);
    }

    public void OnConfirmCreateButtonClicked()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName)) return;

        photonManager.SetPlayerName(playerName);
        photonManager.OnCreateLobbyButtonClicked();
    }

    public void OnConfirmJoinButtonClicked()
    {
        string roomId = roomIdInputField.text;
        if (string.IsNullOrEmpty(roomId)) return;

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
