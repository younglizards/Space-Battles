using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Linq;


public class TurnController : ClientMonoBehaviour
{
    [SerializeField]
    Button btnEndTurn;

    DeckSelectorController deckSelector;
    CardSelectorController cardSelector;

    private void Start()
    {
        deckSelector = FindObjectOfType<DeckSelectorController>();
        cardSelector = FindObjectOfType<CardSelectorController>();
    }

    public void StartTurn()
    {
        // 1. Choose where to draw from
        ShowDeckSelector();

        // 2. Choose a card
        // 3. Do stuff
    }

    void ShowDeckSelector()
    {
        deckSelector.Open();
    }

    // Called by EndBtn
    void EndTurn()
    {
        Client.EndTurn();
        btnEndTurn.interactable = false;
    }

    public void ShowCardSelector(Card[] cards)
    {
        cardSelector.Open(cards);
    }

    public void CloseCardSelector()
    {
        cardSelector.Close();
    }

    public void NotifyFirstPlayer(int playerId)
    {
        btnEndTurn.interactable = PhotonNetwork.player.ID == playerId;
    }

    public void NotifyCurrentPlayer(int playerId)
    {
        btnEndTurn.interactable = PhotonNetwork.player.ID == playerId;
    }
}
