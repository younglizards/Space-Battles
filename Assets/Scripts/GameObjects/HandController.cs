using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Monobehaviour that receives Photon's callbacks, interacts with the Hand instance and the GUI Hands (both of them).
/// </summary>
public class HandController : MonoBehaviour
{
    public delegate void EnemyHandUpdatedDelegate(int totalAmmount);
    public EnemyHandUpdatedDelegate OnEnemyHandUpdated;

    public delegate void PlayerHandUpdatedDelegate(Card[] cardsToAdd);
    public PlayerHandUpdatedDelegate OnPlayerHandUpdated;

    public delegate void PlayerMustChooseCardsDelegate(Card[] cardsToPick);
    public PlayerMustChooseCardsDelegate OnCardChoiceGiven;

    public delegate void PlayerPlayedCardDelegate(int response, string cardId, int cardPositionInHand, int squarePosition);
    public PlayerPlayedCardDelegate OnPlayerPlayedCard;

    public delegate void EnemyPlayedCardDelegate(string cardId, int squarePosition);
    public EnemyPlayedCardDelegate OnEnemyPlayedCard;

    public void UpdateEnemyCards(int totalAmmount)
    {
        OnEnemyHandUpdated?.Invoke(totalAmmount);
    }

    public void UpdatePlayerCards(Card[] cardsToAdd)
    {
        OnPlayerHandUpdated?.Invoke(cardsToAdd);
    }

    public void PlayPlayerCard(int response, string cardId, int cardPositionInHand, int squarePosition)
    {
        OnPlayerPlayedCard?.Invoke(response, cardId, cardPositionInHand, squarePosition);
    }

    public void PlayEnemyCard(string cardId, int squarePosition)
    {
        OnEnemyPlayedCard?.Invoke(cardId, squarePosition);
    }
}
