using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundMessage : MonoBehaviour
{
    public TextMeshProUGUI text;

    const string start = "Press the start button to begin";
    const string playerChoose = "Hunters Choosing Cards";
    const string revealChoice = "Hunters Reveal Cards";
    const string transformChoose = "Transform";
    const string roll = "Rolling dice";
    const string enemyAttack = "Enemy Attacks";
    const string instantEffect = "Instant Effects";
    const string playerAttack = "Hunters Attack";
    const string enemyEscapes = "Enemy escapes";
    const string enemyDies = "Enemy died";
    const string bossRemains = "Boss remains";
    const string hunterDreamPhase = "Hunter's Dream Effects";
    const string hunterDreamChoose = "Hunter's Dream, Hunter chooses a card";
    const string hunterDreamDiscardPhase = "Some hunters may have to discard a card";
    const string hunterDreamDiscard = "Hunter's Dream, Hunter discards a card";
    const string roundEnd = "End of round";
    const string revealEnemy = "Reveal new enemy";

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
            case State.HUNTER_ATTACK2:
            case State.HUNTER_ATTACK3:
            case State.HUNTER_ATTACK4:
            case State.HUNTER_ATTACK5:
                {
                    text.text = playerAttack;
                    break;
                }
            case State.ENEMY_ESCAPES:
                {
                    if (OffGameManager.enemy.isDead)
                        text.text = enemyDies;
                    else
                        text.text = enemyEscapes;
                    if (OffGameManager.enemy.isBoss)
                        text.text = bossRemains;
                    break;
                }
            case State.HUNTER_DREAM:
                {
                    text.text = hunterDreamPhase;
                    break;
                }
            case State.HUNTER_DREAM1:
            case State.HUNTER_DREAM2:
            case State.HUNTER_DREAM3:
            case State.HUNTER_DREAM4:
            case State.HUNTER_DREAM5:
                {
                    text.text = hunterDreamChoose;
                    break;
                }
            case State.HUNTER_DREAM_DISCARD:
                {
                    text.text = hunterDreamDiscardPhase;
                    break;
                }
            case State.HUNTER_DREAM_DISCARD1:
            case State.HUNTER_DREAM_DISCARD2:
            case State.HUNTER_DREAM_DISCARD3:
            case State.HUNTER_DREAM_DISCARD4:
            case State.HUNTER_DREAM_DISCARD5:
                {
                    text.text = hunterDreamDiscard;
                    break;
                }
            case State.ROUND_END:
                {
                    text.text = roundEnd;
                    break;
                }
        }
    }
}
