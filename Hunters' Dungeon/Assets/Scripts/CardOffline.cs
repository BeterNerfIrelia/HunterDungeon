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

    public TransformData tData;

    int originalDamage;


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
        tData = null;

        originalDamage = this.damage;
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
        tData = null;

        originalDamage = this.damage;
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

        if(card.tData!=null)
            tData = new TransformData(card.tData);

        originalDamage = this.damage;
    }

    public void AddTransformCard(TransformData tData)
    {
        this.tData = new TransformData(tData);
    }

    public void TransformCard()
    {
        string c = name;
        name = tData.name;
        tData.name = c;

        c = description;
        description = tData.description;
        tData.description = c;

        c = imageName;
        imageName = tData.imageName;
        tData.imageName = c;

        int d = damage;
        damage = tData.damage;
        tData.damage = d;

        
        Effect e = new Effect(effect);
        
        effect = new Effect(tData.effect);

        tData.effect = new Effect(e);
        
        isTransform = !isTransform;
        tData.isTransform = !isTransform;

        bool f = isImmune;
        isImmune = tData.isImmune;
        tData.isImmune = f;
    }

    public bool IsWeapon()
    {
        return cardType switch
        {
            CardType.WEAPON_MELEE => true,
            CardType.WEAPON_RANGED => true,
            _ => false
        };
    }

    public void ResetDamage()
    {
        damage = originalDamage;
        if (id != 207)
            isImmune = false;
    }
}
