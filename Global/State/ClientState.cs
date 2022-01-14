using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Global.State
{
	public class ClientState
	{
		public int Points { get; set; }
		public int Emerald { get; set; }
		public int Diamond { get; set; }
		public int Onyx { get; set; }
		public int Ruby { get; set; }
		public int Saphire { get; set; }
		public int Wild { get; set; }
		public List<string> ClaimedMines { get; set; }
		public List<string> ClaimedNobles { get; set; }
		public List<string> ReservedCards { get; set; }
	}
}
