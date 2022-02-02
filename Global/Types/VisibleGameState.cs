using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextTableFormatter;

namespace Global.Types
{
	public class VisibleGameState
	{
		public List<IdentifiedClient> ClientStates { get; set; }
		public List<DevelopmentCard> FirstTierCards { get; set; }
		public List<DevelopmentCard> SecondTierCards { get; set; }
		public List<DevelopmentCard> ThirdTierCards { get; set; }
		public List<DevelopmentCard> RevealvedDevelopmentCards { get; set; }
		public List<Noble> UnclaimedNobles { get; set; }
		public GemQuantity AvailableGems { get; set; }
		public int AvailableWilds { get; set; }

		public void Print()
		{
			PrintNobles();
			Console.WriteLine();
			PrintGems();
			Console.WriteLine();
			PrintTier("First Tier Cards", FirstTierCards);
			Console.WriteLine();
			PrintTier("Second Tier Cards", SecondTierCards);
			Console.WriteLine();
			PrintTier("Third Tier Cards", ThirdTierCards);
			Console.WriteLine();
			PrintClientState();
			Console.WriteLine();
		}

		private void PrintNobles()
		{
			Console.WriteLine("Unclaimed nobles");
			const int numColumns = 7;
			var table = new TextTable(numColumns);
			table.AddCell(string.Empty);
			table.AddCell("Points");
			AddGemHeadings(table);
			
			foreach (var noble in UnclaimedNobles)
			{
				table.AddCell(noble.Name);
				table.AddCell(GetString(noble.Points));
				table.AddCell(GetString(noble.Price.Diamond));
				table.AddCell(GetString(noble.Price.Emerald));
				table.AddCell(GetString(noble.Price.Onyx));
				table.AddCell(GetString(noble.Price.Ruby));
				table.AddCell(GetString(noble.Price.Sapphire));
			}
			Console.WriteLine(table.Render());
		}

		private void PrintGems()
		{
			Console.WriteLine("Available Gems");
			const int numColumns = 6;
			var table = new TextTable(numColumns);
			table.AddCell("Wild");
			AddGemHeadings(table);
			table.AddCell(GetString(AvailableWilds));
			table.AddCell(GetString(AvailableGems.Diamond));
			table.AddCell(GetString(AvailableGems.Emerald));
			table.AddCell(GetString(AvailableGems.Onyx));
			table.AddCell(GetString(AvailableGems.Ruby));
			table.AddCell(GetString(AvailableGems.Sapphire));
			Console.WriteLine(table.Render());
		}
		
		private void PrintTier(string tierLabel, List<DevelopmentCard> cards)
		{
			Console.WriteLine(tierLabel);
			const int numColumns = 7;
			var table = new TextTable(numColumns);
			table.AddCell("Gem");
			table.AddCell("Points");
			AddGemHeadings(table);
			foreach (var card in cards)
			{
				table.AddCell(card.Gem.ToTable());
				table.AddCell(GetString(card.Points));
				table.AddCell(GetString(card.Price.Diamond));
				table.AddCell(GetString(card.Price.Emerald));
				table.AddCell(GetString(card.Price.Onyx));
				table.AddCell(GetString(card.Price.Ruby));
				table.AddCell(GetString(card.Price.Sapphire));
			}
			Console.WriteLine(table.Render());
		}

		private void AddGemHeadings(TextTable table)
		{
			table.AddCell("Diamond");
			table.AddCell("Emerald");
			table.AddCell("Onyx");
			table.AddCell("Ruby");
			table.AddCell("Sapphire");
		}

		private void PrintClientState()
		{
			Console.WriteLine("Player States (cards / gems)");
			const int numColumns = 7;
			var table = new TextTable(numColumns);
			table.AddCell(string.Empty);
			table.AddCell("Points");
			AddGemHeadings(table);
			foreach (var client in ClientStates)
			{
				table.AddCell(client.UserName);
				table.AddCell(client.Points.ToString());
				table.AddCell($"{GetString(client.ClaimedDevelopmentCards.Count(x => x.Gem == Gem.Diamond))} / {GetString(client.Gems.Diamond)}");
				table.AddCell($"{GetString(client.ClaimedDevelopmentCards.Count(x => x.Gem == Gem.Emerald))} / {GetString(client.Gems.Emerald)}");
				table.AddCell($"{GetString(client.ClaimedDevelopmentCards.Count(x => x.Gem == Gem.Onyx))} / {GetString(client.Gems.Onyx)}");
				table.AddCell($"{GetString(client.ClaimedDevelopmentCards.Count(x => x.Gem == Gem.Ruby))} / {GetString(client.Gems.Ruby)}");
				table.AddCell($"{GetString(client.ClaimedDevelopmentCards.Count(x => x.Gem == Gem.Sapphire))} / {GetString(client.Gems.Sapphire)}");
			}
			Console.WriteLine(table.Render());
		}

		public static string GetString(int number)
		{
			return number == 0 ? string.Empty : number.ToString();
		}
	}

	public static class Extensions
	{
		public static string ToTable(this Gem gem)
		{
			return gem switch
			{
				Gem.Diamond => "Diamond",
				Gem.Emerald => "Emerald",
				Gem.Onyx => "Onyx",
				Gem.Ruby => "Ruby",
				Gem.Sapphire => "Sapphire",
				_ => throw new Exception("Type of gem is not valid"),
			};
		}
	}
}
