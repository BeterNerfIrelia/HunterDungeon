using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class BackButton : MonoBehaviourPunCallbacks
{
    public Button backButton;

    void Start()
    {
        backButton.onClick.AddListener(HandleOnClick);
    }

    void HandleOnClick()
    {
        string scene = SceneManager.GetActiveScene().name;
        switch(scene)
        {
            case "OnlineMenu":
                {
                    SceneManager.LoadScene(1);
                    break;
                }
            case "Lobby":
                {
                    PhotonNetwork.LeaveLobby();
                    break;
                }
            case "OfflineMenu":
                {
                    SceneManager.LoadScene("MainMenu");
                    break;
                }
            case "OfflineGame":
                {
                    SceneManager.LoadScene("OfflineMenu");
                    break;
                }
        }
    }

    public override void OnLeftLobby()
    {
        SceneManager.LoadScene(2);
    }
}
