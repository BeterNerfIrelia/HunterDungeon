using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOffline
{
    public int id;
    public CardType cardType;
    public string name;
    public string description;
    public string imageName;
    public int damage;
    public Effect effect;
    public Modifier modifier;

    public bool isHunterDream;
    public bool isTransform;
    public bool isImmune;

    CardOffline transformedCard;

    public CardOffline(int id, CardType cardType, string name, string description, string imageName, int damage, Effect effect, bool isHunterDream, bool isTransform, bool isImmune)
    {
        this.id = id;
        this.cardType = cardType;
        this.name = name;
        this.description = description;
        this.imageName = imageName;
        this.damage = damage;
        this.effect = new Effect(effect);
        this.isHunterDream = isHunterDream;
        this.isTransform = isTransform;
        this.isImmune = isImmune;
    }

    public CardOffline(int id, CardType cardType, string name, string description, string imageName, int damage, Effect effect, Modifier modifier, bool isHunterDream, bool isTransform, bool isImmune)
    {
        this.id = id;
        this.cardType = cardType;
        this.name = name;
        this.description = description;
        this.imageName = imageName;
        this.damage = damage;
        this.effect = new Effect(effect);
        this.modifier = new Modifier(modifier);
        this.isHunterDream = isHunterDream;
        this.isTransform = isTransform;
        this.isImmune = isImmune;
    }

    public CardOffline(CardOffline card)
    {
        id = card.id;
        cardType = card.cardType;
        name = card.name;
        description = card.description;
        imageName = card.imageName;
        damage = card.damage;
        effect = new Effect(card.effect);
        if(card.modifier != null)
            modifier = new Modifier(card.modifier);
        isHunterDream = card.isHunterDream;
        isTransform = card.isTransform;
        isImmune = card.isImmune;
    }
}
