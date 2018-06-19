using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardSquare
{
    public int position;
    public Card Card { get; private set; }
    public bool IsEmpty { get { return Card == null; } }

    public BoardSquare(int position)
    {
        this.position = position;
    }

    public void TakeUp(Card card)
    {
        Card = card;
    }
}
