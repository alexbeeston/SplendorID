using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages
{
	public class RegisterNewClientResponse : BaseResponseMessage
	{
		public string ClientId { get; set; }
	}
}
