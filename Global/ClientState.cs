using System;
using System.Collections.Generic;
using System.Text;

namespace Global
{
	public class ClientState
	{
		public ClientState(string clientId)
		{
			ClientId = clientId;
		}

		public List<string> ClaimedMines { get; set; }
		public List<string> ClaimedNobles { get; set; }
		public List<string> ReservedCards { get; set; }
		public int Emeralds { get; set; }
		public int Diamonds { get; set; }
		public int Onyx { get; set; }
		public int Rubies { get; set; }
		public int Saphire { get; set; }
		public int Wild { get; set; }
		public string ClientId { get; set; }
		public string UserName { get; set; }
	}
}
