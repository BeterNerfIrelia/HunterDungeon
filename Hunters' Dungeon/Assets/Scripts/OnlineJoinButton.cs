using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class OnlineJoinButton : MonoBehaviourPunCallbacks
{
    public static string code;
    public TMP_InputField inputField;
    public TextMeshProUGUI warning;
    public TextMeshProUGUI incorrect;

    static List<RoomInfo> rooms;

    private void Awake()
    {
        warning.gameObject.SetActive(false);
        incorrect.gameObject.SetActive(false);
    }

    private void Start()
    {
    }

    public void HandleOnClick()
    {
        if (inputField.text.Length != 6)
        {
            warning.gameObject.SetActive(true);
            return;
        }
        code = inputField.text;
        if (!CheckIfExists(code))
        {
            incorrect.gameObject.SetActive(true);
        }
        SceneManager.LoadScene("Room");
    }

    bool CheckIfExists(string name)
    {
        bool isIn = false;
        foreach(RoomInfo r in rooms)
            if(r.Name.Equals(name))
            {
                isIn = true;
                break;
            }
        return isIn;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        CacheRooms(roomList);
    }

    public static void CacheRooms(List<RoomInfo> list)
    {
        rooms.Clear();
        rooms = new List<RoomInfo>(list);
    }
}
