using System;

[Serializable]
public enum LobbyUIState {
    SELECT,
    JOIN,
    CREATE
}

public sealed class LobbyCanvasManager : CanvasManager<LobbyUIState> {
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
}
