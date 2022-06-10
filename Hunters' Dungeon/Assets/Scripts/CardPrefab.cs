using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPrefab : MonoBehaviour
{
    public TextMeshProUGUI cardName;
    public Image cardImage;
    public TextMeshProUGUI damageValue;
    public TextMeshProUGUI description;
    public TextMeshProUGUI cardType;
    public TextMeshProUGUI cardOwner;
    public Button transformButton;
    public Animation cardAnimation;
    public bool clickedOn = false;
    public Vector3 originalScale = new Vector3(1, 1, 1);
    public Vector3 selectedScale = new Vector3(1.5f, 1.5f, 1);

    public void UpdateCard(PlayerOffline player)
    {
        cardName.text = player.card.name;
        cardImage.sprite = null;
        damageValue.text = player.card.damage.ToString();
        if (int.Parse(damageValue.text) < 1)
            damageValue.transform.parent.gameObject.SetActive(false);
        else
            damageValue.transform.parent.gameObject.SetActive(true);
        description.text = player.card.description;
        cardType.text = CardTypeToString.CardToString(player.card.cardType);
        cardOwner.text = player.name;
        cardOwner.gameObject.SetActive(true);

        if (player.card.cardType == CardType.ITEM || player.card.cardType == CardType.OTHER)
            transformButton.gameObject.SetActive(false);
        else
            transformButton.gameObject.SetActive(true);
    }

    public void UpdateCard(CardOffline card, string playerName)
    {
        cardName.text = card.name;
        cardImage.sprite = null;
        damageValue.text = card.damage.ToString();
        if (int.Parse(damageValue.text) < 1)
            damageValue.transform.parent.gameObject.SetActive(false);
        else
            damageValue.transform.parent.gameObject.SetActive(true);
        description.text = card.description;
        cardType.text = CardTypeToString.CardToString(card.cardType);
        cardOwner.text = playerName;
        cardOwner.gameObject.SetActive(true);

        if (card.cardType == CardType.ITEM || card.cardType == CardType.OTHER)
            transformButton.gameObject.SetActive(false);
        else
            transformButton.gameObject.SetActive(true);
    }

    public void UpdateCard(CardOffline card)
    {
        cardName.text = card.name;
        cardImage.sprite = null;
        damageValue.text = card.damage.ToString();

        if (int.Parse(damageValue.text) < 1)
            damageValue.transform.parent.gameObject.SetActive(false);
        else
            damageValue.transform.parent.gameObject.SetActive(true);
        description.text = card.description;
        cardType.text = CardTypeToString.CardToString(card.cardType);

        cardOwner.gameObject.SetActive(false);
        if (card.cardType == CardType.ITEM || card.cardType == CardType.OTHER)
            transformButton.gameObject.SetActive(false);
        else
            transformButton.gameObject.SetActive(true);
    }

    public void CardSelectionAnimation()
    {
        cardAnimation.Play("Card_Selected");
        clickedOn = true;
    }

    public void CardDeselectionAnimation()
    {
        cardAnimation.Play("Card_Deselected");
        clickedOn = false;
    }
}
