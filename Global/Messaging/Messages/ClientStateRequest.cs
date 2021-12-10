using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages
{
	public class ClientStateRequest : BasePayload
	{
		public string ClientId { get; set; }
	}
}
