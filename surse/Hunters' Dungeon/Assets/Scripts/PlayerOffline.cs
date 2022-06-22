using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffline
{
    public static int maxHealth = 10;

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
    public bool hasDamaged = false;

    int transfCapacity = 2;
    public int transforms;

    public Modifier modifier;

    public List<CardOffline> deck = new List<CardOffline>(7);
    public List<CardOffline> discardedCards = new List<CardOffline>(7);
    public List<CardOffline> discardables;
    public CardOffline card;

    public List<Trophy> trophies = new List<Trophy>(3);

    public int totalPoints = -1;

    public int totalDamage = 0;
    public int sufferedDamageTotal = 0;
    public int sufferedDamageFromEnemy = 0;
    public bool doubleDamageMelee = false;
    public bool doubleDamageRanged = false;

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
            totalPoints = 0;
            totalPoints += unbankedPoints;
            totalPoints += bankedPoints;
            totalPoints += trophies[0].levels[trophies[0].level];
            totalPoints += trophies[1].levels[trophies[1].level];
            totalPoints += trophies[2].levels[trophies[2].level];

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

    public void DiscardCard(int pos)
    {
        discardedCards.Add(new CardOffline(deck[pos]));
        deck.RemoveAt(pos);
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
            discardedCards[i].ResetDamage();
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
        hasDamaged = false;
    }

    public void UpdateOrder(int max)
    {
        order--;
        order = order < 0 ? max - 1 : order;
    }

    public int LostPoints(int value)
    {
        if(value >= unbankedPoints)
        {
            unbankedPoints = 0;
            return value;
        }

        unbankedPoints -= value;
        return value;
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }

    public bool TakesDoubleDamage()
    {
        return doubleDamageMelee || doubleDamageRanged;
    }

    public bool TakeDamage(int value, bool enemy)
    {
        if (!OffGameManager.firstRound)
        {
            if (card != null && card.isImmune)
                value = 0;
            if (card != null && card.isHunterDream)
                value /= 2;
        }

        if (card != null)
        {
            if (card.id == 109)
                value--;
        }

        if (TakesDoubleDamage())
            value *= 2;

        if(value >= health)
        {
            if (OffGameManager.enemy.id == 2004)
                LostPoints(health);

            health = 0;
            unbankedPoints = 0;
            isDead = true;
            return isDead;
        }

        health -= value;
        sufferedDamageTotal += value;
        if (enemy)
            sufferedDamageFromEnemy += value;

        if (OffGameManager.enemy.id == 2004)
            LostPoints(value);

        return isDead;
    }

    public bool TakeDamage(int value, bool enemy, bool ignore)
    {
        if (!OffGameManager.firstRound)
        {
            if (!ignore)
            {
                if (card != null && card.isImmune)
                    value = 0;
                if (card != null && card.isHunterDream)
                    value /= 2;
                if (card != null)
                {
                    if (card.id == 109)
                        value--;
                }
            }
        }

        

        if (value >= health)
        {
            if (OffGameManager.enemy.id == 2004)
                LostPoints(health);

            health = 0;
            unbankedPoints = 0;
            isDead = true;
            return isDead;
        }

        health -= value;
        sufferedDamageTotal += value;
        if (enemy)
            sufferedDamageFromEnemy += value;

        if (OffGameManager.enemy.id == 2004)
            LostPoints(value);

        return isDead;
    }

    public void ChooseCard()
    {
        card = new CardOffline(deck[chosenCard]);

        deck.RemoveAt(chosenCard);
        if(card.effect.countType != CountType.NONE)
        {
            card.damage = CountCards(card.effect.countType);
        }
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

    public void ResetCounts()
    {
        foreach (CardOffline c in deck)
            if (c.effect.countType != CountType.NONE)
                c.damage = 0;
        foreach(CardOffline c in discardedCards)
            if (c.effect.countType != CountType.NONE)
                c.damage = 0;

        sufferedDamageTotal = sufferedDamageFromEnemy = 0;
        doubleDamageMelee = doubleDamageRanged = false;
        totalDamage = 0;
    }

    public int CountCards(CountType ct)
    {
        switch(ct)
        {
            case CountType.MELEE:
                return discardedCards.FindAll(c => c.cardType == CardType.WEAPON_MELEE).Count;
            case CountType.RANGED:
                return discardedCards.FindAll(c => c.cardType == CardType.WEAPON_RANGED).Count;
            case CountType.POINTS:
                return unbankedPoints;
            default:
                return 0;
        }
    }

    public void Bank()
    {
        bankedPoints += unbankedPoints;
        unbankedPoints = 0;
    }

    public void RecoverRandomCard()
    {
        if (discardedCards.Count > 0)
        {
            int v = Random.Range(0, discardedCards.Count);
            CardOffline c = new CardOffline(discardedCards[v]);
            discardedCards.RemoveAt(v);
            c.ResetDamage();
            if (c.isTransform)
                c.TransformCard();
            deck.Add(new CardOffline(c));
        }
    }

    public void GainRandomTrophy()
    {
        List<Trophy> tr = trophies.FindAll(t => t.level != t.maxLevel);
        if(tr.Count > 0)
        {
            int i = Random.Range(0, tr.Count);
            tr[i].level++;
        }
    }

    public void DiscardRandomCards(int count)
    {
        var ds = deck.FindAll(c => !c.isHunterDream);
        
        if(count >= ds.Count)
        {
            for (int i = 0; i < deck.Count;)
            {
                if (!deck[i].isHunterDream)
                {
                    discardedCards.Add(new CardOffline(deck[i]));
                    deck.RemoveAt(i);
                }
                else
                    i++;
            }
        }
        else
        {
            int i = 0;

            while (deck[i].isHunterDream)
                i++;
            discardedCards.Add(new CardOffline(deck[i]));
            deck.RemoveAt(i);

            while (deck[i].isHunterDream)
                i++;
            discardedCards.Add(new CardOffline(deck[i]));
            deck.RemoveAt(i);

        }
    }
}
