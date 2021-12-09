using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Client
{
	public class RequestClientState : BasePayload
	{
		public string ClientId { get; set; }
	}
}
