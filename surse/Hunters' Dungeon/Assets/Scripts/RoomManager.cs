using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static string code;
    public TextMeshProUGUI codeText;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
        if (OnlineCreateButton.create)
        {
            code = GenerateCode.Generate();
        }
        else
        {
            code = OnlineJoinButton.code;
        }
        codeText.text = code;
    }

    void SetRoomName()
    {
        codeText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnJoinedLobby()
    {
        if (OnlineCreateButton.create)
            PhotonNetwork.CreateRoom(code, new RoomOptions() { MaxPlayers = 5 });
        else
            PhotonNetwork.JoinRoom(code);
    }

}
