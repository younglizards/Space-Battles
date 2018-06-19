using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

public class CardsDatabase : MonoBehaviour
{
    static Dictionary<string, CardData> cardsById = new Dictionary<string, CardData>();
    public static CardBackData cardBackData;

    static CardsDatabase()
    {
        Build();
    }

    static void Build()
    {
        string dataAsJson = string.Empty;

        TextAsset[] cards = Resources.LoadAll<TextAsset>("Cards/Common");

        for (int i = 0; i < cards.Length; i++)
        {
            CardData data = JsonUtility.FromJson<CardData>(cards[i].text);
            cardsById[data.id] = data;
        }

        TextAsset cardBackAsset = Resources.Load<TextAsset>("CardBacks/card_back_01");
        dataAsJson = cardBackAsset.text;
        cardBackData = JsonUtility.FromJson<CardBackData>(dataAsJson);
    }

    public static bool GetById(string cardId, out CardData data)
    {
        if (cardsById.TryGetValue(cardId, out data))
        {
            return true;
        }
        Debug.LogWarning("Uknown card id " + cardId);
        return false;
    }

    public static string GetName(string cardId)
    {
        CardData data;
        if (cardsById.TryGetValue(cardId, out data))
        {
            return data.name;
        }
        Debug.LogWarning("Uknown card id " + cardId);
        return string.Empty;
    }
}
