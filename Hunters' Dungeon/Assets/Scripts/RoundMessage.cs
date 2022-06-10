using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundMessage : MonoBehaviour
{
    public TextMeshProUGUI text;

    const string start = "Press the start button to begin.";
    const string playerChoose = "Hunters Choosing Cards.";
    const string revealChoice = "Hunters Reveal Cards.";
    const string transformChoose = "Transform";
    const string roll = "Rolling dice";
    const string enemyAttack = "Enemy Attacks";
    const string instantEffect = "Instant Effects";
    const string playerAttack = "Hunters Attack";
    const string enemyEscapes = "Enemy escapes";
    const string hunterDreamChoose = "Hunter's Dream, {0} chooses a card.";
    const string hunterDreamDiscard = "Hunter's Dream, {0} discards a card.";
    const string revealEnemy = "Reveal new enemy.";

    private void Start()
    {
    }

    private void Update()
    {
        switch(OffGameManager.state)
        {
            case State.START:
                {
                    gameObject.SetActive(true);
                    text.text = start;
                    break;
                }
            case State.REVEAL_ENEMY:
                {
                    text.text = revealEnemy;
                    break;
                }
            case State.CHOOSE_CARD1:
            case State.CHOOSE_CARD2:
            case State.CHOOSE_CARD3:
            case State.CHOOSE_CARD4:
            case State.CHOOSE_CARD5:
                {
                    text.text = playerChoose;
                    break;
                }
            case State.REVEAL_CARDS:
                {
                    text.text = revealChoice;
                    break;
                }
            case State.ROLL_DICE:
                {
                    text.text = roll;
                    break;
                }
            case State.ENEMY_ATTACK:
                {
                    text.text = enemyAttack;
                    break;
                }
            case State.HUNTER_ATTACK1:
                {
                    text.text = playerAttack;
                    break;
                }

        }
    }
}
