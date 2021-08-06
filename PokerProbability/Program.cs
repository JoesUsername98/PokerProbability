using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerProbability
{
	class Program
	{
		static void Main(string[] args)
		{
			var MyGame = new PokerGame(1);

			/*
			PokerGame TestGame = new PokerGame(0);
			
			Hand Personal = new Hand();
			Personal.MyHand.Add(new Card(MySuit: Card.SUIT.Spades, MyValue: Card.VALUE.Two));
			Personal.MyHand.Add(new Card(MySuit: Card.SUIT.Clubs, MyValue: Card.VALUE.Ace));
			Personal.Print(); Console.WriteLine();

			Hand Community = new Hand();
			Community.MyHand.Add(new Card(MySuit: Card.SUIT.Hearts, MyValue: Card.VALUE.Eight));
			Community.MyHand.Add(new Card(MySuit: Card.SUIT.Hearts, MyValue: Card.VALUE.Queen));
			Community.MyHand.Add(new Card(MySuit: Card.SUIT.Diamonds, MyValue: Card.VALUE.King));
			//Community.MyHand.Add(new Card(MySuit: Card.SUIT.Clubs, MyValue: Card.VALUE.Two));
			//Community.MyHand.Add(new Card(MySuit: Card.SUIT.Spades, MyValue: Card.VALUE.Six));
			Community.Print(); Console.WriteLine();

			double[] Outcomes = TestGame.CalculateOutcome(Personal, Community);
			double SumOuts = Outcomes.Sum();
			int i = 0;
			foreach (PokerGame.HANDTYPES Hands in Enum.GetValues(typeof(PokerGame.HANDTYPES)))
			{
				Outcomes[i] = Math.Round((Outcomes[i] / SumOuts) * 100,3);
				Console.Write(String.Format("{0,13} : ", Enum.Format(typeof(PokerGame.HANDTYPES), Hands, "g")));
				Console.WriteLine(String.Format("{0:00.00} % ", Outcomes[i]));
				i++;
			}
			*/
		}
	}
}