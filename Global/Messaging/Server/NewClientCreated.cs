using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Server
{
	public class NewClientCreated : BasePayload
	{
		public string ClientId { get; set; }
		public string AuthorizationKey { get; set; }
	}
}
