using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleUIController : MonoBehaviour
{
    [Header("Buttons")]
    public Button lobbyJoinButton;
    public Button lobbyCreateButton;
    public Button joinButton;
    public Button joinBackButton;
    public Button createBackButton;
    public Button confirmNameButton;

    [Header("Parent Objects")]
    public GameObject lobbySelectParent;
    public GameObject lobbyJoinParent;
    public GameObject lobbyCreateParent;

    [Header("Input Fields")]
    public TMP_InputField inputField;

    [Header("Text Fields")]
    public TMP_Text textAmount;

    private PhotonManager photonManager;
    private bool isCreatingLobby;

    void Start()
    {
        photonManager = GetComponent<PhotonManager>();

        inputField.onValueChanged.AddListener(UpdateTextAmount);

        lobbyJoinButton.onClick.AddListener(OnLobbyJoinButtonClicked);
        lobbyCreateButton.onClick.AddListener(OnLobbyCreateButtonClicked);
        joinButton.onClick.AddListener(OnJoinButtonClicked);
        joinBackButton.onClick.AddListener(OnBackButtonClicked);
        createBackButton.onClick.AddListener(OnBackButtonClicked);
        confirmNameButton.onClick.AddListener(OnConfirmNameButtonClicked);

        lobbySelectParent.SetActive(true);
        lobbyJoinParent.SetActive(false);
        lobbyCreateParent.SetActive(false);

        UpdateTextAmount(inputField.text);
    }

    void OnLobbyJoinButtonClicked()
    {
        isCreatingLobby = false;
        lobbyJoinParent.SetActive(true);
        lobbySelectParent.SetActive(false);
    }

    void OnLobbyCreateButtonClicked()
    {
        isCreatingLobby = true;
        lobbyCreateParent.SetActive(true);
        lobbySelectParent.SetActive(false);
    }

    void OnBackButtonClicked()
    {
        lobbyJoinParent.SetActive(false);
        lobbyCreateParent.SetActive(false);
        lobbySelectParent.SetActive(true);
    }

    void OnJoinButtonClicked()
    {
        lobbyCreateParent.SetActive(true);
        lobbyJoinParent.SetActive(false);
    }

    void OnConfirmNameButtonClicked()
    {
        string playerName = inputField.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            photonManager.SetPlayerName(playerName);

            if (isCreatingLobby)
            {
                photonManager.OnCreateLobbyButtonClicked();
            }
            else
            {
                photonManager.OnJoinButtonClicked();
            }

            lobbyCreateParent.SetActive(false);
        }
        else
        {
            Debug.LogError("Player name is empty!");
            // ここにUIを表示する処理を追加
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
