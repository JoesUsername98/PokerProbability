using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProbability
{
	public class Player
	{

		public enum BLIND
		{
			Big,
			Small,
			None
		}
		public BLIND Blind { get; set; }
		public int Chips { get; set; }

		public Hand MyHand;
		public PokerGame.HANDTYPES MyHandType;
		public int PlayerID { get; set; }//Ensure Unique and assigned on construction.
		public double[] OutcomeChance { get; set; } 
		public Player(int ID)
		{
			MyHand = new Hand();
			//Ensure Unique and assigned on construction.
			PlayerID = ID;
			Chips = 5000;
		}

		public double WinChance (Hand ComCards)
		{
			//Some calculation here
			return 0.00;
		}
	}
}
