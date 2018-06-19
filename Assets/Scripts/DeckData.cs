using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Holds all the information to build a deck. 
/// Specifically contains the cards and the number of repetitions of each of them.
/// </summary>
[Serializable]
public class DeckData
{
    public string id;
    public string name;
    public List<CardStack> cardStacks;
}

/// <summary>
/// Holds the info of each card stack, meaning the card id and the number of repetitions.
/// </summary>
[Serializable]
public class CardStack
{
    public string id;
    public int ammount;
}