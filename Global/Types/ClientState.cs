using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Global.Types
{
	public class ClientState
	{
		public int Points { get; set; }
		public GemQuantity Gems { get; set; }
		public int Wilds { get; set; }
		public List<string> ClaimedDevelopmentCards { get; set; }
		public List<string> ClaimedNobles { get; set; }
		public List<string> ReservedDevelopmentCards { get; set; }
	}
}
