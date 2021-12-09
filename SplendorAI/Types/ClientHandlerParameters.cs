using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Server.Types
{
	class ClientHandlerParameters
	{
		public Socket Socket { get; set; }
		public IDataProvider DataProvider { get; set; }
	}
}
