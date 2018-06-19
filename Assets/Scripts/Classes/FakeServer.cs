using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

class FakeServer : MonoBehaviour
{
    Match match;
    int numberOfPlayersReady = 0;

    private void Awake()
    {
        Assert.IsTrue(PhotonNetwork.isMasterClient);

        PhotonPlayer[] players = PhotonNetwork.playerList;
        Assert.IsTrue(players.Length == 2);

        match = new Match(players[0].ID, players[1].ID);
    }

    private void OnEnable()
    {
        PhotonNetwork.OnEventCall += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.OnEventCall -= OnEvent;
    }

    void OnEvent(byte eventcode, object content, int senderid)
    {
        Assert.IsTrue(PhotonNetwork.isMasterClient);
        switch (eventcode)
        {
            case EventCodes.N_PLAYER_READY:
                {
                    Assert.IsNull(content);
                    PlayerReady(senderid);
                }
                break;
            case EventCodes.R_FIRST_HAND:
                {
                    Assert.IsNull(content);
                    RequestFirstHand(senderid);
                }
                break;
            case EventCodes.N_END_TURN:
                {
                    Assert.IsNull(content);
                    EndTurn(senderid);
                }
                break;
            case EventCodes.R_BEGIN_TURN_DRAW_CARDS:
                {
                    Assert.IsTrue(content.GetType() == typeof(int[]));
                    Assert.IsTrue(((int[])content).Length == 2);
                    BeginTurnDrawCards(senderid, (int[])content);
                }
                break;
            case EventCodes.R_BEGIN_TURN_PICK_CARD:
                {
                    Assert.IsTrue(content.GetType() == typeof(int));
                    BeginTurnPickCard(senderid, (int)content);
                }
                break;
            case EventCodes.R_PLAY_CARD:
                {
                    Assert.IsTrue(content.GetType() == typeof(object[]));
                    Assert.IsTrue(((object[])content).Length == 3);

                    string cardId = (string)((object[])content)[0];
                    int positionInHand = (int)((object[])content)[1];
                    int squarePosition = (int)((object[])content)[2];

                    RequestPlayCard(senderid, cardId, positionInHand, squarePosition);
                }
                break;
        }
    }

    void PlayerReady(int playerId)
    {
        numberOfPlayersReady++;
        Log("Player " + playerId + " ready");
        if (numberOfPlayersReady == GameConstants.NUMBER_OF_PLAYERS)
        {
            // Both players are ready to play
            NotifyAllPlayersReady();
            NotifyFirstPlayer();
        }
    }

    void NotifyAllPlayersReady()
    {
        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };

        Log("All players ready");
        PhotonNetwork.RaiseEvent(EventCodes.B_ALL_PLAYERS_READY, null, true, options);
    }

    void NotifyFirstPlayer()
    {
        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All
        };

        Log("First player = " + match.turn.FirstPlayerId);
        PhotonNetwork.RaiseEvent(EventCodes.B_FIRST_PLAYER, match.turn.FirstPlayerId, true, options);
    }

    void RequestFirstHand(int senderid)
    {
        Log("Player " + senderid + " requesting first hand");

        Player player = match.GetPlayer(senderid);

        List<Card> cards = match.RequestFirstHand(player.id);

        RaiseEventOptions options = new RaiseEventOptions
        {
            TargetActors = new int[] { senderid }
        };

        PhotonNetwork.RaiseEvent(EventCodes.A_FIRST_HAND, MyJson.ToJson(cards), true, options);

        NotifyHandStatus(senderid);
    }

    void NotifyHandStatus(int senderid)
    {
        Player player = match.GetPlayer(senderid);
        Player other = match.GetOppositePlayer(senderid);

        RaiseEventOptions options = new RaiseEventOptions
        {
            TargetActors = new int[] { other.id }
        };

        Log("Player " + senderid + " notifying hand status to " + other.id);
        PhotonNetwork.RaiseEvent(EventCodes.N_HAND_STATUS, new int[] { senderid, player.hand.CardCount }, true, options);
    }

    void EndTurn(int playerId)
    {
        if (match.turn.CurrentPlayerId == playerId)
        {
            Log("Player " + playerId + " ending turn");

            match.turn.EndTurn();

            RaiseEventOptions options = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(EventCodes.B_CURRENT_PLAYER, match.turn.CurrentPlayerId, true, options);
        }
    }

    void BeginTurnDrawCards(int playerId, int[] request)
    {
        if (match.turn.CurrentPlayerId == playerId)
        {
            Log("Player " + playerId + " draws");

            int commonCards = request[0];
            int spaceshipCards = request[1];

            List<Card> cards = match.StartTurnRequestCards(playerId, commonCards, spaceshipCards);

            RaiseEventOptions options = new RaiseEventOptions
            {
                TargetActors = new int[] { playerId }
            };

            PhotonNetwork.RaiseEvent(EventCodes.A_BEGIN_TURN_DRAW_CARDS, MyJson.ToJson(cards), true, options);

        }
    }

    void BeginTurnPickCard(int playerId, int cardIdx)
    {
        Card card = match.PickCard(playerId, cardIdx);
        if (card != null)
        {
            Log("Player " + playerId + " picked card " + card.CardName);

            RaiseEventOptions options = new RaiseEventOptions
            {
                TargetActors = new int[] { playerId }
            };

            PhotonNetwork.RaiseEvent(EventCodes.A_BEGIN_TURN_PICK_CARD, JsonUtility.ToJson(card), true, options);
            NotifyHandStatus(playerId);
        }
    }

    void RequestPlayCard(int playerId, string cardId, int positionInHand, int squarePosition)
    {


        if (match.PlayCard(playerId, cardId, positionInHand, squarePosition))
        {
            Log("Player " + playerId + " played card " + CardsDatabase.GetName(cardId));

            object msgData = new object[] { playerId, Responses.CARD_PLAYED_SUCCESSFULLY, cardId, positionInHand, squarePosition };

            RaiseEventOptions options = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(EventCodes.A_PLAY_CARD, msgData, true, options);

            NotifyHandStatus(playerId);
        }
        else
        {
            Log("Player " + playerId + " tried to play card " + CardsDatabase.GetName(cardId) + ", but couldn't");

            RaiseEventOptions options = new RaiseEventOptions
            {
                TargetActors = new int[] { playerId }
            };

            object msgData = new object[] { playerId, Responses.CARD_PLAYED_FAILED, cardId, positionInHand, squarePosition };

            PhotonNetwork.RaiseEvent(EventCodes.A_PLAY_CARD, msgData, true, options);
        }
    }

    void Log(string msg)
    {
        Debug.Log("<color=yellow>" + msg + "</color>");
    }
}