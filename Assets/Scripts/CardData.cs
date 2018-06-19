using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Card's base data. Cannot be modified. Multipliers & stuff like that should be aplied to the card object.
/// </summary>
[Serializable]
public class CardData
{
    public string id;
    public string name;
    public int cost;
    public int damage;
    public int shipSlots;
    public int unitSlots;
    public int shields;
    public string text;
    public string flavorText;
    public string category;

    [NonSerialized]
    Sprite sprite;

    public Sprite Sprite
    {
        get
        {
            if (sprite == null) { sprite = Resources.Load<Sprite>("art/cards/" + id); }
            return sprite;
        }
    }
}
