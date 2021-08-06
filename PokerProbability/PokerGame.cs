using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerProbability
{
	/*
	 * A burn pile?
	 * A container of bets (and from what player)?
	 * Logic to calculate likelihood of winning [and from different player perspectives of knowledge]
	 */
	public class PokerGame
	{
		protected int NoOfPlayers = 1;
		public List<Player> PlayerList = new List<Player>();
		public Deck CardDeck = new Deck(doShuffle: true);
		public Hand CommunityCards = new Hand();

		public enum HANDTYPES
		{
			HighCard,
			Pair,
			TwoPair,
			ThreeOfAKind,
			Straight,
			Flush,
			FullHouse,
			FourOfAKind,
			StraightFlush,
			RoyalFlush
		}

		public PokerGame(int Players)
		{
			if (Players > 0)
			{
				StartGame(Players);
			}
		}
		public void StartGame(int Players)
		{
			//create players 
			for (int i = 0; i < Players; i++)
			{
				PlayerList.Add(new Player(NoOfPlayers));
				NoOfPlayers++;
			}

			#region "Game sequence"
			int[] CommCardsToDeal = { 0, 3, 1, 1 };
			string[] RoundMsg = { "Bet to see flop", "Bet to see turn", "Bet to see river", "Bet to win!" };

			//Deal out 2 personal cards for each player
			for (int i = 0; i < 2; i++)
			{
				foreach (Player p in PlayerList)
				{
					MoveCard(from: CardDeck, to: p.MyHand);
				}
			}
			//For each round of game
			for (int j = 0; j < CommCardsToDeal.Length; j++)
			{
				//for each card per round
				for (int i = 0; i < CommCardsToDeal[j]; i++)
				{
					MoveCard(from: CardDeck, to: CommunityCards);
				}
				//Calculate Handtype
				foreach (Player p in PlayerList)
				{
					p.MyHandType = FindBestHand(p.MyHand, CommunityCards);
					if (CommunityCards.CardNo() >= 3)
					{
						p.OutcomeChance = CalculateOutcome(p.MyHand, CommunityCards);
					}
				}
				PrintTable();
				Console.WriteLine(RoundMsg[j]);
			}
			//Decide winner.
			var Winners = new List<int> { 0 };
			for (int i = 1; i < PlayerList.Count; i++)
			{
				if (PlayerList[i].MyHandType > PlayerList[Winners[0]].MyHandType)
				{//if new player has a better hand than old player.
					Winners.Clear();
					Winners.Add(i);
				}
				else if (PlayerList[i].MyHandType == PlayerList[Winners[0]].MyHandType)
				{
					Winners.Add(i);
				}
			}
			//Win Message
			Console.Write("Player ");
			foreach (int winner in Winners)
			{
				Console.Write((winner + 1) + ", ");
			}
			Console.WriteLine("Wins!");
			#endregion

		}

		void MoveCard(Hand from, Hand to)
		{//Moves card from 'from' hand to 'to' hand.
			to.Push(from.Deque());
		}
		public void PrintTable()
		{
			Console.Write("Community Cards:");
			CommunityCards.Print();
			Console.WriteLine(); //Console.WriteLine();

			foreach (Player p in PlayerList)
			{
				Console.Write("Player{0}'s Hand:", p.PlayerID);

				p.MyHand.Print();
				if (p.MyHand.CardNo() == 2)
				{
					Console.WriteLine();
					Console.WriteLine("Current Hand Type: " + p.MyHandType);
				}

				if (CommunityCards.CardNo() >= 3)
				{
					int i = 0;
					foreach (HANDTYPES Hands in Enum.GetValues(typeof(HANDTYPES)))
					{
						double chance = Math.Round((p.OutcomeChance[i] / p.OutcomeChance.Sum()) * 100, 2);
						Console.Write(String.Format("{0,13} : ", Enum.Format(typeof(HANDTYPES), Hands, "g")));
						Console.WriteLine(String.Format("{0:00.0} % ", chance));
						i++;
					}
				}
				Console.WriteLine();
			}

		}
		public HANDTYPES FindBestHand(Hand Personal, Hand Community)
		{
			List<int[]> CommunityCombos = new List<int[]>();
			//Check Personal Hand is Length 2, Community between 3 and 5
			if (Personal.CardNo() + Community.CardNo() < 5)
			{
				if (Personal.MyHand[0].Value == Personal.MyHand[1].Value)
				{
					return HANDTYPES.Pair;
				}
				else
				{
					return HANDTYPES.HighCard;
				}
			}
			if (Community.CardNo() == 0)
			{
				CommunityCombos.Add(new int[] { 5, 6 });
			}

			//Generate Possible combinations of community and personal cards
			#region "Combination Generation" 

			var DeckDistributions = new List<int[]>();
			if (Community.CardNo() >= 3)
			{
				DeckDistributions.Add(new int[] { 2, 3 });
			}
			if (Community.CardNo() >= 4)
			{
				DeckDistributions.Add(new int[] { 1, 4 });// 1 x personal,  4 x Community
			}

			HANDTYPES BestHandType = HANDTYPES.HighCard;
			Hand BestHand = new Hand();

			foreach (var DeckDist in DeckDistributions)
			{
				//[1x Personal Hand, 4x Community]
				var PersonalPool = ComboGen(Pool: Personal.CardNo(), Draw: DeckDist[0]);
				var CommunityPool = ComboGen(Pool: Community.CardNo(), Draw: DeckDist[1]);
				#endregion

				#region "Hand Generation" 
				for (int i = 0; i < DeckDist[0]; i++)
				{
					foreach (int[] PersonalAllocation in PersonalPool) // For each combo of Personal Cards
					{
						foreach (int[] CommunalAllocation in CommunityPool) // For each combo of Community Cards 
						{
							//Creates dummy hand
							Hand PossCombo = new Hand();

							foreach (int iP in PersonalAllocation) //Add each card to the hand
							{
								PossCombo.Enqueue(new Card(MySuit: Personal.MyHand[iP].Suit,
																					 MyValue: Personal.MyHand[iP].Value));
							}
							foreach (int iC in CommunalAllocation) //Add each card to the hand
							{
								PossCombo.Enqueue(new Card(MySuit: Community.MyHand[iC].Suit,
																					 MyValue: Community.MyHand[iC].Value));
							}
							#endregion
							#region "Hand Detection Logic"
							var ThisHandType = DetermineHand(PossCombo);
							if (ThisHandType > BestHandType)
							{
								BestHandType = ThisHandType; BestHand = PossCombo;
							}
							#endregion
						}
					}
				}
			}
			return BestHandType; // Can Return BestHand If Required.
		}
		public bool IsFlush(Hand h)
		{// Ensures that all cards are the same suit. Returns true if the case.

			for (int i = 1; i < h.CardNo(); i++)
			{
				if (h.MyHand[i - 1].Suit != h.MyHand[i].Suit)
				{
					return false;
				}
			}
			return true;
		}
		public bool IsStraight(Hand h)
		{// Ensures that all cards are consectutive in value. Returns true if the case.
		 //Must Sort First
			h.SortByValue();
			for (int i = 1; i < h.CardNo(); i++)
			{
				if ((int)h.MyHand[i].Value - (int)h.MyHand[i - 1].Value != 10)
				{
					return false;
				}
			}
			return true;
		}
		public Dictionary<Card.VALUE, int> FindRepeats(Hand h)
		{
			Dictionary<Card.VALUE, int> PairsReport = new Dictionary<Card.VALUE, int>();
			foreach (Card c in h.MyHand)
			{
				if (!PairsReport.ContainsKey(c.Value))
				{
					PairsReport.Add(c.Value, 1);
				}
				else
				{
					PairsReport[c.Value] += 1;
				}
			}
			return PairsReport;
		}
		public double[] CalculateOutcome(Hand Personal, Hand Community)
		{
			/*
			 * Probability Calculator
			 */

			var Outcomes = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //Count of Outcome

			if (Community.CardNo() < 5)
			{

				var CardsLeftInDeck = new Deck();//Generate Hand of unknown cards 
				var PossibleHands = new List<Hand>(); // List of possible hands hands

				//Remove known cards
				foreach (Card c in Personal.MyHand)
				{
					CardsLeftInDeck.RemoveCard(c.Suit, c.Value);
				}
				foreach (Card c in Community.MyHand)
				{
					CardsLeftInDeck.RemoveCard(c.Suit, c.Value);
				}
				var UnknownPool = ComboGen(Pool: CardsLeftInDeck.CardNo(), Draw: (7 - Personal.CardNo() - Community.CardNo()));

				foreach (int[] UnknownAllocation in UnknownPool)
				{
					#region "Hand Generation" 
					Hand PossCombo = new Hand();
					foreach (var c in Community.MyHand) //Add each card to the hand
					{
						PossCombo.Enqueue(c);
					}
					foreach (int iU in UnknownAllocation) //Add each card to the hand
					{
						PossCombo.Enqueue(new Card(MySuit: CardsLeftInDeck.MyHand[iU].Suit,
																			 MyValue: CardsLeftInDeck.MyHand[iU].Value));
					}
					#endregion
					#region "Hand Detection Logic"
					var BestHandType = FindBestHand(Personal: Personal, Community: PossCombo);
					#endregion
					//Increment the outcome of this one hand.
					OutcomeCounter(Input: BestHandType, Outcomes);
				}
			}
			else if (Community.CardNo() == 5)// If we know all the cards then 100% probability is known
			{
				var BestHandType = FindBestHand(Personal, Community);
				OutcomeCounter(Input: BestHandType, Outcomes);
			}
			return Outcomes;
		}

		public static int Factorial(int input)
		{
			return (input == 1) ? 1 : Factorial(input - 1) * input;
		}
		public static List<int[]> ComboGen(int Pool, int Draw)
		{
			var Params = new int[3, Draw];
			for (int i = 0; i < Draw; i++)
			{
				Params[0, i] = 0;
				Params[1, i] = Pool;
				Params[2, i] = 0;
			}

			var Combinations = new List<int[]>();
			ComboGenRecur(Combinations, Params.GetLength(1), Params);
			return Combinations;
		}
		public static void ComboGenRecur(List<int[]> Combinations, int LoopsLeft, int[,] Param)
		{
			/*
			 * Dynamically creates nested loops
			 * 
			 * Params is a 2D Matrix. 
			 * Each Column is a loop or card you want to iterate through
			 * The First Row is the start value of that loop.
			 * The Second Row is the end value. NB. Loops stop prior to this value
			 * The Third Row is actually Irrelevant but holds the current loop index of each loop
			 * 
			 * Combinations is the list of combinations that you input and get modified.
			 * 
			 * LoopsLeft is the Loop counter to terminate the recursions
			 */
			if (LoopsLeft == 0) //Base Case
			{
				var Combo = new int[Param.GetLength(1)]; // Define a new combination.
				for (int i = 0; i < Param.GetLength(1); i++) //Loop to create the array..
				{
					Combo[i] = Param[2, i];
				}
				Combinations.Add(Combo);//... and add array
				return;
			}
			else // Creating a loop case
			{
				int LoopDepth = Param.GetLength(1) - LoopsLeft; // Which nested loop you are in
				for (int i = Param[0, LoopDepth]; i < Param[1, LoopDepth]; i++)// loop to nest from
				{
					Param[2, LoopDepth] = i; // UPDATES nested loop index positions very important
					if (LoopsLeft != 1) // prevent {1 , 2} and {2 , 1}
					{
						Param[0, LoopDepth + 1] = i + 1;
					}
					ComboGenRecur(Combinations, LoopsLeft - 1, Param); // internal call to recur
				}
				return;
			}
		}
		private HANDTYPES DetermineHand(Hand Input)
		{
			HANDTYPES ThisHandType = HANDTYPES.HighCard;

			if (IsStraight(Input) && IsFlush(Input))
			{
				ThisHandType = HANDTYPES.StraightFlush;
			}
			else if (IsStraight(Input))
			{
				ThisHandType = HANDTYPES.Straight;
			}
			else if (IsFlush(Input))
			{
				ThisHandType = HANDTYPES.Flush;
			}
			else
			{
				var PairsReport = FindRepeats(Input);
				var JustValues = PairsReport.Values.ToArray();
				Array.Sort(JustValues);

				if (JustValues.SequenceEqual(new int[] { 1, 4 }))
				{
					ThisHandType = HANDTYPES.FourOfAKind;
				}
				else if (JustValues.SequenceEqual(new int[] { 1, 1, 3 }))
				{
					ThisHandType = HANDTYPES.ThreeOfAKind;
				}
				else if (JustValues.SequenceEqual(new int[] { 2, 3 }))
				{
					ThisHandType = HANDTYPES.FullHouse;
				}
				else if (JustValues.SequenceEqual(new int[] { 1, 2, 2 }))
				{
					ThisHandType = HANDTYPES.TwoPair;
				}
				else if (JustValues.SequenceEqual(new int[] { 1, 1, 1, 2 }))
				{
					ThisHandType = HANDTYPES.Pair;
				}
			}
			return ThisHandType;
		}

		private void OutcomeCounter(HANDTYPES Input, double[] Outcomes)
		{
			switch (Input) //Increment Outcome
			{
				case HANDTYPES.HighCard:
					//Console.WriteLine("Increment HighCard");
					Outcomes[0]++;
					break;
				case HANDTYPES.Pair:
					//Console.WriteLine("Increment Pair");
					Outcomes[1]++;
					break;
				case HANDTYPES.TwoPair:
					//Console.WriteLine("Increment TwoPair");
					Outcomes[2]++;
					break;
				case HANDTYPES.ThreeOfAKind:
					//Console.WriteLine("Increment ThreeOfAKind");
					Outcomes[3]++;
					break;
				case HANDTYPES.Straight:
					//Console.WriteLine("Increment Straight");
					Outcomes[4]++;
					break;
				case HANDTYPES.Flush:
					//Console.WriteLine("Increment Flush");
					Outcomes[5]++;
					break;
				case HANDTYPES.FullHouse:
					//Console.WriteLine("Increment FullHouse");
					Outcomes[6]++;
					break;
				case HANDTYPES.FourOfAKind:
					//Console.WriteLine("Increment FourOfAKind");
					Outcomes[7]++;
					break;
				case HANDTYPES.StraightFlush:
					//Console.WriteLine("Increment StraightFlush");
					Outcomes[8]++;
					break;
				case HANDTYPES.RoyalFlush:
					//Console.WriteLine("Increment RoyalFlush");
					Outcomes[9]++;
					break;
				default:
					break;
			}
		}
	}
}
