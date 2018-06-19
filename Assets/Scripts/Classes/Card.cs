
using ExitGames.Client.Photon;
using System;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

[Serializable]
public class Card
{
    [SerializeField]
    CardData data;

    public Card(CardData data)
    {
        this.data = data;
    }

    public string Id { get { return data.id; } }
    public int Cost { get { return data.cost; } }
    public string FlavorText { get { return data.flavorText; } }
    public string CardName { get { return data.name; } }
    public int Damage { get { return data.damage; } }
    public string Category { get { return data.category; } }
    public int Shields { get { return data.shields; } }
    public int ShipSlots { get { return data.shipSlots; } }
    public int UnitSlots { get { return data.unitSlots; } }
    public string Text { get { return data.text; } }

    [NonSerialized]
    Sprite sprite;

    public Sprite Sprite
    {
        get
        {
            if (sprite == null) { sprite = Resources.Load<Sprite>("art/cards/" + data.id); }
            return sprite;
        }
    }
}
