using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class OnlineCreateButton : MonoBehaviour
{
    public Button createButton;
    public static bool create = false;

    private void Start()
    {
        create = false;
    }

    public void ChangeScene()
    {
        create = true;
        
        SceneManager.LoadScene("Room");
    }
}
