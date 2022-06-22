using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RollDiceButton : MonoBehaviour
{
    public static bool rolled = false;
    public static bool finishedRolling = false;
    static int value = 0;
    static int presses = 0;

    public Image diceImage;
    public TextMeshProUGUI rollText;
    public TextMeshProUGUI rollValue;
    public Button diceButton;

    public Sprite initialSprite;
    public Sprite rollSprite;

    static List<string> green = new List<string>() { "0", "0", "1", "+1", "+1", "2" };
    static List<string> yellow = new List<string>() { "0", "1", "1", "+1", "+2", "3" };
    static List<string> red = new List<string>() { "0", "+1", "2", "+2", "3", "4" };

    private void Start()
    {
        rollText.text = "Press the roll button.";
        diceImage.sprite = initialSprite;
    }

    private void Update()
    {
        if (presses < Dice.values.Count)
            diceButton.enabled = true;
        else if(rolled)
            diceButton.enabled = false;

        if (OffGameManager.state == State.ROLL_DICE || OffGameManager.state == State.ENEMY_ATTACK)
            diceButton.gameObject.SetActive(true);
        else
            diceButton.gameObject.SetActive(false);

    }

    public void HandleOnClick()
    {

        if(!rolled)
        {
            value = Dice.roll(OffGameManager.enemy.diceType);
            rolled = true;
            diceImage.sprite = rollSprite;
            diceImage.color = GetColourType(OffGameManager.enemy.diceType);
        }

        StartCoroutine(Rolling());
    }

    public Color32 GetColourType(DiceType dt)
    {
        return dt switch
        {
            DiceType.GREEN => new Color32(8, 150, 0, 255),
            DiceType.YELLOW => new Color32(187, 189, 41, 255),
            _ => new Color32(139, 14, 14, 255)
        };
    }

    IEnumerator Rolling()
    {
        rollText.text = "Rolling...";
        
        for(int i=0;i<20;++i)
        {
            rollValue.text = GetRandomValue(OffGameManager.enemy.diceType);
            yield return new WaitForSeconds(0.05f);
        }
        rollText.text = "You rolled";
        rollValue.text = Dice.values[presses++];

        if (Dice.values.Count == presses)
        {
            yield return new WaitForSeconds(1);
            rollText.text = "You will suffer";
            rollValue.text = Dice.rollValue.ToString();
            OffGameManager.state = State.ENEMY_ATTACK;
            finishedRolling = true;
        }
        else
        {
            rollText.text = "Roll again.";
            if (OffGameManager.enemy.id == 2006)
                OffGameManager.enemy.health++;
        }
        
    }

    public string GetRandomValue(DiceType dt)
    {
        switch(dt)
        {
            case DiceType.GREEN:
                {
                    return green[Random.Range(0,green.Count)];
                    
                }
            case DiceType.YELLOW:
                {
                    return yellow[Random.Range(0, green.Count)];
                }
            default:
                {
                    return red[Random.Range(0, green.Count)];
                }
        }
    }

    public void ResetDice()
    {
        rolled = false;
        finishedRolling = false;
        value = 0;
        presses = 0;

        diceImage.sprite = initialSprite;
        diceImage.color = new Color32(255, 255, 255, 255);
        rollText.text = "Press the roll button.";
        rollValue.text = string.Empty;
    }
}
