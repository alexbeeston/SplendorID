using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Global.State
{
	public class IdentifiedClient : ClientState
	{
		public string ClientId { get; set; }
		public string UserName { get; set; }
		public Socket Socket { get; set; }
	}
}
