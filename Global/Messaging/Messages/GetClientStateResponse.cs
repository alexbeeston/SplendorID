using System;
using System.Collections.Generic;
using System.Text;

using Global.Types;

namespace Global.Messaging.Messages
{
	public class GetClientStateResponse
	{
		public ClientState State { get; set; }
	}
}
