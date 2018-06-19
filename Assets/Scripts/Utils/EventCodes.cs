using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class EventCodes
{
    // R* Codes are for requests from client to master
    // N* Codes are for notifications from anyone to everyone
    // A* Codes are for responses from master to client
    // B* Codes are for broadcastings from anyone to everyone

    /// <summary>
    /// Event raised by any player when it has finished loading the game. 
    /// </summary>
    public const byte N_PLAYER_READY = 0;

    /// <summary>
    /// Event raised by the master to notify when all players have finished loading the game.
    /// </summary>
    public const byte B_ALL_PLAYERS_READY = 1;

    /// <summary>
    /// Event raised by all clients when all players are ready, to request the starting hand.
    /// </summary>
    public const byte R_FIRST_HAND = 2;

    /// <summary>
    /// Event raised by the MasterClient returning the cards requested by the client.
    /// </summary>
    public const byte A_FIRST_HAND = 3;

    /// <summary>
    /// Event raised by the master client to broadcast who the first player is.
    /// </summary>
    public const byte B_FIRST_PLAYER = 4;

    /// <summary>
    /// Event raised by the master when the current player's changes.
    /// </summary>
    public const byte N_END_TURN = 5;

    /// <summary>
    /// Event raised by the master when the current player's changes.
    /// </summary>
    public const byte N_START_TURN = 6;

    /// <summary>
    /// Event raised by the current player to request cards.
    /// </summary>
    public const byte R_BEGIN_TURN_DRAW_CARDS = 8;

    /// <summary>
    ///  Event raised by the master to give the cards requested. 
    /// </summary>
    public const byte A_BEGIN_TURN_DRAW_CARDS = 9;

    /// <summary>
    /// Event raised by a client to request the use of a card to the master client.
    /// </summary>
    public const byte R_BEGIN_TURN_PICK_CARD = 10;

    /// <summary>
    /// Event raised by the master client to confirm the use of a card by a client.
    /// </summary>
    public const byte A_BEGIN_TURN_PICK_CARD = 11;

    /// <summary>
    /// Event raised by the master client to notify the current player.
    /// </summary>
    public const byte B_CURRENT_PLAYER = 12;

    /// <summary>
    /// Event raised by the master client to notify that a player hand has been updated.
    /// </summary>
    public const byte N_HAND_STATUS = 13;

    /// <summary>
    /// Event raised by a client to notify that he's trying to play a card.
    /// </summary>
    public const byte R_PLAY_CARD = 14;

    /// <summary>
    /// Event raised by the master client to notify that the request was successful.
    /// </summary>
    public const byte A_PLAY_CARD = 15;
}
