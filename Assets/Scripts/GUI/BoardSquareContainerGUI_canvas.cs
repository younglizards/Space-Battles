using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquareContainerGUI_canvas : MonoBehaviour
{
    public BoardSquareGUI_canvas slotPrefab;
    Transform myTransform;

    SideGUI_canvas side;

    private void Start()
    {
        side = GetComponentInParent<SideGUI_canvas>();

        myTransform = transform;

        myTransform.DestroyAllChildren();

        for (int i = 0; i < GameConstants.NUMBER_OF_BOARD_SLOTS_PER_PLAYER; i++)
        {
            BoardSquareGUI_canvas slotGUI = Instantiate(slotPrefab, myTransform);
            slotGUI.side = side;
            slotGUI.gameObject.name = "slot_" + i;
        }

        // Callbacks
        HandController hand = FindObjectOfType<HandController>();

        if (side.owner == Owner.PLAYER)
        {
            hand.OnPlayerPlayedCard += PlayerPlayedCard;
        }
        else if (side.owner == Owner.ENEMY)
        {
            hand.OnEnemyPlayedCard += EnemyPlayedCard;
        }
        else
        {
            Debug.LogError("Who are you?");
        }
    }

    BoardSquareGUI_canvas GetSquareAtPosition(int squarePosition)
    {
        Transform c = myTransform.GetChild(squarePosition);
        return c.GetComponent<BoardSquareGUI_canvas>();
    }

    void PlayerPlayedCard(int response, string cardId, int cardPositionInHand, int squarePosition)
    {
        if (response == Responses.CARD_PLAYED_FAILED)
        {
            return;
        }

        BoardSquareGUI_canvas square = GetSquareAtPosition(squarePosition);
        square.TakeUp(cardId);
    }

    void EnemyPlayedCard(string cardId, int squarePosition)
    {
        // mirrored!
        squarePosition = transform.childCount - 1 - squarePosition;
        BoardSquareGUI_canvas square = GetSquareAtPosition(squarePosition);
        square.TakeUp(cardId);
    }
}
