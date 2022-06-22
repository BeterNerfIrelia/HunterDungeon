using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ExitButton : MonoBehaviour
{
    // Start is called before the first frame update

    public Button exitButton;

    void Start()
    {
        exitButton.onClick.AddListener(HandleExit);
    }

    public void HandleExit()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        Application.Quit();
    }
}
