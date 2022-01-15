using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Types
{
	public class VisibleGameState
	{
		public List<IdentifiedClient> ClientStates { get; set; }
		public List<DevelopmentCard> RevealvedDevelopmentCards { get; set; }
		public List<Noble> UnclaimedNobles { get; set; }
		public GemQuantity AvailableGems { get; set; }
		public int AvailableWilds { get; set; }
	}
}
