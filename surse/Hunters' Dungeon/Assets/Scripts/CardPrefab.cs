using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardPrefab : MonoBehaviour
{
    public Image cardBackground;
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
        cardBackground.color = GetColour(player.card.cardType);
        cardName.text = player.card.name;
        cardImage.sprite = null;
        damageValue.text = player.card.damage.ToString();
        if (int.Parse(damageValue.text) < 1)
        {
            damageValue.transform.parent.gameObject.SetActive(false);
            if (player.card.effect.countType != CountType.NONE)
            {
                damageValue.transform.parent.gameObject.SetActive(true);
                damageValue.text = player.CountCards(GetCountType(player.card.id)).ToString();
            }
        }
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
        if (player.transforms == 0)
            transformButton.gameObject.SetActive(false);
    }

    public void UpdateCard(CardOffline card, string playerName)
    {
        cardBackground.color = GetColour(card.cardType);
        cardName.text = card.name;
        cardImage.sprite = null;
        damageValue.text = card.damage.ToString();
        if (int.Parse(damageValue.text) < 1)
        {
            damageValue.transform.parent.gameObject.SetActive(false);
            if (card.effect.countType != CountType.NONE)
            {
                damageValue.transform.parent.gameObject.SetActive(true);
                damageValue.text = "X";
            }
        }
        else
            damageValue.transform.parent.gameObject.SetActive(true);

        if(card.effect.countType != CountType.NONE)
        {
            damageValue.transform.parent.gameObject.SetActive(true);
            damageValue.text = "X";
        }

        description.text = card.description;
        cardType.text = CardTypeToString.CardToString(card.cardType);
        cardOwner.text = playerName;
        cardOwner.gameObject.SetActive(true);

        if (card.cardType == CardType.ITEM || card.cardType == CardType.OTHER)
            transformButton.gameObject.SetActive(false);
        else
            transformButton.gameObject.SetActive(true);
    }

    public void UpdateCard(CardOffline card, PlayerOffline player)
    {
        cardBackground.color = GetColour(card.cardType);
        cardName.text = card.name;
        cardImage.sprite = null;
        damageValue.text = card.damage.ToString();
        if (int.Parse(damageValue.text) < 1)
        {
            damageValue.transform.parent.gameObject.SetActive(false);
            
            if (card.effect.countType != CountType.NONE)
            {
                damageValue.transform.parent.gameObject.SetActive(true);
                damageValue.text = player.CountCards(GetCountType(card.id)).ToString();
            }
        }
        else
            damageValue.transform.parent.gameObject.SetActive(true);
        description.text = card.description;
        cardType.text = CardTypeToString.CardToString(card.cardType);
        cardOwner.text = player.name;
        cardOwner.gameObject.SetActive(true);

        if (card.cardType == CardType.ITEM || card.cardType == CardType.OTHER)
            transformButton.gameObject.SetActive(false);
        else
            transformButton.gameObject.SetActive(true);

        if(player.transforms == 0)
        {
            transformButton.gameObject.SetActive(false);
        }
    }

    public void UpdateCard(CardOffline card)
    {
        cardBackground.color = GetColour(card.cardType);
        cardName.text = card.name;
        cardImage.sprite = null;
        damageValue.text = card.damage.ToString();

        if (int.Parse(damageValue.text) < 1)
        {
            damageValue.transform.parent.gameObject.SetActive(false);
            if (card.effect.countType != CountType.NONE)
            {
                damageValue.transform.parent.gameObject.SetActive(true);
                damageValue.text = "X";
            }
        }
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

    CountType GetCountType(int id)
    {
        // 6A0404
        return id switch
        {
            302 => CountType.POINTS,
            303 => CountType.MELEE,
            304 => CountType.RANGED,
            _ => CountType.NONE
        };
    }

    Color32 GetColour(CardType ct)
    {
        return ct switch
        {
            CardType.WEAPON_MELEE => new Color32(139, 14, 14, 255),
            CardType.WEAPON_RANGED => new Color32(0, 89, 248, 255),
            _ => new Color32(96, 96, 96, 255)
        };
    }
}
