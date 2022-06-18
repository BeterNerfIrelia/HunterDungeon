using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Enemies
{
    Modifier modifier;

    public List<Enemy> enemies = new List<Enemy>();
    public List<Enemy> deck = new List<Enemy>();
    public Enemy finalBoss;

    public int enemyIndex;
    public int deckSize;
    public int normals = 0;
    public int bosses = 3;

    public Dictionary<int, bool> apps;

    List<int> testingIds = new List<int>() { 1020 };

    public Enemies() {
        apps = new Dictionary<int, bool>();
    }

    public bool EmptyDeck()
    {
        return deckSize == 0;
    }

    public void SetupTestDeck()
    {
        foreach(var i in testingIds)
            deck.Add(new Enemy(enemies.Find(e => e.id == i)));
    }

    public void AddHealth(int value)
    {
        foreach (Enemy e in deck)
            e.health += value;
        finalBoss.health += value;
    }

    public void SetupFinalBoss()
    {
        List<Enemy> fBosses = enemies.FindAll(f => f.isFinalBoss);
        int v = Random.Range(0, fBosses.Count);
        finalBoss = new Enemy(fBosses[v]);
    }

    public Enemy GetEnemy()
    {
        if (deck.Count == 0)
            return finalBoss;
        return deck[enemyIndex];
    }

    public void RemoveEnemy()
    {
        if (deck.Count > 0)
        {
            deck.RemoveAt(enemyIndex);
            deckSize = deck.Count;
            enemyIndex = GetRandomEnemy();
        }
    }

    public int GetRandomEnemy()
    {
        enemyIndex = Random.Range(0, deck.Count);
        return enemyIndex;
    }

    public void SetupDeck()
    {
        int count = 0;
        int v;
        List<Enemy> available;
        while(count < normals)
        {
            available = enemies.FindAll(e => (e.id / 1000) == 1 && !apps[e.id]);
            v = Random.Range(0, available.Count);
            deck.Add(new Enemy(available[v]));
            apps[available[v].id] = true;
            count++;
        }

        count = 0;
        while(count < bosses)
        {
            available = enemies.FindAll(e => (e.id / 1000) == 2 && !apps[e.id]);
            v = Random.Range(0, available.Count);
            deck.Add(new Enemy(available[v]));
            apps[available[v].id] = true;
            count++;
        }
        deckSize = deck.Count;
    }

    public int ReadEnemies()
    {
        StreamReader file = new StreamReader("Assets\\Resources\\Data\\enemies.txt");
        string line;
        string[] components;

        file.ReadLine();
        bool read = false;

        Enemy enemy;

        Effect effect;
        List<TrophyType> trophies = new List<TrophyType>(3);
        DiceType dt;

        line = file.ReadLine();
        while(line != null)
        {
            read = true;
            components = line.Split(';');

            dt = GetDiceType(int.Parse(components[5]));
            effect = new Effect(
                int.Parse(components[6]) > 0,
                GetEffectType(int.Parse(components[7])),
                GetCountType(int.Parse(components[8]))
                );
            trophies.Clear();
            for (int i = 0; i < int.Parse(components[11]); ++i)
                trophies.Add(GetTrophyType(int.Parse(components[12 + i])));

            enemy = new Enemy(
                int.Parse(components[0]),
                components[1],
                components[2].Contains("*") ? string.Empty : components[2],
                components[3].Contains("*") ? string.Empty : components[3],
                int.Parse(components[4]),
                dt,
                trophies,
                effect,
                int.Parse(components[9]) > 0,
                int.Parse(components[10]) > 0
                );
            enemies.Add(new Enemy(enemy));

            apps.Add(enemy.id, false);
            line = file.ReadLine();
        }
        file.Close();
        if (!read)
            return 1;
        return 0;
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

    public DiceType GetDiceType(int id)
    {
        return id switch
        {
            0 => DiceType.GREEN,
            1 => DiceType.YELLOW,
            2 => DiceType.RED,
            _ => DiceType.YELLOW,
        };
    }

    public TrophyType GetTrophyType(int id)
    {
        return id switch
        {
            0 => TrophyType.SLIME,
            1 => TrophyType.SKULL,
            2 => TrophyType.CLAW,
            _ => TrophyType.SLIME
        };
    }
}
