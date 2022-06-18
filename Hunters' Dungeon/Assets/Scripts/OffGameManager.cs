using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class OffGameManager : MonoBehaviour
{
    public static List<PlayerOffline> staticPlayers = new List<PlayerOffline>();
    public List<PlayerOffline> players;
    public List<PlayerOffline> order = new List<PlayerOffline>();

    public EventSystem eventSystem;

    public GameObject mainPlayer;
    public GameObject playerPrefab;
    public GameObject otherPlayersEmpty;

    public GameObject playerCardsEmpty;

    public GameObject currentEnemyEmpty;
    public GameObject finalBossEmpty;
    public GameObject revealCardsEmpty;
    public GameObject armouryEmpty;

    public static Cards cards;
    public static Enemies enemies;
    public static State state;
    public static Enemy enemy;

    [HideInInspector] public List<int> playerIDs;
    public List<int> pidsBefore;
    int maxId = -1;
    int additionalHealth;

    public Button startButton;
    public Button continueButton;
    public Button revealCardsButton;
    public Button revealArmouryButton;
    public Button revealFinalBoss;
    public Button rollDiceButton;
    public bool enemyChosen;
    public bool once;

    PointerEventData ped;
    public List<RaycastResult> rayResults;
    GraphicRaycaster raycaster;
    public Canvas canvas;
    public static bool firstRound;

    public static List<PlayerOffline> dreams = new List<PlayerOffline>();

    public GameObject board;

    public GameObject winnerEmpty;
    public GameObject winnerPrefab;

    private void Start()
    {
        firstRound = true;
        once = false;
        enemyChosen = false;
        players = new List<PlayerOffline>(staticPlayers);
        
        cards = new Cards();
        enemies = new Enemies();
        maxId = -1;
        playerIDs = new List<int>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();


        state = State.START;
        winnerEmpty.SetActive(false);
        currentEnemyEmpty.SetActive(false);
        finalBossEmpty.SetActive(false);
        playerCardsEmpty.SetActive(false);
        revealCardsEmpty.SetActive(false);
        armouryEmpty.SetActive(false);

        continueButton.gameObject.SetActive(false);
        revealCardsButton.gameObject.SetActive(false);
        revealArmouryButton.gameObject.SetActive(false);
        rollDiceButton.gameObject.SetActive(false);

        playerIDs.Clear();
        playerIDs = new List<int>();
        int counter = 0;
        foreach (var p in players)
        {
            playerIDs.Add(p.id);
            p.order = counter++;
            if (p.id > maxId)
                maxId = p.id;
            p.GetTotalPoints();
        }

        pidsBefore = new List<int>(playerIDs);

        additionalHealth = (players.Count - 3) * 2;
        for (int i = 1; i < players.Count; ++i)
            Instantiate(playerPrefab, otherPlayersEmpty.transform);
        SetupCards();
        SetupPlayerDecks();
        SetupEnemies();
        enemies.finalBoss = new Enemy(enemies.enemies.Find(e => e.id == 3003));
        if (enemies.finalBoss.effect.hasEffect)
            if (enemies.finalBoss.effect.effectType == EffectType.REVEAL)
                EffectHandler(enemies.finalBoss.id);

        finalBossEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemies.finalBoss);

        foreach (var p in players)
        {
            p.FillDiscardables();
            p.GetTotalPoints();
        }
        
    }

    private void Update()
    {
        mainPlayer.GetComponentInChildren<PlayerPrefab>().UpdatePlayer(players[playerIDs[0] - 1]);
        for(int i = 1;i<players.Count;++i)
        {
            otherPlayersEmpty.transform.GetChild(i-1).GetComponent<PlayerPrefab>().UpdatePlayer(players[playerIDs[i] - 1]);
        }

        HandleState();
    }

    public void HandleState()
    {
        switch(state)
        {
            case State.START:
                {
                    revealFinalBoss.gameObject.SetActive(false);
                    revealArmouryButton.gameObject.SetActive(false);
                    break;
                }
            case State.REVEAL_ENEMY:
                {
                    if(!enemyChosen)
                    {
                        enemyChosen = true;
                        //enemy = enemies.deck[enemies.GetRandomEnemy()];
                        enemy = enemies.deck[0];
                    }
                    if(!once)
                    {
                        once = true;
                        finalBossEmpty.SetActive(false);
                    }

                    if (enemy.isFinalBoss)
                        revealFinalBoss.gameObject.SetActive(false);

                    currentEnemyEmpty.SetActive(true);
                    currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                    break;
                }
            case State.CHOOSE_CARD1:
            case State.CHOOSE_CARD2:
            case State.CHOOSE_CARD3:
            case State.CHOOSE_CARD4:
            case State.CHOOSE_CARD5:
                {
                    HandleChooseCard();
                    break;
                }
            case State.ROLL_DICE:
                {
                    playerCardsEmpty.SetActive(false);
                    revealCardsEmpty.SetActive(false);
                    rollDiceButton.gameObject.SetActive(true);
                    continueButton.interactable = false;
                    break;
                }
            case State.ENEMY_ATTACK:
                {
                    continueButton.interactable = true;
                    break;
                }
            case State.ENEMY_ESCAPES:
                {
                    currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).DisableCard();
                    break;
                }
            case State.HUNTER_DREAM1:
            case State.HUNTER_DREAM2:
            case State.HUNTER_DREAM3:
            case State.HUNTER_DREAM4:
            case State.HUNTER_DREAM5:
                {
                    HandleChooseArmoury();
                    break;
                }
            case State.HUNTER_DREAM_DISCARD1:
            case State.HUNTER_DREAM_DISCARD2:
            case State.HUNTER_DREAM_DISCARD3:
            case State.HUNTER_DREAM_DISCARD4:
            case State.HUNTER_DREAM_DISCARD5:
                {
                    
                    HandleChooseDiscardCard();
                    break;
                }
            case State.ROUND_END:
                {
                    revealArmouryButton.interactable = false;
                    break;
                }
            case State.FINAL_SCORES:
                {
                    if (!once)
                    {
                        once = true;
                        HandleWinners();
                    }
                    
                    break;
                }
            default: 
                break;
        }
    }

    public void HandleContinueButton()
    {
        switch(state)
        {
            case State.REVEAL_ENEMY:
                {
                    foreach (var p in players)
                    {
                        p.ResetClicksAndCards();
                        p.ResetAttacked();
                    }

                    if (enemy.effect.hasEffect)
                        if (enemy.effect.effectType == EffectType.REVEAL)
                            EffectHandler(enemy.id);

                    state = State.CHOOSE_CARD1;
                    revealFinalBoss.interactable = false;
                    revealArmouryButton.interactable = false;

                    mainPlayer.GetComponentInChildren<PlayerPrefab>(true).HandleShowCards(players[playerIDs[0] - 1]);
                    revealCardsButton.gameObject.SetActive(true);
                    once = false;
                    armouryEmpty.SetActive(false);
                    finalBossEmpty.SetActive(false);
                    firstRound = false;



                    break;
                }
            case State.CHOOSE_CARD1:
            case State.CHOOSE_CARD2:
                {
                    HandleChooseCommon();
                    break;
                }
            case State.CHOOSE_CARD3:
                {
                    HandleChooseCommon();
                    if (players.Count == 3)
                        HandleLastPlayerChoose();
                    break;
                }
            case State.CHOOSE_CARD4:
                {
                    HandleChooseCommon();
                    if (players.Count == 4)
                        HandleLastPlayerChoose();
                    break;
                }
            case State.CHOOSE_CARD5:
                {
                    HandleChooseCommon();
                    HandleLastPlayerChoose();
                    break;
                }
            case State.REVEAL_CARDS:
                {
                    currentEnemyEmpty.SetActive(true);
                    revealCardsEmpty.SetActive(false);
                    state = State.INSTANT_EFFECT;
                    break;
                }
            case State.INSTANT_EFFECT:
                {
                    
                    dreams.Clear();
                    for(int i=0;i<players.Count;++i)
                    {
                        if (players[playerIDs[i] - 1].card.effect.effectType == EffectType.REVEAL)
                            dreams.Add(players[playerIDs[i] - 1]);
                        
                    }

                    var exclude = dreams.FindAll(p => p.card.id == 201).Count;
                    if (exclude > 1)
                    {
                        for (int i = 0; i < dreams.Count;)
                        {
                            if (dreams[i].card.id == 201)
                                dreams.RemoveAt(i);
                            else
                                i++;
                        }
                    }

                    if (dreams.Count > 0)
                    {
                        state = State.INSTANT_EFFECT1;
                        currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy, dreams[0]);

                        if (dreams[0].id != playerIDs[0])
                        {
                            bool condition = false;
                            do
                            {
                                UpdateIdOrder();
                                if (dreams[0].id == playerIDs[0] || dreams[0].id == pidsBefore[0])
                                    condition = true;
                            } while (!condition);
                        }
                    }
                    else
                        state = State.ROLL_DICE;
                    break;
                }
            case State.INSTANT_EFFECT1:
            case State.INSTANT_EFFECT2:
            case State.INSTANT_EFFECT3:
            case State.INSTANT_EFFECT4:
            case State.INSTANT_EFFECT5:
                {
                    HandleInstantEffectPlayer();
                    break;
                }
            case State.ROLL_DICE:
                {
                    continueButton.interactable = false;
                    armouryEmpty.SetActive(false);
                    finalBossEmpty.SetActive(false);
                    break;
                }
            case State.ENEMY_ATTACK:
                {
                    foreach (PlayerOffline p in players)
                    {
                        p.TakeDamage(Dice.rollValue, true);
                        if (p.card.id == 110 && !p.card.isTransform)
                        {
                            p.card.damage += p.sufferedDamageTotal;
                        }
                    }

                    
                    order = (players.FindAll(p => !p.isDead && !p.hasAttacked));
                    if (order.Count > 0)
                    {
                        Sort(order);
                        currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy, order[0]);
                        state = State.HUNTER_ATTACK1;
                    }
                    else
                    {
                        state = State.ENEMY_ESCAPES;
                    }
                    break;
                }
            case State.HUNTER_ATTACK1:
            case State.HUNTER_ATTACK2:
            case State.HUNTER_ATTACK3:
            case State.HUNTER_ATTACK4:
            case State.HUNTER_ATTACK5:
                {
                    HandleAttackPlayer();
                    break;
                }
            case State.ENEMY_ESCAPES:
                {
                    HandlePistolHeal();
                    if(enemy.isDead)
                    {
                        foreach (PlayerOffline p in players)
                            if (p.hasAttacked || p.hasDamaged)
                                p.AddTrophy(enemy.trophies);

                        if (enemy.isFinalBoss)
                        {
                            state = State.FINAL_SCORES;
                            once = false;
                            winnerEmpty.SetActive(true);
                            board.SetActive(false);
                            break;
                        }
                        //currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                        currentEnemyEmpty.SetActive(false);
                        enemy = enemies.GetEnemy();
                        state = State.HUNTER_DREAM;
                        return;
                    }

                    if(enemy.isBoss)
                    {
                        state = State.HUNTER_DREAM;
                        return;
                    }

                    // enemy escapes because it's neither a boss nor dead

                    currentEnemyEmpty.SetActive(false);
                    state = State.HUNTER_DREAM;
                    once = false;
                    break;
                }
            case State.HUNTER_DREAM:
                {
                    dreams.Clear();
                    for (int i = 0; i < players.Count; ++i)
                    {
                        players[playerIDs[i] - 1].ResetClicksAndCards();
                        if (players[playerIDs[i] - 1].card.isHunterDream)
                        {
                            players[playerIDs[i] - 1].HunterDream();
                            dreams.Add(players[playerIDs[i] - 1]);
                        }
                        if (players[playerIDs[i] - 1].isDead)
                            players[playerIDs[i] - 1].Revive();
                    }

                    if (dreams.Count > 0)
                    {
                        state = State.HUNTER_DREAM1;
                        OnClickArmoury();
                        if (dreams[0].id != playerIDs[0])
                        {
                            bool condition = false;
                            do
                            {
                                UpdateIdOrder();
                                if (dreams[0].id == playerIDs[0] || dreams[0].id == pidsBefore[0])
                                    condition = true;
                            } while (!condition);
                        }

                    }
                    else
                        state = State.ROUND_END;
                    break;
                }
            case State.HUNTER_DREAM1:
            case State.HUNTER_DREAM2:
            case State.HUNTER_DREAM3:
            case State.HUNTER_DREAM4:
            case State.HUNTER_DREAM5:
                {
                    HandleChooseDream();
                    break;
                }
            case State.HUNTER_DREAM_DISCARD:
                {
                    dreams.Clear();
                    for(int i=0;i<players.Count;++i)
                    {
                        players[playerIDs[i] - 1].ResetClicksAndCards();
                        players[playerIDs[i] - 1].FillDiscardables();
                        players[playerIDs[i] - 1].ResetTransformCards();
                        if (players[playerIDs[i] - 1].HasToDiscard())
                        {
                            dreams.Add(players[playerIDs[i] - 1]);
                        }
                    }

                    if (dreams.Count > 0)
                    {
                        state = State.HUNTER_DREAM_DISCARD1;
                        if (dreams[0].id != playerIDs[0])
                        {
                            bool condition = false;
                            do
                            {
                                UpdateIdOrder();
                                if (dreams[0].id == playerIDs[0] || dreams[0].id == pidsBefore[0])
                                    condition = true;
                            } while (!condition);
                        }
                        mainPlayer.GetComponentInChildren<PlayerPrefab>(true).HandleDiscardCards(dreams[0]);
                    }
                    else
                        state = State.ROUND_END;
                    break;
                }
            case State.HUNTER_DREAM_DISCARD1:
            case State.HUNTER_DREAM_DISCARD2:
            case State.HUNTER_DREAM_DISCARD3:
            case State.HUNTER_DREAM_DISCARD4:
            case State.HUNTER_DREAM_DISCARD5:
                {
                    HandleChooseDiscard();
                    break;
                }
            case State.ROUND_END:
                {
                    cards.RefillTopDeck();
                    revealArmouryButton.interactable = true;
                    UpdateIdOrder();
                    UpdatePlayerOrder();
                    state = State.REVEAL_ENEMY;
                    pidsBefore = new List<int>(playerIDs);
                    currentEnemyEmpty.GetComponentInChildren<RollDiceButton>(true).ResetDice();

                    foreach (PlayerOffline p in players)
                    {
                        p.ResetTransformCards();
                        p.ResetCounts();
                        p.ResetClicksAndCards();
                        p.ResetAttacked();
                    }

                    break;
                }
        }
    }

    public void HandleAttackPlayer()
    {
        EnemyPrefab ep = currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true);
        if (enemy.TakeDamage(order[0], currentEnemyEmpty))
        {
            state = State.ENEMY_ESCAPES;
            ep.UpdateEnemy(enemy, order[0]);
            ep.DisableCard();
            return;
        }

        order.RemoveAt(0);
        if (order.Count > 0)
        {
            state = StateHandler.ChangeState(state);
            ep.UpdateEnemy(enemy, order[0]);

        }
        else
            state = State.ENEMY_ESCAPES;
        
    }

    public void HandleInstantEffectPlayer()
    {
        EffectHandler(dreams[0]);

        dreams.RemoveAt(0);
        if (dreams.Count > 0)
        {
            bool condition = false;
            do
            {
                UpdateIdOrder();
                if (dreams[0].id == playerIDs[0] || dreams[0].id == pidsBefore[0])
                    condition = true;
            } while (!condition);
            currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy, dreams[0]);
        }
        else
            HandleLastInstantEffectPlayer();

        state = StateHandler.ChangeState(state);
        if (enemy.isDead)
            state = State.ENEMY_ESCAPES;
    }

    public void HandleLastInstantEffectPlayer()
    {
        currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).DisableCard();
        bool condition = false;
        do
        {
            UpdateIdOrder();
            if (playerIDs[0] == pidsBefore[0])
                condition = true;
        } while (!condition);
    }

    public void UpdatePlayerOrder()
    {
        foreach (var p in players)
            p.UpdateOrder(players.Count);
    }

    public void HandleShowCardsButton()
    {
        switch (state)
        {
            case State.CHOOSE_CARD1:
            case State.CHOOSE_CARD2:
            case State.CHOOSE_CARD3:
            case State.CHOOSE_CARD4:
            case State.CHOOSE_CARD5:
                {
                    if (playerCardsEmpty.activeInHierarchy)
                    {
                        playerCardsEmpty.SetActive(false);
                    }
                    else
                    {
                        mainPlayer.GetComponentInChildren<PlayerPrefab>(true).HandleShowCards(players[playerIDs[0] - 1]);
                    }
                    break;
                }
        }
    }

    public void HandleChooseDiscard()
    {
        if (!dreams[0].hasClickedOnCard)
            return;

        // dreams[0].AddCard(cards.GetFromTopDeck(dreams[0].chosenCard));
        dreams[0].RemoveCard(dreams[0].discardables[dreams[0].chosenCard].id);
        RescaleCards();
        
        if(dreams.Count >= 2)
            mainPlayer.GetComponentInChildren<PlayerPrefab>(true).HandleDiscardCards(dreams[1]);

        dreams.RemoveAt(0);
        if (dreams.Count > 0)
        {
            bool condition = false;
            do
            {
                UpdateIdOrder();
                if (dreams[0].id == playerIDs[0] || dreams[0].id == pidsBefore[0])
                    condition = true;
            } while (!condition);
        }
        else
            HandleLastPlayerDiscard();

        state = StateHandler.ChangeState(state);
    }

    public void HandleLastPlayerDiscard()
    {
        playerCardsEmpty.SetActive(false);
        bool condition = false;
        do
        {
            UpdateIdOrder();
            if (playerIDs[0] == pidsBefore[0])
                condition = true;
        } while (!condition);
    }

    public void HandleChooseDream()
    {
        if (!dreams[0].hasClickedOnCard)
            return;

        dreams[0].AddCard(cards.GetFromTopDeck(dreams[0].chosenCard));
        RescaleArmoury();

        dreams.RemoveAt(0);
        if (dreams.Count > 0)
        {
            bool condition = false;
            do
            {
                UpdateIdOrder();
                if (dreams[0].id == playerIDs[0] || dreams[0].id == pidsBefore[0])
                    condition = true;
            } while (!condition);
        }
        else
            HandleLastPlayerArmoury();

        UpdateArmouryCards();
        state = StateHandler.ChangeState(state);
    }

    public void HandleChooseCommon()
    {
        if (!players[playerIDs[0] - 1].hasClickedOnCard)
            return;

        players[playerIDs[0] - 1].ChooseCard();
        RescaleCards();
        UpdateIdOrder();
        mainPlayer.GetComponentInChildren<PlayerPrefab>(true).HandleShowCards(players[playerIDs[0] - 1]);
        state = StateHandler.ChangeState(state);
        once = false;
    }

    public void UpdateArmouryCards()
    {
        for (int i = 0; i < armouryEmpty.transform.childCount; ++i)
            armouryEmpty.transform.GetChild(i).gameObject.SetActive(false);
        GameObject tmp;
        for(int i=0;i<cards.topDeck.Count;++i)
        {
            tmp = armouryEmpty.transform.GetChild(i).gameObject;
            tmp.SetActive(true);
            tmp.GetComponentInChildren<CardPrefab>(true).UpdateCard(cards.topDeck[i]);
        }
    }

    public static void Sort(List<PlayerOffline> ps)
    {
        for(int i=0;i<ps.Count -1;++i)
            for(int j=i+1;j<ps.Count;++j)
                if(ps[i].order > ps[j].order)
                {
                    var p = ps[i];
                    ps[i] = ps[j];
                    ps[j] = p;
                }
    }

    public void HandlePlayerOrder()
    {
        foreach(PlayerOffline p in players)
        {
            p.UpdateOrder(players.Count);
        }
    }

    public void HandleLastPlayerArmoury()
    {
        armouryEmpty.SetActive(false);
        once = false;
        bool condition = false;
        do
        {
            UpdateIdOrder();
            if (playerIDs[0] == pidsBefore[0])
                condition = true;
        } while (!condition);
    }

    public void HandleLastPlayerChoose()
    {
        playerCardsEmpty.SetActive(false);
        revealCardsEmpty.SetActive(true);
        HandleRevealCards();

        revealFinalBoss.interactable = true;
        revealArmouryButton.interactable = true;
        revealCardsButton.gameObject.SetActive(false);
        once = false;
        currentEnemyEmpty.SetActive(false);

        armouryEmpty.SetActive(false);
        finalBossEmpty.SetActive(false);

        foreach (PlayerOffline p in players)
            p.HandleTransformCount();

    }

    public void HandleRevealCards()
    {
        for (int i = 0; i < revealCardsEmpty.transform.childCount; ++i)
            revealCardsEmpty.transform.GetChild(i).gameObject.SetActive(false);
        for(int i=0;i<players.Count;++i)
        {
            revealCardsEmpty.transform.GetChild(i).gameObject.SetActive(true);
            revealCardsEmpty.transform.GetChild(i).gameObject.GetComponentInChildren<CardPrefab>().UpdateCard(players[playerIDs[i] - 1]);
        }
    }

    public void HandleChooseDiscardCard()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ped = new PointerEventData(eventSystem) { position = Input.mousePosition };

            rayResults = new List<RaycastResult>();
            raycaster.Raycast(ped, rayResults);

            foreach (RaycastResult res in rayResults)
            {
                if (res.gameObject.transform.parent != null)
                {
                    if (res.gameObject.name.Contains("Transform"))
                    {
                        /*
                        var cardName = res.gameObject.transform.parent.GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text;
                        Debug.Log("Cartea mea este " + cardName);
                        foreach (var c in dreams[0].discardables)
                            if (c.name.Equals(cardName))
                            {
                                c.TransformCard();
                                mainPlayer.GetComponentInChildren<PlayerPrefab>(true).HandleDiscardCards(dreams[0]);
                                break;
                            }
                        */
                        break;
                    }
                    if (res.gameObject.name.Contains("CardBackground"))
                    {

                        string objName;
                        CardPrefab scaleComparer = res.gameObject.transform.parent.gameObject.GetComponentInChildren<CardPrefab>(true);
                        if (!dreams[0].hasClickedOnCard)
                        {
                            dreams[0].hasClickedOnCard = true;
                            scaleComparer.CardSelectionAnimation();
                            objName = res.gameObject.transform.parent.name;
                            dreams[0].chosenCard = objName[objName.Length - 1] - '1';
                            break;
                        }

                        dreams[0].prevChosenCard = dreams[0].chosenCard;
                        objName = res.gameObject.transform.parent.name;
                        dreams[0].chosenCard = objName[objName.Length - 1] - '1';

                        Vector3 cardScale = res.gameObject.transform.localScale;
                        if (dreams[0].prevChosenCard == dreams[0].chosenCard)
                            break;
                        if (cardScale == scaleComparer.originalScale)
                        {
                            scaleComparer.CardSelectionAnimation();
                            scaleComparer = playerCardsEmpty
                                .transform
                                .GetChild(dreams[0].prevChosenCard)
                                .gameObject
                                .GetComponentInChildren<CardPrefab>(true);
                            scaleComparer.CardDeselectionAnimation();
                        }

                    }
                }

            }
        }
    }


    public void HandleChooseArmoury()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ped = new PointerEventData(eventSystem) { position = Input.mousePosition };

            rayResults = new List<RaycastResult>();
            raycaster.Raycast(ped, rayResults);

            foreach (RaycastResult res in rayResults)
            {
                if (res.gameObject.transform.parent != null)
                {
                    if (res.gameObject.name.Contains("Transform"))
                    {
                        var cardName = res.gameObject.transform.parent.GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text;
                        foreach (var c in cards.topDeck)
                            if (c.name.Equals(cardName))
                            {
                                if (c.IsWeapon())
                                {
                                    c.TransformCard();
                                }
                                OnClickArmoury();
                                break;
                            }
                        break;
                    }
                    if (res.gameObject.name.Contains("CardBackground"))
                    {
                        
                        string objName;
                        CardPrefab scaleComparer = res.gameObject.transform.parent.gameObject.GetComponentInChildren<CardPrefab>(true);
                        if (!dreams[0].hasClickedOnCard)
                        {
                            dreams[0].hasClickedOnCard = true;
                            scaleComparer.CardSelectionAnimation();
                            objName = res.gameObject.transform.parent.name;
                            dreams[0].chosenCard = objName[objName.Length - 1] - '1';
                            break;
                        }

                        dreams[0].prevChosenCard = dreams[0].chosenCard;
                        objName = res.gameObject.transform.parent.name;
                        dreams[0].chosenCard = objName[objName.Length - 1] - '1';

                        Vector3 cardScale = res.gameObject.transform.localScale;
                        if (dreams[0].prevChosenCard == dreams[0].chosenCard)
                            break;
                        if (cardScale == scaleComparer.originalScale)
                        {
                            scaleComparer.CardSelectionAnimation();
                            scaleComparer = armouryEmpty
                                .transform
                                .GetChild(dreams[0].prevChosenCard)
                                .gameObject
                                .GetComponentInChildren<CardPrefab>(true);
                            scaleComparer.CardDeselectionAnimation();
                        }

                    }
                }

            }
        }
    }

    public void HandleChooseCard()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ped = new PointerEventData(eventSystem) { position = Input.mousePosition };

            rayResults = new List<RaycastResult>();
            raycaster.Raycast(ped, rayResults);

            foreach(RaycastResult res in rayResults)
            {
                if(res.gameObject.transform.parent != null)
                {
                    if (res.gameObject.name.Contains("Transform"))
                    {
                        var cardName = res.gameObject.transform.parent.GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>(true).text;
                        foreach (var c in players[playerIDs[0] - 1].deck)
                            if (c.name.Equals(cardName))
                            {
                                if(c.IsWeapon())
                                    c.TransformCard();
                                mainPlayer.GetComponentInChildren<PlayerPrefab>(true).HandleShowCards(players[playerIDs[0]-1]);
                                break;
                            }
                        break;
                    }
                    if(res.gameObject.name.Contains("CardBackground"))
                    {
                        string objName;
                        CardPrefab scaleComparer = res.gameObject.transform.parent.gameObject.GetComponentInChildren<CardPrefab>(true);
                        if (!players[playerIDs[0] - 1].hasClickedOnCard)
                        {
                            players[playerIDs[0] - 1].hasClickedOnCard = true;
                            scaleComparer.CardSelectionAnimation();
                            objName  = res.gameObject.transform.parent.name;
                            players[playerIDs[0] - 1].chosenCard = objName[objName.Length - 1] - '1';

                            break;
                        }

                        players[playerIDs[0] - 1].prevChosenCard = players[playerIDs[0] - 1].chosenCard;
                        objName = res.gameObject.transform.parent.name;
                        players[playerIDs[0] - 1].chosenCard = objName[objName.Length - 1] - '1';

                        Vector3 cardScale = res.gameObject.transform.localScale;
                        if (players[playerIDs[0] - 1].prevChosenCard == players[playerIDs[0] - 1].chosenCard)
                            break;
                        if(cardScale == scaleComparer.originalScale)
                        {
                            scaleComparer.CardSelectionAnimation();
                            scaleComparer = playerCardsEmpty
                                .transform
                                .GetChild(players[playerIDs[0] - 1].prevChosenCard)
                                .gameObject
                                .GetComponentInChildren<CardPrefab>(true);
                            scaleComparer.CardDeselectionAnimation();
                        }
                        
                    }
                }

            }
        }
    }

    public void RescaleCards()
    {
        for (int i = 0; i < playerCardsEmpty.transform.childCount; ++i)
            playerCardsEmpty.transform.GetChild(i).localScale = new Vector3(1, 1, 1);
    }

    public void RescaleArmoury()
    {
        for (int i = 0; i < armouryEmpty.transform.childCount; ++i)
            armouryEmpty.transform.GetChild(i).localScale = new Vector3(1, 1, 1);
    }

    public void SetupEnemies()
    {
        enemies.ReadEnemies();
        //enemies.SetupDeck();
        enemies.SetupTestDeck();
        enemies.SetupFinalBoss();
        enemies.AddHealth(additionalHealth);
    }

    public void SetupCards()
    {
        cards.ReadCards();
        cards.ReadTransforms();
        cards.SetupDeck();
        cards.SetupTopDeck(players.Count);
    }

    void SetupPlayerDecks()
    {
        foreach(var p in players)
        {
            p.deck = new List<CardOffline>(cards.GetStartingHand());
        }
    }

    void ShiftPlayers()
    {
        int id = playerIDs[0];
        for (int i = 0; i < playerIDs.Count - 1; ++i)
            playerIDs[i] = playerIDs[i + 1];
        playerIDs[playerIDs.Count - 1] = id;
    }

    public void UpdateIdOrder()
    {

        /*
        if (playerIDs[0] >= maxId)
            ShiftPlayers();
        else
        {
            int id = playerIDs[0];
            playerIDs[0] = playerIDs[id];
            playerIDs[id] = id;
        }*/
        ShiftPlayers();

    }

    public void HandleShowCards()
    {
        if (playerCardsEmpty.activeInHierarchy)
        {
            if(!StateHandler.IsChoosingState(state))
                playerCardsEmpty.SetActive(false);
        }
        else
        {
            playerCardsEmpty.SetActive(true);
            for (int i = 0; i < playerCardsEmpty.transform.childCount; ++i)
                playerCardsEmpty.transform.GetChild(i).gameObject.SetActive(false);
            PlayerOffline p = players[playerIDs[0] - 1];
            GameObject tmp;
            for (int i = 0; i < p.deck.Count; ++i)
            {
                tmp = playerCardsEmpty.transform.GetChild(i).gameObject;
                tmp.SetActive(true);
                tmp.GetComponentInChildren<CardPrefab>(true).UpdateCard(p.deck[i], p.name);
            }
        }
    }

    public void OnClickStart()
    {
        state = State.REVEAL_ENEMY;
        revealFinalBoss.gameObject.SetActive(true);
        revealArmouryButton.gameObject.SetActive(true);

        startButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(true);
    }

    public void OnClickFinalBoss()
    {
        if (finalBossEmpty.activeInHierarchy)
            finalBossEmpty.SetActive(false);
        else
        {
            finalBossEmpty.SetActive(true);
            finalBossEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemies.finalBoss);
        }
    }

    public void OnClickArmoury()
    {
        if (armouryEmpty.activeInHierarchy)
        {
            if(!StateHandler.IsDreamingState(state))
                armouryEmpty.SetActive(false);
            else
            {
                GameObject tmp;
                for (int i = 0; i < cards.topDeck.Count; ++i)
                {
                    tmp = armouryEmpty.transform.GetChild(i).gameObject;
                    tmp.SetActive(true);
                    tmp.GetComponentInChildren<CardPrefab>(true).UpdateCard(cards.topDeck[i]);
                }
            }
        }
        else
        {
            armouryEmpty.SetActive(true);
            for (int i = 0; i < armouryEmpty.transform.childCount; ++i)
                armouryEmpty.transform.GetChild(i).gameObject.SetActive(false);
            GameObject tmp;
            for (int i = 0; i < cards.topDeck.Count; ++i)
            {
                tmp = armouryEmpty.transform.GetChild(i).gameObject;
                tmp.SetActive(true);
                tmp.GetComponentInChildren<CardPrefab>(true).UpdateCard(cards.topDeck[i]);
            }
        }
    }

    public void HandleWinners()
    {
        foreach (var player in players)
            player.GetTotalPoints();

        PlayerOffline p;
        for(int i=0;i<players.Count -1;++i)
            for(int j=i+1;j<players.Count;++j)
                if(players[i].totalPoints < players[j].totalPoints)
                {
                    p = players[i];
                    players[i] = players[j];
                    players[j] = p;
                }

        GameObject tmp;
        for(int i=0;i<players.Count;++i)
        {
            tmp = Instantiate(winnerPrefab, winnerEmpty.transform);
            tmp.GetComponentInChildren<WInnerPrefab>(true).UpdateWinner(players[i], i + 1);
        }

    }

    public void ClearPlayers()
    {
        staticPlayers.Clear();
    }

    public void EffectHandler(int id)
    {
        switch(id)
        {
            case 1007:
                {
                    foreach (PlayerOffline p in players)
                        p.TakeDamage(1, true);
                    break;
                }

            case 1008:
                {
                    foreach (PlayerOffline p in players)
                        p.LostPoints(2);
                    break;
                }
            case 1010:
                {
                    foreach (PlayerOffline p in players)
                        if (p.discardedCards.Count >= 4)
                            p.RecoverRandomCard();
                    break;
                }
            case 1017:
                {
                    int mx = 0;
                    foreach (PlayerOffline p in players)
                        if (p.deck.Count > mx)
                            mx = p.deck.Count;
                    foreach (PlayerOffline p in players)
                        if (p.deck.Count == mx)
                            p.TakeDamage(2, true);
                    break;
                }
            case 1018:
                {
                    int mn = players[0].totalPoints;
                    foreach (PlayerOffline p in players)
                        if (p.totalPoints < mn)
                            mn = p.totalPoints;
                    foreach (PlayerOffline p in players)
                        if (p.totalPoints == mn)
                            p.unbankedPoints += 3;
                    break;
                }
            case 1019:
                {
                    enemy.health += players.Count;
                    currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                    break;
                }
            case 1020:
                {
                    int mn = players[0].totalPoints;
                    foreach (PlayerOffline p in players)
                        if (p.totalPoints < mn)
                            mn = p.totalPoints;
                    foreach (PlayerOffline p in players)
                        if (p.totalPoints == mn)
                            p.unbankedPoints += 2;
                    break;
                }
            case 3002:
                {
                    foreach (Enemy e in enemies.deck)
                        e.health += 2;
                    break;
                }
            case 3003:
                {
                    PlayerOffline.maxHealth = 8;
                    foreach (PlayerOffline p in players)
                        p.health = PlayerOffline.maxHealth;
                    break;
                }

            case 1001:
                {
                    int mn = 100;
                    foreach (PlayerOffline p in players)
                        if (p.health < mn)
                            mn = p.health;
                    foreach (PlayerOffline p in players)
                        if (p.health == mn)
                            p.TakeDamage(100, false);
                    break;
                }
            case 1009:
                {
                    foreach (PlayerOffline p in players)
                        p.LostPoints(3);
                    break;
                }
            case 1011:
                {
                    int mx = 0;
                    foreach (PlayerOffline p in players)
                        if (p.unbankedPoints > mx)
                            mx = p.unbankedPoints;
                    foreach (PlayerOffline p in players)
                        if (p.unbankedPoints == mx)
                            p.unbankedPoints -= 5;
                    break;
                }
            case 1012:
                {
                    foreach (PlayerOffline p in players)
                        p.GainRandomTrophy();
                    break;
                }
            case 1013:
                {
                    foreach (PlayerOffline p in players)
                        p.TakeDamage(2, true);
                    break;
                }
            case 1014:
                {
                    foreach (PlayerOffline p in players)
                        p.DiscardRandomCards(2);

                    break;
                }
            case 1015:
                {
                    int mx = 0;
                    foreach (PlayerOffline p in players)
                        if (p.unbankedPoints > mx)
                            mx = p.unbankedPoints;
                    foreach (PlayerOffline p in players)
                        if (p.unbankedPoints == mx)
                            p.health -= 5;
                    break;
                }
            case 1016:
                {
                    int mx = 0;
                    foreach (PlayerOffline p in players)
                        if (p.totalPoints > mx)
                            mx = p.totalPoints;
                    foreach (PlayerOffline p in players)
                        if (p.totalPoints == mx)
                            p.health -= 3;
                    break;
                }
            case 2001:
                {
                    foreach (PlayerOffline p in players)
                        if (p.hasAttacked && p.card.damage >= 3)
                            p.unbankedPoints += 1;
                    break;
                }
            case 2003:
                {
                    foreach (PlayerOffline p in players)
                        if (p.hasAttacked || p.hasDamaged)
                            p.GainRandomTrophy();
                    break;
                }
            case 3001:
                {
                    foreach (PlayerOffline p in players)
                        p.Heal(2);
                    break;
                }
            case 3004:
                {
                    foreach (PlayerOffline p in players)
                        if (p.card.damage <= 2)
                            p.TakeDamage(1, true);
                    break;
                }
            case 3005:
                {
                    foreach(PlayerOffline p in players)
                        if(p.isDead)
                        {
                            foreach (PlayerOffline p2 in players)
                                if (p2.id != p.id)
                                    p2.GainRandomTrophy();
                        }
                    break;
                }
            case 3006:
                {
                    foreach (PlayerOffline p in players)
                        p.TakeDamage(1, true);
                    break;
                }
        }
    }

    public void EffectHandler(PlayerOffline player)
    {
        switch(player.card.id)
        {
            case 103:
                {
                    if(player.card.isTransform)
                    {
                        foreach (PlayerOffline p in players)
                            if (p.id != player.id)
                                p.TakeDamage(1, false);
                    }
                    break;
                }
            case 104:
                {
                    foreach (PlayerOffline p in players)
                        if (!p.card.IsWeapon())
                            p.TakeDamage(2, false);
                    if(player.card.isTransform)
                    {
                        player.TakeDamage(1, false);
                        if(!player.isDead)
                        {
                            foreach (PlayerOffline p in players)
                                if (p.id != player.id)
                                    p.TakeDamage(2, false);
                            enemy.TakeDamage(player, 2, false);
                            currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                        }
                    }
                    break;
                }
            case 105:
                {
                    foreach (PlayerOffline p in players)
                        if (p.id != player.id)
                            p.TakeDamage(1, false);
                    if(player.card.isTransform)
                    {
                        foreach (PlayerOffline p in players)
                            if (p.id != player.id)
                                p.TakeDamage(1, false);
                        enemy.TakeDamage(player, 1, false);
                        currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                    }
                    break;
                }
            case 106:
                {
                    foreach(PlayerOffline p in players)
                        if(p.id != player.id)
                        {
                            if(p.card.cardType == CardType.WEAPON_RANGED)
                            {
                                player.unbankedPoints += p.LostPoints(1);
                            }
                        }
                    if(player.card.isTransform)
                    {
                        int v = player.CountCards(CountType.RANGED);
                        player.card.damage += v;
                    }
                    break;
                }
            case 107:
                {
                    int heal = 1;
                    foreach (CardOffline c in player.discardedCards)
                        if (c.cardType == CardType.WEAPON_MELEE && c.id != player.card.id)
                            heal += 2;
                    player.Heal(heal);
                    
                    if(player.card.isTransform)
                    {
                        if (heal >= 5)
                            player.card.damage += 2;
                    }
                    break;
                }
            case 108:
                {
                    foreach (PlayerOffline p in players)
                        if (p.id != player.id)
                            p.TakeDamage(1, false);
                    break;
                }
            case 109:
                {
                    if(player.card.isTransform)
                    {
                        foreach (PlayerOffline p in players)
                            if (p.id != player.id)
                                p.TakeDamage(1, false);
                    }
                    break;
                }
            case 201:
                {
                    enemy.TakeDamage(player, player.card.damage, true);
                    currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                    break;
                }
            case 202:
                {
                    if(player.unbankedPoints >= 1)
                    {
                        player.unbankedPoints -= 1;
                        player.card.damage += 2;
                        if (player.card.isTransform)
                            player.card.damage += 1;
                    }
                    break;
                }
            case 203:
                {
                    if (player.card.isTransform)
                        player.card.isImmune = true;
                    else
                    {
                        bool immune = true;
                        foreach (PlayerOffline p in players)
                            if (p.id != player.id)
                                if (p.card.cardType == CardType.WEAPON_RANGED)
                                {
                                    immune = false;
                                    break;
                                }
                        if (immune)
                            player.card.isImmune = true;
                    }
                    break;
                }
            case 204:
                {
                    foreach(PlayerOffline p in players)
                        if(p.id != player.id)
                            if(p.card.IsWeapon() && p.card.cardType == CardType.WEAPON_MELEE)
                                p.doubleDamageMelee = true;
                    break;
                }
            case 205:
                {
                    foreach (PlayerOffline p in players)
                        if (p.id != player.id)
                            if (p.card.IsWeapon() && p.card.cardType == CardType.WEAPON_RANGED)
                                p.doubleDamageRanged = true;
                    break;
                }
            case 206:
                {
                    foreach (PlayerOffline p in players)
                        if (p.id != player.id)
                            p.TakeDamage(2, false, true);
                    if(player.card.isTransform)
                    {
                        enemy.TakeDamage(player, 1, false);
                        currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                    }
                    break;
                }
            case 207:
                {
                    player.card.isImmune = true;
                    break;
                }
            case 208:
                {
                    foreach(PlayerOffline p in players)
                    {
                        if (p.id != player.id)
                            if (p.card.IsWeapon() && p.card.cardType == CardType.WEAPON_MELEE)
                                p.card.damage--;
                    }

                    if(player.card.isTransform)
                    {
                        foreach (PlayerOffline p in players)
                            if (p.id != player.id)
                                p.TakeDamage(1, false);
                    }
                    break;
                }
            case 209:
                {
                    foreach (PlayerOffline p in players)
                        if (p.id != player.id)
                        {
                            if (p.card.cardType == CardType.WEAPON_MELEE)
                                p.TakeDamage(2, false);
                            else
                                if (p.card.cardType == CardType.WEAPON_RANGED)
                                p.TakeDamage(1, false);
                        }
                    if (player.card.isTransform)
                    {
                        enemy.TakeDamage(player, 2, false);
                        currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                    }
                    break;
                }
            case 210:
                {
                    foreach (PlayerOffline p in players)
                        if (p.id != player.id)
                            p.TakeDamage(1, false);
                    if(player.card.isTransform)
                    {
                        foreach (PlayerOffline p in players)
                            if (p.id != player.id)
                                p.TakeDamage(1, false);
                        enemy.TakeDamage(player, 1, false);
                        currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                    }
                    break;
                }
            case 301:
                {
                    player.Heal(2);
                    player.Bank();
                    break;
                }
            case 305:
                {
                    enemy.health += players.Count;
                    currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                    break;
                }
            case 306:
                {
                    foreach(PlayerOffline p in players)
                    {
                        p.TakeDamage(1, false);
                    }

                    enemy.TakeDamage(player, 1, false);
                    currentEnemyEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemy);
                    break;
                }
        }
    }

    public void HandlePistolHeal()
    {
        foreach (PlayerOffline p in players)
            if (!p.isDead)
            {
                if (p.card.id == 201 && p.card.isTransform)
                    p.Heal(p.sufferedDamageFromEnemy / 2);
            }
    }
}
