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
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text textAmount;
    private PhotonManager photonManager;

    protected override void Start()
    {
        base.Start();
        photonManager = GetComponent<PhotonManager>();
        inputField.onValueChanged.AddListener(UpdateTextAmount);
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

    public void OnConfirmNameButtonClicked()
    {
        string playerName = inputField.text;
        if (string.IsNullOrEmpty(playerName)) return;
        
        photonManager.SetPlayerName(playerName);

        LobbyUIState currentState = GetCurrentState();

        if (currentState == LobbyUIState.CREATE)
        {
            photonManager.OnCreateLobbyButtonClicked();
        }

        if (currentState == LobbyUIState.JOIN)
        {
            photonManager.OnJoinButtonClicked();
        }
    }

    void UpdateTextAmount(string text)
    {
        int currentLength = text.Length;
        int maxChars = inputField.characterLimit;

        textAmount.text = $"{currentLength}/{maxChars}";

        if (currentLength >= maxChars)
            textAmount.color = new Color32(255, 87, 87, 255); // FF5757
        else
            textAmount.color = new Color32(87, 87, 87, 255); // 575757
    }
}
