using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand
{
    List<CardInHand> cards;

    public int CardCount { get { return cards.Count; } }

    public Hand(Player player)
    {
        cards = new List<CardInHand>();
    }

    /// <summary>
    /// Adds the supplied cards to the hand.
    /// </summary>
    /// <param name="cardsToAdd">Cards to add to the hand.</param>
    public void AddCards(List<Card> cardsToAdd)
    {
        for (int i = 0; i < cardsToAdd.Count; i++)
        {
            CardInHand card = new CardInHand
            {
                card = cardsToAdd[i],
                position = cards.Count
            };
            cards.Add(card);
        }
    }

    public void AddCard(Card card)
    {
        CardInHand cardH = new CardInHand
        {
            card = card,
            position = cards.Count
        };
        cards.Add(cardH);
    }

    public Card GetCardAtPosition(int cardPositionInHand)
    {
        if (cardPositionInHand < 0 || cardPositionInHand >= cards.Count)
        {
            throw new Exception("Index out of bounds: " + cardPositionInHand + " (hand has " + cards.Count + " cards)");
        }
        return cards[cardPositionInHand].card;
    }

    public Card RemoveCardAtPosition(int cardPositionInHand)
    {
        Card card = cards[cardPositionInHand].card;
        cards.RemoveAt(cardPositionInHand);

        RecalculatePositions();

        return card;
    }

    void RecalculatePositions()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].position = i;
        }
    }
}
