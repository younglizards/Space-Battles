using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Player
{
    public readonly int id;

    public readonly Hand hand;
    public readonly Deck deck;

    public Player(int playerId, string deckId)
    {
        this.id = playerId;
        hand = new Hand(this);

        deck = new Deck(deckId);
    }
}
