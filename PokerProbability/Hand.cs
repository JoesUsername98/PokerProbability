using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProbability
{
	
	public class Hand
	{
		private List<Card> _myHand = new();

		public List<Card> MyHand { get => _myHand; set => _myHand = value; }

		public void Shuffle()
		//Shuffles deck using Fisher-Yates
		{
			Random rng = new Random();

			int n = _myHand.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				Card value = _myHand[k];
				_myHand[k] = _myHand[n];
				_myHand[n] = value;
			}

		}

		public void Print()
		// Prints the deck
		{
			foreach (Card c in _myHand)
			{
				c.Print();
			}
		}

		public Card Deque() 
		{
			var movingCard = _myHand[_myHand.Count - 1];
			MyHand.RemoveAt(_myHand.Count - 1);
			return movingCard;
		}

		public void RemoveCard(Card.SUIT SuitToRemove, Card.VALUE ValueToRemove)
		{
			for (int i = 0; i < CardNo(); i++)
			{
				if (MyHand[i].Suit == SuitToRemove && MyHand[i].Value == ValueToRemove) 
				{
					MyHand.RemoveAt(i);
					return;
				}
			}
		}

		public void Enqueue(Card toAdd)
		{
			_myHand.Insert(0, toAdd);
		}
		public void Push(Card toAdd)
		{
			_myHand.Add(toAdd);
		}

		public int CardNo()
		{
			return _myHand.Count;
		}

		public void SortByValue()
		{
			_myHand.Sort((x, y) => x.GetNumericVal().CompareTo(y.GetNumericVal()));
		}

		public void SortBySuit()
		{
			_myHand.Sort((x, y) => (x.GetNumericVal()%10).CompareTo((y.GetNumericVal()%10)));
		}
	}
}
