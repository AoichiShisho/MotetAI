using System;
using TMPro;
using UnityEngine;

[Serializable]
public enum TitleUIState {
    SELECT,
    INPUT_ID,
    INPUT_NAME,
}

public enum JoinMode {
    EMPTY,
    CREATE,
    JOIN,
}

public sealed class TitleViewModel : CanvasManager<TitleUIState> {
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField roomIdInputField;
    [SerializeField] private TMP_Text textAmount;
    [SerializeField] private PhotonManager photonManager;
    [SerializeField] private ScreenTransition screenTransition;
    private JoinMode joinMode = JoinMode.EMPTY;

    protected override void Start()
    {
        base.Start();
        nameInputField.onValueChanged.AddListener(UpdateTextAmount);
        screenTransition.SetInitialLeftPosition();
    }

    public void OnLobbySelectButtonClicked()
    {
        SetUIState(TitleUIState.SELECT);
    }

    public void NavigateInputNameUI()
    {
        SetUIState(TitleUIState.INPUT_NAME);
    }

    public void NavigateInputIdUI()
    {
        string playerName = nameInputField.text;
        photonManager.SetPlayerName(playerName);

        if (!string.IsNullOrEmpty(playerName))
        {
            SetUIState(TitleUIState.INPUT_ID);
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
            default:
                break;
        }
    }

    void ConfirmCreateLobby()
    {
        string playerName = nameInputField.text;
        //if (string.IsNullOrEmpty(playerName)) return;

        screenTransition.EnterTransition(() =>
        {
            photonManager.SetPlayerName(playerName);
            photonManager.OnCreateLobbyButtonClicked(playerName);
        });
    }

    public void ConfirmJoinLobby()
    {
        string roomId = roomIdInputField.text;

        screenTransition.EnterTransition(() =>
        {
            photonManager.OnJoinButtonClicked(roomId);
        });
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
