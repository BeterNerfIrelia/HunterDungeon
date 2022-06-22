using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnlineButton : MonoBehaviour
{
    public Button onlineButton;    

    void Start()
    {
        onlineButton.onClick.AddListener(ChangeScene);    
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(2);
    }
}
