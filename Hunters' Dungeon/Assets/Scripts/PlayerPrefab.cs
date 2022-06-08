using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPrefab : MonoBehaviour
{
    public TextMeshProUGUI upoints;
    public TextMeshProUGUI bpoints;
    public TextMeshProUGUI cards;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerHealth;
    public Button trophyButton;
    public TextMeshProUGUI slimeValue;
    public TextMeshProUGUI skullValue;
    public TextMeshProUGUI clawValue;

    public GameObject trophies;

    private void Start()
    {
        trophies.SetActive(false);
    }

    public void UpdatePlayer(PlayerOffline player)
    {
        upoints.text = player.unbankedPoints.ToString();
        bpoints.text = player.bankedPoints.ToString();
        cards.text = player.deck.Count.ToString();
        slimeValue.text = player.trophies[0].levels[player.trophies[0].level].ToString();
        skullValue.text = player.trophies[1].levels[player.trophies[1].level].ToString();
        clawValue.text = player.trophies[2].levels[player.trophies[2].level].ToString();

        playerHealth.text = player.health.ToString();
        playerName.text = player.name;
    }

    public void ClickOnTrophies()
    {
        if (trophies.activeInHierarchy)
            trophies.SetActive(false);
        else
            trophies.SetActive(true);
    }
}
