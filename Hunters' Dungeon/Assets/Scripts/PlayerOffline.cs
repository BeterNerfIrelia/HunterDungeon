using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffline
{
    public static int maxHealth = 8;

    public int id;
    public string name;
    public bool bot;

    public int order;
    public int health;
    public int unbankedPoints;
    public int bankedPoints;

    public int chosenCard = -1;
    public int prevChosenCard = -1;
    public bool hasClickedOnCard = false;
    public bool isDead = false;

    public Modifier modifier;

    public List<CardOffline> deck = new List<CardOffline>(7);
    public List<CardOffline> discardedCards = new List<CardOffline>(7);
    public CardOffline card;

    public List<Trophy> trophies = new List<Trophy>(3);

    public PlayerOffline() { }

    public PlayerOffline(int id, string name, bool bot)
    {
        this.id = id;
        this.name = name;
        this.bot = bot;

        modifier = new Modifier();

        InitializeTrophies();
        InitializeInfo();
    }

    public bool TakeDamage(int value)
    {
        if (!OffGameManager.firstRound)
        {
            if (card.isImmune)
                value = 0;
            if (card.isHunterDream)
                value /= 2;
        }

        if(value >= health)
        {
            health = 0;
            unbankedPoints = 0;
            isDead = true;
            return isDead;
        }

        health -= value;

        return isDead;
    }

    public void ChooseCard()
    {
        card = new CardOffline(deck[chosenCard]);
        deck.RemoveAt(chosenCard);
        discardedCards.Add(new CardOffline(card));
    }

    void InitializeTrophies()
    {
        trophies.Add(new Trophy(TrophyType.SLIME));
        trophies.Add(new Trophy(TrophyType.SKULL));
        trophies.Add(new Trophy(TrophyType.CLAW));
    }

    void InitializeInfo()
    {
        unbankedPoints = bankedPoints = 0;
        health = maxHealth;
        order = id;
    }


}
