using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages.Init
{
	public class RegisterNewClientRequest : BasePayload
	{
		public string UserName { get; set; }
	}
}
