using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// GUI component of the Hand class. Contains also some Photon's callbacks.
/// </summary>
public class HandGUI_canvas : MonoBehaviour
{
    Transform myTransform;
    SideGUI_canvas side;

    private void Start()
    {
        side = GetComponentInParent<SideGUI_canvas>();

        myTransform = transform;

        // Destroy preloaded data (Editor)
        myTransform.DestroyAllChildren();

        // Callbacks
        HandController hand = FindObjectOfType<HandController>();

        if (side.owner == Owner.PLAYER)
        {
            hand.OnPlayerHandUpdated += UpdatePlayerCards;
            hand.OnPlayerPlayedCard += PlayerPlayedCard;
        }
        else if (side.owner == Owner.ENEMY)
        {
            hand.OnEnemyHandUpdated += UpdateEnemyCards;
        }
        else
        {
            Debug.LogError("Who are you?");
        }
    }

    // We don't have to fiddle with a 'Hand' instance in this case since this is the enemy.
    // We just trust and render whatever the master client sends us.
    void UpdateEnemyCards(int ammount)
    {
        // Only makes sense if this is the enemy's hand
        Assert.IsTrue(side.owner == Owner.ENEMY);

        int numberOfCardsInEnemyHand = transform.childCount;

        // No work needed if the total cards remain the same.
        if (numberOfCardsInEnemyHand == ammount)
        {
            return;
        }

        // We have to remove cards
        if (numberOfCardsInEnemyHand > ammount)
        {
            for (int i = numberOfCardsInEnemyHand - 1; i >= ammount; i--)
            {
                Destroy(myTransform.GetChild(i).gameObject);
            }
        }
        else
        {
            // We have to add cards
            int add = ammount - numberOfCardsInEnemyHand;
            CardGUI_canvas cardGUIPrefab = Prefabs.Get("cardGUI_canvas").GetComponent<CardGUI_canvas>();

            for (int i = 0; i < add; i++)
            {
                CardGUI_canvas cardCpy = Instantiate(cardGUIPrefab, myTransform);
                cardCpy.Location = CardLocation.ENEMY_HAND;
            }
        }
    }

    CardGUI_canvas GetCardGUIAtPosition(int cardPosition)
    {
        Transform c = transform.GetChild(cardPosition);
        return c.GetComponent<CardGUI_canvas>();
    }

    void RemoveCardAtPosition(int cardPosition)
    {
        Transform c = transform.GetChild(cardPosition);
        GameObject.Destroy(c.gameObject);
    }

    // In this case we have to add the cards to the hand instance since this is our hand.
    void UpdatePlayerCards(Card[] cardsToAdd)
    {
        Assert.IsTrue(side.owner == Owner.PLAYER);
        CardGUI_canvas cardGUIPrefab = Prefabs.Get("cardGUI_canvas").GetComponent<CardGUI_canvas>();

        for (int i = 0; i < cardsToAdd.Length; i++)
        {
            CardGUI_canvas cardGUI = Instantiate(cardGUIPrefab, myTransform);
            cardGUI.Location = CardLocation.PLAYER_HAND;
            cardGUI.CardId = cardsToAdd[i].Id;
        }
    }

    public void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    void PlayerPlayedCard(int response, string cardId, int cardPositionInHand, int squarePosition)
    {
        CardGUI_canvas cardGUI = GetCardGUIAtPosition(cardPositionInHand);

        if (response == Responses.CARD_PLAYED_FAILED)
        {
            RebuildLayout();
            return;
        }

        string cardInHandId = cardGUI.CardId;

        if (cardInHandId != cardId)
        {
            Debug.LogWarning(string.Format("Played card (%s) doesn't match the card at that position (%s, %d)",
                cardId, cardInHandId, cardPositionInHand));
            return;
        }

        RemoveCardAtPosition(cardPositionInHand);
        RebuildLayout();
    }
}