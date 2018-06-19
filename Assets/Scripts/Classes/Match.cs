
using System;
using System.Collections.Generic;

public class Match
{
    public readonly Player player1;
    public readonly Player player2;
    public readonly Player[] players;

    public readonly Deck commonDeck;
    public readonly Turn turn;
    public readonly Board board;

    readonly Dictionary<int, Player> playersById = new Dictionary<int, Player>();
    Dictionary<int, bool> firstHandDrawn = new Dictionary<int, bool>();

    public Match(int id1, int id2)
    {
        player1 = new Player(id1, "s_deck_01");
        player2 = new Player(id2, "s_deck_01");
        players = new Player[] { player1, player2 };

        playersById[id1] = player1;
        playersById[id2] = player2;

        firstHandDrawn[id1] = false;
        firstHandDrawn[id2] = false;

        commonDeck = new Deck("c_deck_01");
        board = new Board(this);
        turn = new Turn(this);
    }

    public Player GetPlayer(int id)
    {
        return playersById[id];
    }

    public Player GetOppositePlayer(int id)
    {
        if (player1.id == id)
        {
            return player2;
        }
        else if (player2.id == id)
        {
            return player1;
        }
        else
        {
            throw new KeyNotFoundException("player with id " + id + " not found");
        }
    }

    public List<Card> RequestFirstHand(int playerId)
    {
        Player player = GetPlayer(playerId);

        if (firstHandDrawn[player.id])
        {
            return new List<Card>();
        }

        List<Card> cards = new List<Card>();

        cards.AddRange(commonDeck.DrawCards(GameConstants.NUMBER_OF_COMMON_CARDS_FIRST_TURN));
        cards.AddRange(player.deck.DrawCards(GameConstants.NUMBER_OF_SPACESHIP_CARDS_FIRST_TURN));

        player.hand.AddCards(cards);

        firstHandDrawn[player.id] = true;

        return cards;
    }

    public List<Card> StartTurnRequestCards(int playerId, int numberOfCommonCards, int numberOfPlayerCards)
    {
        if (turn.CurrentPlayerId != playerId)
        {
            return new List<Card>();
        }

        Player player = GetPlayer(playerId);

        List<Card> cards = new List<Card>();

        cards.AddRange(commonDeck.DrawCards(numberOfCommonCards));
        cards.AddRange(player.deck.DrawCards(numberOfPlayerCards));

        turn.ChoosingCard(cards);

        return cards;
    }

    public Card PickCard(int playerId, int cardIdx)
    {
        if (turn.CurrentPlayerId != playerId)
        {
            return null;
        }

        Player player = GetPlayer(playerId);

        List<Card> cards = turn.GetChooseableCards();
        Card card = cards[cardIdx];

        // We remove the card from the list
        cards.RemoveAt(cardIdx);

        // We add them back to the common deck
        commonDeck.AddCards(cards);

        // We add the card to the player hand
        player.hand.AddCard(card);

        return card;
    }

    public bool PlayCard(int playerId, string cardId, int positionInHand, int squarePosition)
    {
        // TODO better error handling

        if (turn.CurrentPlayerId != playerId)
        {
            return false;
        }

        Player player = GetPlayer(playerId);
        Card card = player.hand.GetCardAtPosition(positionInHand);
        
        if (card == null || card.Id != cardId)
        {
            return false;
        }

        if (Rules.IsUnitCard(card.Id) && board.IsSquareEmpty(playerId, squarePosition)) 
        {
            // Unit card and empty slot - we can play the card there.
            player.hand.RemoveCardAtPosition(positionInHand);
            board.PlayCard(playerId, card, squarePosition);
            return true;
        }

        // TODO add remaining logic
        return false;
    }
}
