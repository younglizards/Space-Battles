using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class FakeClient : MonoBehaviour
{
    TurnController turnController;
    HandController handController;

    private void Start()
    {
        handController = FindObjectOfType<HandController>();
        turnController = FindObjectOfType<TurnController>();
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
        switch (eventcode)
        {
            case EventCodes.B_ALL_PLAYERS_READY:
                {
                    Assert.IsNull(content);
                    AllPlayersReady();
                }
                break;
            case EventCodes.B_FIRST_PLAYER:
                {
                    Assert.IsTrue(content.GetType() == typeof(int));
                    NotifyFirstPlayer((int)content);
                }
                break;
            case EventCodes.B_CURRENT_PLAYER:
                {
                    Assert.IsTrue(content.GetType() == typeof(int));
                    NotifyCurrentPlayer((int)content);
                }
                break;
            case EventCodes.A_BEGIN_TURN_DRAW_CARDS:
                {
                    Assert.IsTrue(content.GetType() == typeof(string));
                    Card[] cardsToChoose = MyJson.FromJson<Card>((string)content);

                    turnController.ShowCardSelector(cardsToChoose);
                }
                break;
            case EventCodes.A_FIRST_HAND:
                {
                    Assert.IsTrue(content.GetType() == typeof(string));
                    Card[] cardsToAdd = MyJson.FromJson<Card>((string)content);

                    handController.UpdatePlayerCards(cardsToAdd);
                }
                break;
            case EventCodes.N_HAND_STATUS:
                {
                    Assert.IsTrue(content.GetType() == typeof(int[]));
                    Assert.IsTrue(((int[])content).Length == 2);

                    int sender = ((int[])content)[0];
                    Assert.IsTrue(sender != PhotonNetwork.player.ID);

                    int totalCards = ((int[])content)[1];
                    handController.UpdateEnemyCards(totalCards);
                }
                break;
            case EventCodes.A_BEGIN_TURN_PICK_CARD:
                {
                    Assert.IsTrue(content.GetType() == typeof(string));
                    Card card = JsonUtility.FromJson<Card>((string)content);

                    turnController.CloseCardSelector();
                    handController.UpdatePlayerCards(new Card[] { card });
                }
                break;
            case EventCodes.A_PLAY_CARD:
                {
                    Assert.IsTrue(content.GetType() == typeof(object[]));
                    Assert.IsTrue(((object[])content).Length == 5);

                    int playerId = (int)((object[])content)[0];
                    int response = (int)((object[])content)[1];
                    string cardId = (string)((object[])content)[2];
                    int positionInHand = (int)((object[])content)[3];
                    int squarePosition = (int)((object[])content)[4];

                    ConfirmPlayCardFromHand(playerId, response, cardId, positionInHand, squarePosition);
                }
                break;
        }
    }

    void StartTurn()
    {
        turnController.StartTurn();
    }

    public void EndTurn()
    {
        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.MasterClient
        };

        PhotonNetwork.RaiseEvent(EventCodes.N_END_TURN, null, true, options);
    }

    void NotifyFirstPlayer(int playerId)
    {
        turnController.NotifyFirstPlayer(playerId);
    }

    void NotifyCurrentPlayer(int playerId)
    {
        turnController.NotifyCurrentPlayer(playerId);
        if (playerId == PhotonNetwork.player.ID)
        {
            StartTurn();
        }
    }

    void AllPlayersReady()
    {
        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.MasterClient
        };

        PhotonNetwork.RaiseEvent(EventCodes.R_FIRST_HAND, null, true, options);
    }

    public void RequestCards(int[] request)
    {
        Assert.IsTrue(request.Length == 2);

        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.MasterClient
        };

        // We pack the requesttype as one of the parameters
        PhotonNetwork.RaiseEvent(EventCodes.R_BEGIN_TURN_DRAW_CARDS, request, true, options);
    }

    public void PickCard(int cardIdx)
    {

        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.MasterClient
        };

        // We pack the requesttype as one of the parameters
        PhotonNetwork.RaiseEvent(EventCodes.R_BEGIN_TURN_PICK_CARD, cardIdx, true, options);
    }

    public void RequestPlayCardFromHand(string cardId, int cardPositionInHand, int squarePosition)
    {
        RaiseEventOptions options = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.MasterClient
        };

        object msgData = new object[] { cardId, cardPositionInHand, squarePosition };

        PhotonNetwork.RaiseEvent(EventCodes.R_PLAY_CARD, msgData, true, options);
    }

    void ConfirmPlayCardFromHand(int playerId, int response, string cardId, int cardPositionInHand, int squarePosition)
    {
        if (playerId == PhotonNetwork.player.ID)
        {
            handController.PlayPlayerCard(response, cardId, cardPositionInHand, squarePosition);
        }
        else
        {
            handController.PlayEnemyCard(cardId, squarePosition);
        }
    }

    void Log(string msg)
    {
        Debug.Log(msg);
    }
}
