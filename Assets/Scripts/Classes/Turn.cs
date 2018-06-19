using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class Turn
{

    public int FirstPlayerId { get { return data.firstPlayerId; } }
    public int CurrentPlayerId { get { return data.currentPlayerId; } }

    readonly Match match;
    readonly TurnData data;

    Dictionary<int, bool> firstTurnPlayed = new Dictionary<int, bool>();

    public Turn(Match match)
    {
        this.match = match;

        data = new TurnData();

        firstTurnPlayed[match.player1.id] = false;
        firstTurnPlayed[match.player2.id] = false;

        PickFirstPlayer();
    }

    void PickFirstPlayer()
    {
#if UNITY_EDITOR
        int firstPlayerId = match.player1.id;
#else
        int firstPlayerId = match.players[Rand.Range(0, GameConstants.NUMBER_OF_PLAYERS)].id;
#endif

        data.firstPlayerId = firstPlayerId;
        StartTurn(firstPlayerId);
    }

    void StartTurn(int playerId)
    {
        data.phase = TurnPhase.BEGINNING;
        data.currentPlayerId = playerId;
        data.count++;
    }

    public void EndTurn()
    {
        data.phase = TurnPhase.ENDING;

        // If this was the first turn we update the register at the end of the turn.
        if (IsFirstTurn(data.currentPlayerId))
        {
            firstTurnPlayed[data.currentPlayerId] = true;
        }

        data.Clean();

        StartTurn(match.GetOppositePlayer(data.currentPlayerId).id);
    }

    public bool IsFirstTurn(int playerId)
    {
        return firstTurnPlayed[playerId];
    }

    public void ChoosingCard(List<Card> cards)
    {
        data.phase = TurnPhase.CHOOSING_CARD;
        data.Put("cards", cards);
    }

    public List<Card> GetChooseableCards()
    {
        Assert.IsTrue(data.phase == TurnPhase.CHOOSING_CARD);
        return (List<Card>)data.Get("cards");
    }
}

class TurnData
{
    public int count;
    public int firstPlayerId;
    public int currentPlayerId;
    public TurnPhase phase;

    Dictionary<string, object> parameters = new Dictionary<string, object>();

    public void Put(string paramKey, object value)
    {
        parameters[paramKey] = value;
    }

    public object Get(string paramKey)
    {
        return parameters[paramKey];
    }

    public void Clean()
    {
        phase = TurnPhase.UNDEFINDED;
        parameters.Clear();
    }
}

enum TurnPhase { UNDEFINDED = -1, BEGINNING, CHOOSING_DECK, CHOOSING_CARD, ENDING };