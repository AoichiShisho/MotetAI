using UnityEngine;
using UnityEngine.UI;

public class TitleUIController : MonoBehaviour
{
    [Header("Buttons")]
    public Button lobbyJoinButton;
    public Button lobbyCreateButton;
    public Button joinButton;
    public Button backButton;

    [Header("Parent Objects")]
    public GameObject lobbySelectParent;
    public GameObject lobbyJoinParent;

    private PhotonManager photonManager;

    void Start()
    {
        photonManager = GetComponent<PhotonManager>();

        lobbyJoinButton.onClick.AddListener(OnLobbyJoinButtonClicked);
        lobbyCreateButton.onClick.AddListener(photonManager.OnCreateLobbyButtonClicked);
        joinButton.onClick.AddListener(photonManager.OnJoinButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);

        lobbySelectParent.SetActive(true);
        lobbyJoinParent.SetActive(false);
    }

    void OnLobbyJoinButtonClicked()
    {
        lobbyJoinParent.SetActive(true);
        lobbySelectParent.SetActive(false);
    }

    void OnBackButtonClicked()
    {
        lobbyJoinParent.SetActive(false);
        lobbySelectParent.SetActive(true);
    }
}
