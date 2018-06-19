using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckGUI : MonoBehaviour
{
    // This isn't associated to a real card - it'll be visible if cards > 0 and viceversa.
    [SerializeField]
    GameObject firstCard;

    void Start()
    {
        // deck = FindObjectOfType<DeckObject>();
        // Both the master and the non-master client will receive this callback
        // deck.OnRemainingCardsChanged += OnDeckCardsChanged;
    }

    void OnDeckCardsChanged(int[] cardsAmmount)
    {
    }
}
