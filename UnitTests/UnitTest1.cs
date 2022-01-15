using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

using Newtonsoft.Json;

using Global.Types;

namespace UnitTests
{

	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestDevelopmentCardEquality()
		{
			var card1 = JsonConvert.DeserializeObject<DevelopmentCard>(File.ReadAllText(Path.Join(pathToConfigsDir, "SampleDevelopmentCard.json")));
			var card2 = JsonConvert.DeserializeObject<DevelopmentCard>(File.ReadAllText(Path.Join(pathToConfigsDir, "SampleDevelopmentCard.json")));
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

		private string pathToConfigsDir = @"..\..\..\Configs";
	}
}
