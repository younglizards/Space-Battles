using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectorController : ClientMonoBehaviour
{
    public GameObject chooseCardPanel;
    public Transform chooseCardTransform;

    void Start()
    {
        chooseCardPanel.SetActive(false);
    }

    public void Open(Card[] cards)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Transform cardGUITransf = chooseCardTransform.GetChild(i);
            CardGUI_canvas cardGUI = cardGUITransf.GetComponent<CardGUI_canvas>();
            cardGUI.CardId = cards[i].Id;
        }

        chooseCardPanel.SetActive(true);
    }

    public void Close()
    {
        chooseCardPanel.SetActive(false);
    }

    public void PickCard(int card)
    {
        Client.PickCard(card);
    }
}
