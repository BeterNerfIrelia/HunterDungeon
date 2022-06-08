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
