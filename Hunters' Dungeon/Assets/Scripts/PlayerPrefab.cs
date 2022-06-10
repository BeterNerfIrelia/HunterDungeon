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

    public GameObject cardSpace;
    public GameObject cardPrefab;


    List<CardOffline> playerCards = new List<CardOffline>();
    List<GameObject> objectCards = new List<GameObject>();

    private void Start()
    {
        trophies.SetActive(false);
        cardSpace = SearchExtension.Search(transform.parent.parent.parent, "PlayerCards").gameObject;
        cardSpace.SetActive(false);
        for (int i = 0; i < cardSpace.transform.childCount; ++i)
            objectCards.Add(cardSpace.transform.GetChild(i).gameObject);
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

    public void HandleShowCards(PlayerOffline player)
    {
        if (playerCards.Count != player.deck.Count)
            GetPlayerCards(player);
        if (cardSpace.activeInHierarchy)
        {
            cardSpace.SetActive(false);
        }
        else
        {
            cardSpace.SetActive(true);
            HandleObjectCards(player);
        }
    }

    public void ClickOnTrophies()
    {
        if (trophies.activeInHierarchy)
            trophies.SetActive(false);
        else
            trophies.SetActive(true);
    }

    void GetPlayerCards(PlayerOffline player)
    {
        playerCards.Clear();
        playerCards = new List<CardOffline>(player.deck);
    }

    void HandleObjectCards(PlayerOffline player)
    {
        for (int i = 0; i < objectCards.Count; ++i)
            objectCards[i].SetActive(false);
        for (int i = 0; i < player.deck.Count; ++i)
        {
            objectCards[i].SetActive(true);
            objectCards[i].GetComponentInChildren<CardPrefab>().UpdateCard(player.deck[i], player.name);
        }
    }
}
