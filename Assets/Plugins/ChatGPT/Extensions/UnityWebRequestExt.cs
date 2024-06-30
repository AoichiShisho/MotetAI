using UnityEngine.Networking;

public static class UnityWebRequestExt {
    public static bool IsError(this UnityWebRequest request) {
        bool connectionError = request.result == UnityWebRequest.Result.ConnectionError;
        bool protocolError = request.result == UnityWebRequest.Result.ProtocolError;
        return connectionError || protocolError;
    }
}
