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
			var copy1OfCard0 = Utils.ReadAllDevelopmentCards(pathToDataDir)[0];
			var copy2OfCard0 = Utils.ReadAllDevelopmentCards(pathToDataDir)[0];
			Assert.IsTrue(copy1OfCard0.Equals(copy2OfCard0));

			copy2OfCard0.Points++;
			Assert.IsFalse(copy1OfCard0.Equals(copy2OfCard0));
			copy2OfCard0.Points--;

			copy2OfCard0.Price.Diamond++;
			Assert.IsFalse(copy1OfCard0.Equals(copy2OfCard0));
			copy2OfCard0.Price.Diamond--;

			copy2OfCard0.Price.Emerald -= 2;
			Assert.IsFalse(copy1OfCard0.Equals(copy2OfCard0));
			copy2OfCard0.Price.Emerald += 2;

			copy2OfCard0.Price.Onyx += 3;
			Assert.IsFalse(copy1OfCard0.Equals(copy2OfCard0));
			copy2OfCard0.Price.Onyx -= 3;

			copy2OfCard0.Price.Ruby = copy1OfCard0.Price.Ruby + 3;
			Assert.IsFalse(copy1OfCard0.Equals(copy2OfCard0));
			copy2OfCard0.Price.Ruby = copy1OfCard0.Price.Ruby;

			copy2OfCard0.Price.Sapphire++;
			Assert.IsFalse(copy1OfCard0.Equals(copy2OfCard0));
			copy2OfCard0.Price.Sapphire--;

			copy1OfCard0.Level = DevelopmentLevel.Low;
			copy2OfCard0.Level = DevelopmentLevel.Middle;
			Assert.IsFalse(copy1OfCard0.Equals(copy2OfCard0));
			copy2OfCard0.Level = DevelopmentLevel.Low;

			copy1OfCard0.Gem = Gem.Onyx;
			copy2OfCard0.Gem = Gem.Diamond;
			Assert.IsFalse(copy1OfCard0.Equals(copy2OfCard0));
			copy2OfCard0.Gem = Gem.Onyx;

			Assert.IsTrue(copy1OfCard0.Equals(copy2OfCard0));
		}

		[TestMethod]
		public void TestDevelopmentCardEqualityOnRandomCards()
		{
			var deck = Utils.ReadAllDevelopmentCards(pathToDataDir);

			for (int i = 0; i < deck.Count; i++)
			{
				(int index1, int index2) = GetRandomIndices(deck.Count);
				Assert.IsFalse(deck[index1].Equals(deck[index2]));
			}
		}

		[TestMethod]
		public void TestNobleEqualityOnFirstNoble()
		{
			var copy1OfNoble0 = Utils.ReadAllNobles(pathToDataDir)[0];
			var copy2OfNoble0 = Utils.ReadAllNobles(pathToDataDir)[0];

			Assert.IsTrue(copy1OfNoble0.Equals(copy2OfNoble0));

			copy2OfNoble0.Points++;
			Assert.IsFalse(copy1OfNoble0.Equals(copy2OfNoble0));
			copy2OfNoble0.Points--;

			copy2OfNoble0.Price.Diamond++;
			Assert.IsFalse(copy1OfNoble0.Equals(copy2OfNoble0));
			copy2OfNoble0.Price.Diamond--;

			Assert.IsTrue(copy1OfNoble0.Equals(copy2OfNoble0));
		}

		[TestMethod]
		public void TestNoblesEqualityOnRandomNobles()
		{
			var deck = Utils.ReadAllNobles(pathToDataDir);

			for (int i = 0; i < deck.Count; i++)
			{
				(int index1, int index2) = GetRandomIndices(deck.Count);
				Assert.IsFalse(deck[index1].Equals(deck[index2]));
			}
		}

		private (int, int) GetRandomIndices(int max)
		{
			var random = new Random();
			var index1 = random.Next(0, max);
			var index2 = (index1 + random.Next(1, max - 1)) % max;
			return (index1, index2);
		}

		private string pathToDataDir = @"..\..\..\..\Global\Data";
	}
}
