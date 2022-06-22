using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomPlayerItem : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public Image host;

    private void Start()
    {
    }

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
    }
}
