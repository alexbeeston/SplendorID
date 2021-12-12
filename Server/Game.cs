using System;
using System.Collections.Generic;
using System.Text;

using Global;

namespace Server
{
	public class Game
	{
		public Game(string gameId)
		{
			GameId = gameId;
			ClientIds = new List<string>();
		}

		public string GameId { get; set; }
		public List<string> ClientIds { get; set; }
		public int Emeralds { get; set; }
		public int Diamonds { get; set; }
		public int Onyx { get; set; }
		public int Rubies { get; set; }
		public int Saphire { get; set; }
		public int Wild { get; set; }

		public void AddClientToGame(string clientId)
		{
			return;
		}
		
		// TODO: add state properties to represent the mines that are uncovered and the ones that will be available
		// TODO: add a claimMine method
		// etc.
	}
}
