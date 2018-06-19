using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class CardGUI_canvas : ClientMonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Name")]
    public GameObject namePanel;
    public TextMeshProUGUI nameTextMesh;

    [Header("Shields")]
    public GameObject shieldsPanel;

    [Header("Art")]
    public Image cardArt;

    [Header("Cost")]
    public GameObject costPanel;
    public TextMeshProUGUI costTextMesh;

    [Header("Text")]
    public GameObject textPanel;
    public TextMeshProUGUI textTextMesh;

    [Header("Slots (unit)")]
    public GameObject slotsUnitPanel;
    public TextMeshProUGUI slotsUnitTextMesh;

    [Header("Slots (spaceship)")]
    public GameObject slotSpaceshipPanel;

    CardGUI_canvas detailCard;

    RectTransform rectTransform;

    CardData card;
    string cardId;
    public string CardId
    {
        get { return cardId; }
        set
        {
            cardId = value;
            card = null;
            CardsDatabase.GetById(cardId, out card);
            UpdateGUI();
        }
    }
    public CardLocation Location
    {
        get { return location; }
        set
        {
            location = value;
            UpdateGUI();
        }
    }

    [SerializeField]
    CardLocation location;

    HandGUI_canvas hand;

    protected bool dragging = false;

    Canvas canvas;
    RectTransform rtCanvas;

    GraphicRaycaster raycaster;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Weird hack to get the disabled object at runtime
        GameObject detail = GameObject.FindGameObjectWithTag("DetailCardWrapper");
        detailCard = detail.transform.GetChild(0).GetComponent<CardGUI_canvas>();

        hand = transform.GetComponentInParent<HandGUI_canvas>();

        canvas = GetComponentInParent<Canvas>();
        rtCanvas = canvas.GetComponent<RectTransform>();

        raycaster = canvas.GetComponent<GraphicRaycaster>();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (location == CardLocation.PLAYER_HAND || location == CardLocation.GAME)
        {
            ShowDetail();
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        HideDetail();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (location != CardLocation.PLAYER_HAND)
        {
            return;
        }

        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            rtCanvas,
            eventData.position,
            eventData.pressEventCamera,
            out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (location == CardLocation.PLAYER_HAND)
        {
            // myTransform.SetParent(canvas.transform, true);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (location == CardLocation.ENEMY_HAND)
        {
            return;
        }

        TryToPlayCard(eventData);
        HideDetail();
    }

    void TryToPlayCard(PointerEventData eventData)
    {
        RaycastResult raycastResult = eventData.pointerCurrentRaycast;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(eventData, results);

        bool validCardPosition = false;

        for (int i = 0; i < results.Count; i++)
        {
            BoardSquareGUI_canvas square = results[i].gameObject.GetComponent<BoardSquareGUI_canvas>();
            if (square && square.Owner == Owner.PLAYER)
            {
                // Ask for permission
                validCardPosition = true;
                RequestPlayCard(square.GetPositionInParent());
                break;
            }
        }

        if (!validCardPosition)
        {
            hand.RebuildLayout();
        }
    }

    void RequestPlayCard(int squarePosition)
    {
        Client.RequestPlayCardFromHand(cardId, transform.GetPositionInParent(), squarePosition);
    }

    void ShowDetail()
    {
        Assert.IsTrue(location == CardLocation.PLAYER_HAND || location == CardLocation.GAME);

        detailCard.CardId = cardId;
        detailCard.gameObject.SetActive(true);
    }

    void HideDetail()
    {
        detailCard.gameObject.SetActive(false);
    }

    void UpdateGUI()
    {
        costPanel.SetActive(false);
        namePanel.SetActive(false);
        textPanel.SetActive(false);
        shieldsPanel.SetActive(false);
        slotSpaceshipPanel.SetActive(false);
        slotsUnitPanel.SetActive(false);

        if (location == CardLocation.ENEMY_HAND)
        {
            cardArt.sprite = CardsDatabase.cardBackData.Sprite;
        }
        else if (location == CardLocation.PLAYER_HAND || location == CardLocation.DETAIL || location == CardLocation.GAME ||
            location == CardLocation.CHOOSING)
        {
            costPanel.SetActive(true);
            namePanel.SetActive(true);
            textPanel.SetActive(true);

            if (card != null)
            {
                nameTextMesh.text = card.name;
                cardArt.sprite = card.Sprite;
                costTextMesh.text = card.cost.ToString();
                textTextMesh.text = card.text;

                if (card.category == CardCategories.SPACESHIP)
                {
                    RenderShields(card.shields);
                    RenderShipSlots(card.shipSlots);
                }
                else if (card.category == CardCategories.CREW)
                {
                    RenderUnitSlots(card.unitSlots);
                }
            }
        }
    }

    void RenderUnitSlots(int slots)
    {
        slotsUnitPanel.SetActive(true);
        slotsUnitTextMesh.text = slots.ToString();
    }

    void RenderShields(int shields)
    {
        shieldsPanel.SetActive(true);

        Transform shieldTransf = shieldsPanel.transform;

        shieldTransf.DestroyAllChildren();

        GameObject shieldGUIPrefab = Prefabs.Get("ShieldGUI_canvas");
        for (int i = 0; i < shields; i++)
        {
            Instantiate(shieldGUIPrefab, shieldTransf);
        }
    }

    void RenderShipSlots(int slots)
    {
        slotSpaceshipPanel.SetActive(true);

        Transform slotsTransf = slotSpaceshipPanel.transform;

        slotsTransf.DestroyAllChildren();

        GameObject slotGUIPrefab = Prefabs.Get("SlotGUI_canvas");
        for (int i = 0; i < slots; i++)
        {
            Instantiate(slotGUIPrefab, slotsTransf);
        }
    }
}
