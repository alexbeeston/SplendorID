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
		public List<DevelopmentCard> RevealvedDevelopmentCards { get; set; }
		public List<Noble> UnclaimedNobles { get; set; }
		public GemQuantity AvailableGems { get; set; }
		public int AvailableWilds { get; set; }

		public void Print()
		{
			PrintNobles();
			Console.WriteLine();
			PrintGems();
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

		private void AddGemHeadings(TextTable table)
		{
			table.AddCell("Diamond");
			table.AddCell("Emerald");
			table.AddCell("Onyx");
			table.AddCell("Ruby");
			table.AddCell("Sapphire");
		}

		public static string GetString(int number)
		{
			return number == 0 ? string.Empty : number.ToString();
		}
	}
}
