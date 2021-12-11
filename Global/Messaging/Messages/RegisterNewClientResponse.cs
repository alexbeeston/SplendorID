using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages
{
	public class RegisterNewClientResponse : BasePayload
	{
		public string ClientId { get; set; }
		public string AuthorizationKey { get; set; }
	}
}
