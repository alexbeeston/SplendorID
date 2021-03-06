using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Global.Types
{
	public class ClientState
	{
		public int Points
		{
			get
			{
				return ClaimedDevelopmentCards.Sum(x => x.Points) + ClaimedNobles.Sum(x => x.Points);
			}
		}
		public GemQuantity Gems { get; set; }
		public int Wilds { get; set; }
		public List<DevelopmentCard> ClaimedDevelopmentCards { get; set; }
		public List<Noble> ClaimedNobles { get; set; }
		public List<DevelopmentCard> ReservedDevelopmentCards { get; set; }
	}

	public class IdentifiedClient : ClientState
	{
		public string ClientId { get; set; }
		public string UserName { get; set; }
	}

	public class AccessibleClient : IdentifiedClient
	{
		public Socket Socket { get; set; }
	}
}
