using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages
{
	public class GetClientStateRequest
	{
		public string RequestedClientId { get; set; }
	}
}
