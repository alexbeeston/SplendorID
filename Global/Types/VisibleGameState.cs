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
			Console.WriteLine("Nobles".PadLeft(3).PadRight(3));
			const int numColumns = 7;
			var basicTable = new TextTable(numColumns);
			basicTable.AddCell(string.Empty);
			basicTable.AddCell("Points");
			basicTable.AddCell("Diamond");
			basicTable.AddCell("Emerald");
			basicTable.AddCell("Onyx");
			basicTable.AddCell("Ruby");
			basicTable.AddCell("Sapphire");
			
			foreach (var noble in UnclaimedNobles)
			{
				basicTable.AddCell(noble.Name);
				basicTable.AddCell(GetString(noble.Points));
				basicTable.AddCell(GetString(noble.Price.Diamond));
				basicTable.AddCell(GetString(noble.Price.Emerald));
				basicTable.AddCell(GetString(noble.Price.Onyx));
				basicTable.AddCell(GetString(noble.Price.Ruby));
				basicTable.AddCell(GetString(noble.Price.Sapphire));
			}
			Console.WriteLine(basicTable.Render());
		}

		public static string GetString(int number)
		{
			return number == 0 ? string.Empty : number.ToString();
		}
	}
}
