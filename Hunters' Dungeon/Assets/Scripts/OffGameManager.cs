using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OffGameManager : MonoBehaviour
{
    public static List<PlayerOffline> players = new List<PlayerOffline>();

    public GameObject mainPlayer;
    public GameObject playerPrefab;
    public GameObject otherPlayersEmpty;


    [HideInInspector] public List<int> playerIDs = new List<int>();
    int maxId = -1;

    private void Start()
    {
        foreach (var p in players)
        {
            Debug.LogFormat("{0} is {1}", p.name, p.bot ? "Bot" : "Player");
            playerIDs.Add(p.id);
            if (p.id > maxId)
                maxId = p.id;
        }
        for (int i = 1; i < players.Count; ++i)
            Instantiate(playerPrefab, otherPlayersEmpty.transform);
        players[0].unbankedPoints = 5;
    }

    private void Update()
    {
        mainPlayer.GetComponentInChildren<PlayerPrefab>().UpdatePlayer(players[playerIDs[0] - 1]);
        for(int i = 1;i<players.Count;++i)
        {
            otherPlayersEmpty.transform.GetChild(i-1).GetComponent<PlayerPrefab>().UpdatePlayer(players[playerIDs[i] - 1]);
        }

    }

    void ShiftPlayers()
    {
        int id = playerIDs[0];
        for (int i = 0; i < playerIDs.Count - 1; ++i)
            playerIDs[i] = playerIDs[i + 1];
        playerIDs[playerIDs.Count - 1] = id;
    }

    public void UpdateIdOrder()
    {
        if (playerIDs[0] >= maxId)
            ShiftPlayers();
        else
        {
            int id = playerIDs[0];
            playerIDs[0] = playerIDs[id];
            playerIDs[id] = id;
        }
    }

}
