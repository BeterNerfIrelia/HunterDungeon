using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OfflineLobbyManager : MonoBehaviour
{
    public GameObject playerContent;
    public List<GameObject> players;
    public Button plus;
    public Button minus;
    public Button start;

    int activePlayers = 0;

    void Start()
    {
        for (int i = 3; i < 5; ++i)
            players[i].SetActive(false);
        foreach (var p in players)
        {
            if (p.activeInHierarchy)
                activePlayers++;
        }
    }

    private void Update()
    {
        if (activePlayers == 3)
            minus.gameObject.SetActive(false);
        else
            minus.gameObject.SetActive(true);

        if (activePlayers == 5)
            plus.gameObject.SetActive(false);
        else
            plus.gameObject.SetActive(true);

    }

    public void AddPlayer()
    {
        if (activePlayers == 5)
            return;
        for(int i=0;i<players.Count;++i)
            if(!players[i].activeInHierarchy)
            {
                players[i].SetActive(true);
                activePlayers++;
                return;
            }
    }

    public void RemovePlayer()
    {
        if (activePlayers == 3)
            return;
        for (int i = players.Count -1; i >= 0; --i)
            if (players[i].activeInHierarchy)
            {
                players[i].SetActive(false);
                activePlayers--;
                return;
            }
    }

    public void StartButton()
    {
        OffGameManager.staticPlayers.Clear();
        OffGameManager.staticPlayers = new List<PlayerOffline>();
        bool canStart = true;
        for(int i=0;i<activePlayers;++i)
        {
            OffPlayer p = players[i].GetComponent<OffPlayer>();
            if(p.nameInput.text.Length < 1)
            {
                canStart = false;
                break;
            }    
        }
        if (!canStart)
            return;

        for(int i=0;i<activePlayers;++i)
        {
            OffPlayer p = players[i].GetComponent<OffPlayer>();
            OffGameManager.staticPlayers.Add(new PlayerOffline(i+1, p.playerName, p.bot)); 
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }

}
