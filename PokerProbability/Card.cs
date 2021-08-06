using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProbability
{
	public class Card 
	{
		public enum SUIT
		{
			Clubs = 1,
			Diamonds = 2,
			Hearts = 3,
			Spades = 4,
		}

		public enum VALUE
		{
			Two = 20,
			Three = 30,
			Four = 40,
			Five = 50,
			Six = 60,
			Seven = 70,
			Eight = 80,
			Nine = 90,
			Ten = 100,
			Jack = 110,
			Queen = 120,
			King = 130,
			Ace = 140
		}
		
		//properties
		public SUIT Suit { get; set; }
		public VALUE Value { get; set; }

		public Card(SUIT MySuit, VALUE MyValue)
		{
			Suit = MySuit;
			Value = MyValue;
		}

		public void Print() 
		{
			#region "switch"
			var SuitStr = Suit switch
			{
				SUIT.Clubs => "\u2663",
				SUIT.Spades => "\u2660",
				SUIT.Hearts => "\u2665",
				SUIT.Diamonds => "\u2666",
				_ => "error",
			};
			var ValueStr = Value switch
			{
				VALUE.Two => "2 ",
				VALUE.Three => "3 ",
				VALUE.Four => "4 ",
				VALUE.Five => "5 ",
				VALUE.Six => "6 ",
				VALUE.Seven => "7 ",
				VALUE.Eight => "8 ",
				VALUE.Nine => "9 ",
				VALUE.Ten => "10",
				VALUE.Jack => "J ",
				VALUE.Queen => "Q ",
				VALUE.King => "K ",
				VALUE.Ace => "A ",
				_ => "error",
			};
			Console.Write("|"+ValueStr+SuitStr+"|");
			#endregion
		}

		public int GetNumericVal()
		{
			return (int)Value + (int)Suit;
		}

		public static bool IsMoreThan(Card greater, Card lesser)
		{
			return (greater.GetNumericVal() > lesser.GetNumericVal());
		}
	}
}
