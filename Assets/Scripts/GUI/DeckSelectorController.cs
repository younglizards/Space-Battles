using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSelectorController : ClientMonoBehaviour
{
    public GameObject drawCardPanel;

    void Start()
    {
        drawCardPanel.SetActive(false);
    }

    public void Open()
    {
        drawCardPanel.SetActive(true);
    }

   void Close()
    {
        drawCardPanel.SetActive(false);
    }

    public void Request2xCommonCards()
    {
        Client.RequestCards(new int[] { 2, 0 });
        Close();
    }

    public void Request2xSpaceshipCards()
    {
        Client.RequestCards(new int[] { 0, 2 });
        Close();
    }

    public void Request1Common1SpaceshipCards()
    {
        Client.RequestCards(new int[] { 1, 1 });
        Close();
    }
}
