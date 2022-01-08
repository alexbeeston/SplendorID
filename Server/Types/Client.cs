using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server.Types
{
	public class Client
	{
		public string AuthKey { get; set; }
		public string ClientId { get; set; }
		public bool IsAdmin { get; set; }
		public Socket Socket { get; set; }
		public string UserName { get; set; }
	}
}
