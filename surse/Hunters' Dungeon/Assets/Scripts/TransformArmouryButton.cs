
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformArmouryButton : MonoBehaviour
{
    public GameObject card;
    public int index;

    private void Start()
    {
        index = card.name[card.name.Length - 1] - '1';
    }
    public void HandleOnClick()
    {
        CardOffline c = OffGameManager.cards.topDeck[index];
        if(c.IsWeapon())
        {
            c.TransformCard();
            card.GetComponentInChildren<CardPrefab>(true).UpdateCard(c);
        }
    }
}
