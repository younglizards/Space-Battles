using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BoardSquareGUI_canvas : MonoBehaviour
{
    public SideGUI_canvas side;

    public Owner Owner { get { return side.owner; } }

    Transform myTransform;

    private void Start()
    {
        myTransform = transform;    
    }

    public void TakeUp(string cardId)
    {
        CardGUI_canvas cardGUIPrefab = Prefabs.Get("cardGUI_canvas").GetComponent<CardGUI_canvas>();

        CardGUI_canvas cardCpy = Instantiate(cardGUIPrefab, myTransform);
        cardCpy.CardId = cardId;
        cardCpy.Location = CardLocation.GAME;
    }

    public int GetPositionInParent()
    {
        return myTransform.GetPositionInParent();
    }
}
