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

    public bool hasAttacked = false;

    int transfCapacity = 2;
    public int transforms;

    public Modifier modifier;

    public List<CardOffline> deck = new List<CardOffline>(7);
    public List<CardOffline> discardedCards = new List<CardOffline>(7);
    public List<CardOffline> discardables;
    public CardOffline card;

    public List<Trophy> trophies = new List<Trophy>(3);

    public int totalPoints = -1;

    public PlayerOffline() { }

    public PlayerOffline(int id, string name, bool bot)
    {
        this.id = id;
        this.name = name;
        this.bot = bot;

        modifier = new Modifier();
        transforms = transfCapacity;
        InitializeTrophies();
        InitializeInfo();
    }

    public void ResetTransformCards()
    {
        foreach (CardOffline c in deck)
            if (c.isTransform)
            {
                if (c.IsWeapon())
                {
                    c.TransformCard();
                }
            }
    }

    public int GetTotalPoints()
    {
        if(totalPoints < 0)
        {
            totalPoints += unbankedPoints;
            totalPoints += bankedPoints;
            totalPoints += trophies[0].levels[trophies[0].level];
            totalPoints += trophies[1].levels[trophies[1].level];
            totalPoints += trophies[2].levels[trophies[2].level];
        }

        return totalPoints;
    }

    public void FillDiscardables()
    {
        discardables = deck.FindAll(c => !c.isHunterDream);
    }

    public bool HasToDiscard()
    {
        return deck.Count > 7;
    }

    public void RemoveCard(int id)
    {
        for(int i=0;i<deck.Count;++i)
            if(deck[i].id == id)
            {
                deck.RemoveAt(i);
                return;
            }
    }

    public void AddCard(CardOffline c)
    {
        deck.Add(new CardOffline(c));
    }

    public void HunterDream()
    {
        bankedPoints += unbankedPoints;
        unbankedPoints = 0;
        for (int i = 0; i < discardedCards.Count; ++i)
        {
            deck.Add(new CardOffline(discardedCards[i]));
        }
        discardedCards.Clear();
        health = maxHealth;
        transforms = transfCapacity;
    }

    public void ResetClicksAndCards()
    {
        chosenCard = prevChosenCard = -1;
        hasClickedOnCard = false;
    }

    public void AddTrophy(List<TrophyType> tts)
    {
        foreach(TrophyType tt in tts)
            foreach(Trophy trophy in trophies)
                if(trophy.trophyType == tt)
                {
                    trophy.level++;
                    if (trophy.level > trophy.maxLevel)
                        trophy.level = trophy.maxLevel;
                }
    }

    public void Revive()
    {
        isDead = false;
        health = maxHealth;
        transforms = 1;
    }

    public void ResetAttacked()
    {
        hasAttacked = false;
    }

    public void UpdateOrder(int max)
    {
        order--;
        order = order < 0 ? max - 1 : order;
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

    public void HandleTransformCount()
    {
        if (card.isTransform)
            transforms--;
    }

}
