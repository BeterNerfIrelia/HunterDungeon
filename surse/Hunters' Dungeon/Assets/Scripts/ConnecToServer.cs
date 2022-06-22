using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class ConnecToServer : MonoBehaviourPunCallbacks
{
    const string playerNamePrefKey = "PlayerName";

    public TMP_InputField usernameInput;
    public TextMeshProUGUI conditions;
    public TextMeshProUGUI connectText;

    public void Awake()
    {
        conditions.gameObject.SetActive(false);
        
    }

    private void Start()
    {
        string defaultName = string.Empty;
        if(PlayerPrefs.HasKey(playerNamePrefKey))
        {
            defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            usernameInput.text = defaultName;
        }
        PhotonNetwork.NickName = defaultName;
    }

    public void OnClickConnect()
    {
        if (usernameInput.text.Length < 4)
        {
            conditions.gameObject.SetActive(true);
        }
        else
        {
            PhotonNetwork.NickName = usernameInput.text;
            connectText.text = "Conecting...";
            PlayerPrefs.SetString(playerNamePrefKey, usernameInput.text);
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene(3);
        //SceneManager.LoadScene("Room");
        //SceneManager.LoadScene("OnlineOptionsMenu");
    }

}
