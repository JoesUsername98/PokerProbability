using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProbability
{
	public class Deck : Hand
	{
		public Deck(bool doShuffle = false)
		{
			//Generate Deck
			foreach (Card.VALUE Value in Enum.GetValues(typeof(Card.VALUE)))
			{
				foreach (Card.SUIT Suit in Enum.GetValues(typeof(Card.SUIT)))
				{
					MyHand.Add(new Card(MySuit: Suit, MyValue: Value));
				}
			}

			if(doShuffle)
			{
				Shuffle();
			}
		}

	}
}
