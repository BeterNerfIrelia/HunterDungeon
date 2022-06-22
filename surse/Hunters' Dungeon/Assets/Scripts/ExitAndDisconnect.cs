using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExitAndDisconnect : MonoBehaviour
{
    public void HandleExit()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }
}
