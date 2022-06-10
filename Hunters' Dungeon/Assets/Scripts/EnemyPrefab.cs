using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyPrefab : MonoBehaviour
{
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI description;
    public Image enemyImage;
    public TextMeshProUGUI health;
    public Image bossImage;
    public Image lampImage;
    public List<Image> trophyImages;

    public GameObject dice;
    public Image diceImage;
    public TextMeshProUGUI rollText;
    public TextMeshProUGUI rollValue;

    public GameObject playerCard;

    public List<Sprite> lamps;
    public List<Sprite> trophies;
    public Sprite initialSprite;
    public Sprite resultSprite;

    private void Start()
    {
        gameObject.SetActive(false);
        playerCard.SetActive(false);
        dice.SetActive(false);
        diceImage.sprite = initialSprite;
        rollValue.text = string.Empty;
        rollText.text = "Press the roll button";
    }

    public void UpdateEnemy(Enemy enemy)
    {
        transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
        enemyName.text = enemy.name;
        description.text = enemy.description;
        health.text = enemy.health.ToString();

        if (enemy.isBoss)
            bossImage.gameObject.SetActive(true);
        else
            bossImage.gameObject.SetActive(false);

        lampImage.sprite = lamps[GetLampIndex(enemy.diceType)];
        HandleTrophyShow(enemy);
    }

    public void UpdateEnemy(Enemy enemy, PlayerOffline player)
    {
        UpdateEnemy(enemy);
        playerCard.SetActive(true);
        playerCard.GetComponentInChildren<CardPrefab>(true).UpdateCard(player);
    }

    public void UpdateDice()
    {
        if (!dice.activeInHierarchy)
            dice.SetActive(true);
    }

    int GetLampIndex(DiceType dt)
    {
        return dt switch
        {
            DiceType.GREEN => 0,
            DiceType.YELLOW => 1,
            _ => 2
        };
    }

    int GetTrophyIndex(TrophyType tt)
    {
        return tt switch
        {
            TrophyType.SLIME => 0,
            TrophyType.SKULL => 1,
            _ => 2
        };
    }

    void HandleTrophyShow(Enemy enemy)
    {
        switch(enemy.trophies.Count)
        {
            case 1:
                {
                    trophyImages[2].gameObject.SetActive(false);
                    trophyImages[1].gameObject.SetActive(false);
                    trophyImages[0].gameObject.SetActive(true);
                    trophyImages[0].sprite = trophies[GetTrophyIndex(enemy.trophies[0])];
                    break;
                }
            case 2:
                {
                    trophyImages[2].gameObject.SetActive(false);
                    trophyImages[1].gameObject.SetActive(true);
                    trophyImages[0].gameObject.SetActive(true);
                    trophyImages[0].sprite = trophies[GetTrophyIndex(enemy.trophies[0])];
                    trophyImages[1].sprite = trophies[GetTrophyIndex(enemy.trophies[1])];
                    break;
                }
            default:
                {
                    trophyImages[2].gameObject.SetActive(true);
                    trophyImages[1].gameObject.SetActive(true);
                    trophyImages[0].gameObject.SetActive(true);
                    trophyImages[0].sprite = trophies[GetTrophyIndex(enemy.trophies[0])];
                    trophyImages[1].sprite = trophies[GetTrophyIndex(enemy.trophies[1])];
                    trophyImages[2].sprite = trophies[GetTrophyIndex(enemy.trophies[2])];
                    break;
                }
        }
    }
}
