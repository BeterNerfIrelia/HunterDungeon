public enum State
{
    START,
    REVEAL_ENEMY,
    REVEAL_FINAL_BOSS,
    CHOOSE_CARD1,
    CHOOSE_CARD2,
    CHOOSE_CARD3,
    CHOOSE_CARD4,
    CHOOSE_CARD5,
    REVEAL_CARDS,
    TRANSFORM1,
    TRANSFORM2,
    TRANSFORM3,
    TRANSFORM4,
    TRANSFORM5,
    INSTANT_EFFECT1,
    INSTANT_EFFECT2,
    INSTANT_EFFECT3,
    INSTANT_EFFECT4,
    INSTANT_EFFECT5,
    ROLL_DICE,
    ENEMY_ATTACK,
    HUNTER_ATTACK1,
    HUNTER_ATTACK2,
    HUNTER_ATTACK3,
    HUNTER_ATTACK4,
    HUNTER_ATTACK5,
    ENEMY_ESCAPES,
    HUNTER_DREAM1,
    HUNTER_DREAM2,
    HUNTER_DREAM3,
    HUNTER_DREAM4,
    HUNTER_DREAM5,
    HUNTER_DREAM_DISCARD1,
    HUNTER_DREAM_DISCARD2,
    HUNTER_DREAM_DISCARD3,
    HUNTER_DREAM_DISCARD4,
    HUNTER_DREAM_DISCARD5,
    FINAL_SCORES,
    GAME_OVER
}

public class StateHandler
{
    public static State ChangeState(State state)
    {
        switch(state)
        {
            case State.CHOOSE_CARD1:
                return State.CHOOSE_CARD2;
            case State.CHOOSE_CARD2:
                return State.CHOOSE_CARD3;
            case State.CHOOSE_CARD3:
                {
                    if (OffGameManager.staticPlayers.Count > 3)
                        return State.CHOOSE_CARD4;
                    return State.REVEAL_CARDS;
                }
            case State.CHOOSE_CARD4:
                {
                    if (OffGameManager.staticPlayers.Count > 4)
                        return State.CHOOSE_CARD5;
                    return State.REVEAL_CARDS;
                }
            case State.CHOOSE_CARD5:
                {
                    return State.REVEAL_CARDS;
                }
            default:
                return state;
        }
        
    }
}