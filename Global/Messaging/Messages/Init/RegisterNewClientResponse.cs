using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages.Init
{
	public class RegisterNewClientResponse : BaseResponseMessage
	{
		public string ClientId { get; set; }
		public bool IsAdmin { get; set; }
	}
}
