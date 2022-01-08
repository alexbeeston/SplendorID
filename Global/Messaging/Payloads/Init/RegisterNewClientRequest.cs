using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Payloads.Init
{
	public class RegisterNewClientRequest : BasePayload
	{
		public string RequestedUserName { get; set; }
	}
}
