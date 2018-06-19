using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains a bunch of static method to check for valid actions.
/// </summary>
public class Rules
{
    /// <summary>
    /// Checks if a card is a unit card, I.E. It can be played in an empty slot.
    /// </summary>
    /// <param name="cardId">Id to check.</param>
    /// <returns>True if this card can be played as a standalone unit.</returns>
    public static bool IsUnitCard(string cardId)
    {
        CardData card;
        if (CardsDatabase.GetById(cardId, out card))
        {
            return card.category == CardCategories.SPACESHIP;
        }
        return false;
    }
}
