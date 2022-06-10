using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier
{

    Modifier self;

    public int damageTaken;
    public bool takeDoubleDamageMelee;
    public bool takeDoubleDamageRanged;
    public int additionalDamage;
    public int damageReductionMelee;
    public int damageReductionRanged;
    public int maximumDamageMelee;
    public int maximumDamageRanged;
    public int damageToEnemy;
    public int damageToOthersMelee;
    public int damageToOthersRanged;
    public int damageToOthersItems;
    public bool canIgnoreHunterDream;
    public bool kills;
    public bool isImmune;
    public CountType countType;
    public int addDamageToDice;
    public int addHealthToEnemy;
    public int healAmount;
    public bool bankPoints;
    public int bankAmount;
    public bool losePoints;
    public int losePointsAmount;
    public bool gainPoints;
    public int gainPointsAmount;
    public bool discardCards;
    public int discardCardsAmount;
    public bool recoverCard;
    public bool claimTrophy;
    public bool transformable;

    public Modifier()
    {
        damageTaken = 0;
        takeDoubleDamageMelee = false;
        takeDoubleDamageRanged = false;
        additionalDamage = 0;
        damageReductionMelee = 0;
        damageReductionRanged = 0;
        maximumDamageMelee = 100;
        maximumDamageRanged = 100;
        damageToEnemy = 0;
        damageToOthersMelee = 0;
        damageToOthersRanged = 0;
        damageToOthersItems = 0;
        canIgnoreHunterDream = false;
        kills = false;
        isImmune = false;
        countType = CountType.NONE;
        addDamageToDice = 0;
        addHealthToEnemy = 0;
        healAmount = 0;
        bankPoints = false;
        bankAmount = 0;
        losePoints = false;
        losePointsAmount = 0;
        gainPoints = false;
        gainPointsAmount = 0;
        discardCards = false;
        discardCardsAmount = 0;
        recoverCard = false;
        claimTrophy = false;
        transformable = false;
    }

    public Modifier(Modifier modifier)
    {

    }

    public void RestoreModifier()
    {

    }
}
