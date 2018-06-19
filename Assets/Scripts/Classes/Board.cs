using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Board
{
    Dictionary<int, List<BoardSquare>> boardSquares;

    readonly Match match;

    public Board(Match match)
    {
        this.match = match;
        boardSquares = new Dictionary<int, List<BoardSquare>>
        {
            [match.player1.id] = new List<BoardSquare>(),
            [match.player2.id] = new List<BoardSquare>()
        };

        for (int i = 0; i < GameConstants.NUMBER_OF_BOARD_SLOTS_PER_PLAYER; i++)
        {
            BoardSquare b1 = new BoardSquare(i);
            boardSquares[this.match.player1.id].Add(b1);

            BoardSquare b2 = new BoardSquare(i);
            boardSquares[this.match.player2.id].Add(b2);
        }
    }

    public bool IsSquareEmpty(int playerId, int squarePosition)
    {
        return boardSquares[playerId][squarePosition].IsEmpty;
    }

    public void PlayCard(int playerId, Card card, int squarePosition)
    {
        boardSquares[playerId][squarePosition].TakeUp(card);
    }
}
