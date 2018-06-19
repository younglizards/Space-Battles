using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Deck
{
    readonly string deckId;

    Stack<Card> cards;

    public Deck(string deckId)
    {
        this.deckId = deckId;
        Build();
    }

    /// <summary>
    /// Builds the deck for the current game.
    /// </summary>
    void Build()
    {
        TextAsset deckFile = Resources.Load<TextAsset>("Decks/" + deckId);

        DeckData deckData = JsonUtility.FromJson<DeckData>(deckFile.text);
        cards = new Stack<Card>();

        for (int i = 0; i < deckData.cardStacks.Count; i++)
        {
            CardStack stack = deckData.cardStacks[i];

            CardData data;
            if (!CardsDatabase.GetById(stack.id, out data))
            {
                continue;
            }

            for (int j = 0; j < stack.ammount; j++)
            {
                // N copies of the same card
                cards.Push(new Card(data));
            }
        }

        Shuffle();
    }

    /// <summary>
    /// Shuffles the deck.
    /// </summary>
    public void Shuffle()
    {
        List<Card> tmp = new List<Card>(cards);
        tmp.Shuffle();

        cards.Clear();

        for (int i = 0; i < tmp.Count; i++)
        {
            cards.Push(tmp[i]);
        }
    }

    /// <summary>
    /// Requests a specific ammount from the deck, or less if there are no enough cards.
    /// </summary>
    /// <param name="ammount">Ammount of solicited cards</param>
    /// <returns>Array of cards</returns>
    public Card[] DrawCards(int ammount)
    {
        // We could make an array based on the current size and the ammount requested, 
        // but maybe the deck could be modified inside the loop - so we better do it this way.
        List<Card> cardsDrawn = new List<Card>();

        for (int i = 0; i < ammount; i++)
        {
            // Break if no more cards in the deck
            if (cards.Count == 0)
            {
                break;
            }
            cardsDrawn.Add(cards.Pop());
        }
        return cardsDrawn.ToArray();
    }

    public void AddCards(List<Card> cardsToAdd)
    {
        for (int i = 0; i < cardsToAdd.Count; i++)
        {
            cards.Push(cardsToAdd[i]);
        }
    }
}
