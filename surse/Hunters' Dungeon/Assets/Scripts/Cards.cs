using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Cards
{
    Modifier modifier;

    List<int> startingCardsIds = new List<int>() { 101, 102, 201, 208, 401 }; //208 = blunderbass
    public List<CardOffline> cards = new List<CardOffline>();
    public List<CardOffline> deck = new List<CardOffline>();
    public List<CardOffline> topDeck = new List<CardOffline>();
    int maximumTopDeck;
    int maxAppearences = 3;

    public Dictionary<int, int> cardApps;

    public Cards() { cardApps = new Dictionary<int, int>(); }

    public List<CardOffline> GetStartingHand()
    {
        List<CardOffline> ret = new List<CardOffline>();
        CardOffline c;
        foreach (var i in startingCardsIds)
        {
            c = cards[GetCardPosById(i)];
            ret.Add(new CardOffline(c));
        }

        return ret;
    }

    public CardOffline GetFromTopDeck(int pos)
    {
        CardOffline ret = new CardOffline(topDeck[pos]);
        topDeck.RemoveAt(pos);
        return ret;
    }

    int GetCardPosById(int id)
    {
        for (int i = 0; i < cards.Count; ++i)
            if (cards[i].id == id)
                return i;
        return 0;
    }

    public void RefillTopDeck()
    {
        if (topDeck.Count == maximumTopDeck)
            return;
        for(int i=topDeck.Count;i<maximumTopDeck;++i)
        {
            topDeck.Add(new CardOffline(deck[0]));
            deck.RemoveAt(0);
        }
    }

    public void SetupTopDeck(int max)
    {
        maximumTopDeck = max;
        
        for (int i = 0; i < maximumTopDeck; ++i)
        {
            topDeck.Add(new CardOffline(deck[0]));
            deck.RemoveAt(0);
        }
    }

    public void SetupDeck()
    {
        int iterations = (cards.Count - 5) * maxAppearences;
        int index;
        List<CardOffline> available;
        CardOffline c;
        while(iterations > 0)
        {
            available = cards.FindAll(c => cardApps[c.id] < maxAppearences && !IsStartingCard(c.id));
            index = Random.Range(0, available.Count);
            cardApps[available[index].id] = cardApps[available[index].id]++;
            c = available[index];
            deck.Add(new CardOffline(available[index]));
            /*
            for(int i=0;i<deck.Count;++i)
            {
                if(deck[i].id == c.id)
                {
                    if(c.cardType == CardType.WEAPON_MELEE || c.cardType == CardType.WEAPON_RANGED)
                        deck[i].AddTransformCard(c.tData);
                    break;
                }
            }
            */
            iterations--;
        }
    }

    public bool IsStartingCard(int id)
    {
        switch(id)
        {
            case 101:
            case 102:
            case 201:
            case 401:
            case 402:
                return true;
            default:
                return false;
        }
    }

    public void ReadTransforms()
    {
        StreamReader file = new StreamReader("Assets\\Resources\\Data\\transforms.txt");
        string line;
        string[] components;

        file.ReadLine();

        line = file.ReadLine();
        TransformData transf;
        Effect effect;
        while (line != null)
        {
            components = line.Split(';');
            effect = new Effect(
                int.Parse(components[6]) > 0,
                GetEffectType(int.Parse(components[7])),
                GetCountType(int.Parse(components[8]))
                );
            /*
            transf = new CardOffline(
                int.Parse(components[0]),
                GetCardType(int.Parse(components[1])),
                components[2],
                components[3].Contains("*") ? string.Empty : components[3],
                components[4].Contains("*") ? string.Empty : components[4],
                int.Parse(components[5]),
                effect,
                int.Parse(components[9]) > 0,
                int.Parse(components[10]) > 0,
                int.Parse(components[11]) > 0
                );
            */

            transf = new TransformData(
                int.Parse(components[0]),
                components[2],
                components[3].Contains("*") ? string.Empty : components[3],
                components[4].Contains("*") ? string.Empty : components[4],
                int.Parse(components[5]),
                effect,
                int.Parse(components[10]) > 0,
                int.Parse(components[11]) > 0
                );

            for (int i = 0; i < cards.Count; ++i)
                if (cards[i].id == transf.id)
                {
                    cards[i].AddTransformCard(transf);
                    break;
                }
           
            
            line = file.ReadLine();
        }
        file.Close();

    }

    public int ReadCards()
    {
        StreamReader file = new StreamReader("Assets\\Resources\\Data\\cards.txt");
        string line;
        string[] components;

        file.ReadLine();
        bool read = false;

        line = file.ReadLine();
        CardOffline card;
        Effect effect;
        while(line != null)
        {
            read = true;
            components = line.Split(';');
            effect = new Effect(
                int.Parse(components[6]) > 0,
                GetEffectType(int.Parse(components[7])),
                GetCountType(int.Parse(components[8]))
                );
            card = new CardOffline(
                int.Parse(components[0]),
                GetCardType(int.Parse(components[1])),
                components[2],
                components[3].Contains("*") ? string.Empty : components[3],
                components[4].Contains("*") ? string.Empty : components[4],
                int.Parse(components[5]),
                effect,
                int.Parse(components[9]) > 0,
                int.Parse(components[10]) > 0,
                int.Parse(components[11]) > 0
                );
            cardApps.Add(card.id, 0);

            cards.Add(new CardOffline(card));
            line = file.ReadLine();
        }
        file.Close();

        if (!read)
            return 1;
        
        return 0;
    }

    public CardType GetCardType(int id)
    {
        return id switch
        {
            0 => CardType.OTHER,
            1 => CardType.WEAPON_MELEE,
            2 => CardType.WEAPON_RANGED,
            3 => CardType.ITEM,
            _ => CardType.OTHER,
        };
    }

    public EffectType GetEffectType(int id)
    {
        return id switch
        {
            0 => EffectType.NONE,
            1 => EffectType.REVEAL,
            2 => EffectType.PASSIVE,
            3 => EffectType.ESCAPE,
            4 => EffectType.COUNT,
            _ => EffectType.NONE,
        };
    }

    public CountType GetCountType(int id)
    {
        return id switch
        {
            0 => CountType.NONE,
            1 => CountType.MELEE,
            2 => CountType.RANGED,
            3 => CountType.POINTS,
            _ => CountType.NONE,
        };
    }
}
