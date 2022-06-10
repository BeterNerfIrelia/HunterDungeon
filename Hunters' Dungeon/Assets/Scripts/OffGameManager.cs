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

    public EventSystem eventSystem;

    public GameObject mainPlayer;
    public GameObject playerPrefab;
    public GameObject otherPlayersEmpty;

    public GameObject playerCardsEmpty;
    public Button showPlayerCards;
    public Button showDiscardCards;

    public GameObject currentEnemyEmpty;
    public GameObject finalBossEmpty;
    public GameObject revealCardsEmpty;
    public GameObject armouryEmpty;

    public static Cards cards;
    public static Enemies enemies;
    public static State state;
    public static Enemy enemy;

    [HideInInspector] public List<int> playerIDs;
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

    private void Start()
    {
        firstRound = true;
        once = false;
        enemyChosen = false;
        players = new List<PlayerOffline>(staticPlayers);
        staticPlayers.Clear();
        cards = new Cards();
        enemies = new Enemies();
        maxId = -1;
        playerIDs = new List<int>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();


        state = State.START;
        currentEnemyEmpty.SetActive(false);
        finalBossEmpty.SetActive(false);
        playerCardsEmpty.SetActive(false);
        revealCardsEmpty.SetActive(false);
        armouryEmpty.SetActive(false);
        showDiscardCards.gameObject.SetActive(false);

        continueButton.gameObject.SetActive(false);
        revealCardsButton.gameObject.SetActive(false);
        revealArmouryButton.gameObject.SetActive(false);
        rollDiceButton.gameObject.SetActive(false);

        playerIDs.Clear();
        playerIDs = new List<int>();
        foreach (var p in players)
        {
            Debug.LogFormat("{0} is {1}", p.name, p.bot ? "Bot" : "Player");
            playerIDs.Add(p.id);

            if (p.id > maxId)
                maxId = p.id;
        }
        additionalHealth = players.Count - 3;
        for (int i = 1; i < players.Count; ++i)
            Instantiate(playerPrefab, otherPlayersEmpty.transform);
        SetupCards();
        SetupPlayerDecks();
        SetupEnemies();
        finalBossEmpty.GetComponentInChildren<EnemyPrefab>(true).UpdateEnemy(enemies.finalBoss);
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
                        enemy = enemies.deck[enemies.GetRandomEnemy()];
                    }
                    if(!once)
                    {
                        once = true;
                        finalBossEmpty.SetActive(false);
                    }

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
                    break;
                }
            case State.ENEMY_ATTACK:
                {
                    continueButton.interactable = true;
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
                    state = State.CHOOSE_CARD1;
                    revealFinalBoss.interactable = false;
                    revealArmouryButton.interactable = false;
                    showPlayerCards.interactable = false;

                    mainPlayer.GetComponentInChildren<PlayerPrefab>(true).HandleShowCards(players[playerIDs[0] - 1]);
                    revealCardsButton.gameObject.SetActive(true);
                    once = false;
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
                    state = State.ROLL_DICE;
                    break;
                }
            case State.ROLL_DICE:
                {
                    continueButton.interactable = false;
                    break;
                }
            case State.ENEMY_ATTACK:
                {
                    foreach(PlayerOffline p in players)
                    {
                        p.TakeDamage(Dice.rollValue);
                    }
                    state = State.HUNTER_ATTACK1;
                    break;
                }
        }
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

    public void HandleChooseCommon()
    {
        if (!players[playerIDs[0] - 1].hasClickedOnCard)
            return;

        players[playerIDs[0] - 1].ChooseCard();
        RescaleCards();
        UpdateIdOrder();
        state = StateHandler.ChangeState(state);
        once = false;
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
                        break;
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

    public void SetupEnemies()
    {
        enemies.ReadEnemies();
        enemies.SetupDeck();
        enemies.SetupFinalBoss();
        enemies.AddHealth(additionalHealth);
    }

    public void SetupCards()
    {
        cards.ReadCards();
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
        if (playerIDs[0] >= maxId)
            ShiftPlayers();
        else
        {
            int id = playerIDs[0];
            playerIDs[0] = playerIDs[id];
            playerIDs[id] = id;
        }
    }

    public void HandleShowCards()
    {
        if (playerCardsEmpty.activeInHierarchy)
            playerCardsEmpty.SetActive(false);
        else
        {
            playerCardsEmpty.SetActive(true);
            for (int i = 0; i < playerCardsEmpty.transform.childCount; ++i)
                playerCardsEmpty.transform.GetChild(i).gameObject.SetActive(false);
            PlayerOffline p = players[playerIDs[0] - 1];
            GameObject tmp;
            for (int i=0;i<p.deck.Count;++i)
            {
                tmp= playerCardsEmpty.transform.GetChild(i).gameObject;
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
            armouryEmpty.SetActive(false);
        else
        {
            armouryEmpty.SetActive(true);
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
    }
}
