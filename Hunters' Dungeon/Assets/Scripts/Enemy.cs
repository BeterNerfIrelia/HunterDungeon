using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public int id;
    public string name;
    public string description;
    public string imageName;
    public int health;
    public DiceType diceType;
    public List<TrophyType> trophies;
    public Effect effect;
    public bool isBoss;
    public bool isFinalBoss;
    public bool isDead = false;
    
    
    public Modifier modifier;

    public Enemy() { }

    public Enemy(int id,string name, string description,string imageName,int health, DiceType diceType, List<TrophyType> trophies, Effect effect, bool isBoss, bool isFinalBoss)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.imageName = imageName;
        this.health = health;
        this.diceType = diceType;
        this.trophies = new List<TrophyType>(trophies);
        this.effect = new Effect(effect);
        this.isBoss = isBoss;
        this.isFinalBoss = isFinalBoss;
    }

    public Enemy(Enemy enemy)
    {
        id = enemy.id;
        name = enemy.name;
        description = enemy.description;
        imageName = enemy.imageName;
        health = enemy.health;
        diceType = enemy.diceType;
        trophies = new List<TrophyType>(enemy.trophies);
        effect = new Effect(enemy.effect);
        isBoss = enemy.isBoss;
        isFinalBoss = enemy.isFinalBoss;
    }
}
