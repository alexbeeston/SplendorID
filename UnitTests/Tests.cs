using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

using Newtonsoft.Json;

using Global;
using Global.Types;
using System.Collections.Generic;

namespace UnitTests
{

	[TestClass]
	public class Tests
	{
		[TestMethod]
		public void TestDevelopmentCardEqualityOnFirstCard()
		{
			var card1 = Utils.ReadAllDevelopmentCards(pathToDataDir)[0];
			var card2 = Utils.ReadAllDevelopmentCards(pathToDataDir)[0];
			TestDevelopmentCardEquality(card1, card2);
		}

		[TestMethod]
		public void TestDevelopmentCardEqualityOnRandomCards()
		{
			var deck1 = Utils.ReadAllDevelopmentCards(pathToDataDir);
			var deck2 = Utils.ReadAllDevelopmentCards(pathToDataDir);
			Assert.IsTrue(deck1.Count == deck2.Count);

			var random = new Random();
			for (int i = 0; i < 30; i++)
			{
				var indexOfRandomCard = random.Next(0, deck1.Count - 1);
				TestDevelopmentCardEquality(deck1[indexOfRandomCard], deck2[indexOfRandomCard]);
			}
		}

		/// <summary>
		/// Verifies that the Equals method on the DevelopmentCard object work.
		/// </summary>
		/// <param name="card1">An instance of card x</param>
		/// <param name="card2">An instance of card x</param>
		public void TestDevelopmentCardEquality(DevelopmentCard card1, DevelopmentCard card2)
		{
			Assert.IsTrue(card1.Equals(card2));

			card2.Points++;
			Assert.IsFalse(card1.Equals(card2));
			card2.Points--;

			card2.Price.Diamond++;
			Assert.IsFalse(card1.Equals(card2));
			card2.Price.Diamond--;

			card2.Price.Emerald -= 2;
			Assert.IsFalse(card1.Equals(card2));
			card2.Price.Emerald += 2;

			card2.Price.Onyx += 3;
			Assert.IsFalse(card1.Equals(card2));
			card2.Price.Onyx -= 3;

			card2.Price.Ruby = card1.Price.Ruby + 3;
			Assert.IsFalse(card1.Equals(card2));
			card2.Price.Ruby = card1.Price.Ruby;

			card2.Price.Sapphire++;
			Assert.IsFalse(card1.Equals(card2));
			card2.Price.Sapphire--;

			card1.Level = DevelopmentLevel.Low;
			card2.Level = DevelopmentLevel.Middle;
			Assert.IsFalse(card1.Equals(card2));
			card2.Level = DevelopmentLevel.Low;

			card1.Gem = Gem.Onyx;
			card2.Gem = Gem.Diamond;
			Assert.IsFalse(card1.Equals(card2));
			card2.Gem = Gem.Onyx;

			Assert.IsTrue(card1.Equals(card2));
		}

		[TestMethod]
		public void TestNobleEquality()
		{
			//var card1 = JsonConvert.DeserializeObject<DevelopmentCard>(File.ReadAllText(Path.Join(pathToDataDir, "SampleDevelopmentCard.json")));
			//var list = new List<DevelopmentCard>
			//{
			//	card1, card1
			//};
			//File.WriteAllText(Path.Join(pathToDataDir, "list.json"), JsonConvert.SerializeObject(list, Formatting.Indented));
			//var n = new Noble
			//{
			//	Points = 4,
			//	Price = new GemQuantity
			//	{
			//		Diamond = 0,
			//		Emerald = 3,
			//		Onyx = 2,
			//		Ruby = 1,
			//		Sapphire = 2
			//	}
			//};
			//string d = JsonConvert.SerializeObject(n, Formatting.Indented);
		}

		private string pathToDataDir = @"..\..\..\..\Global\Data";
	}
}
