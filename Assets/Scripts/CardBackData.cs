using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class CardBackData
{
    public string id;

    [NonSerialized]
    Sprite sprite;
    public Sprite Sprite
    {
        get
        {
            if (sprite == null) { sprite = Resources.Load<Sprite>("Art/Cards/" + id); }
            return sprite;
        }
    }
}
