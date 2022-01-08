using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Payloads.Init
{
	public class RegisterNewClientResponse : BaseResponsePayload
	{
		public string ClientId { get; set; }
		public string AuthorizationKey { get; set; }
		public bool IsAdmin { get; set; }
	}
}
