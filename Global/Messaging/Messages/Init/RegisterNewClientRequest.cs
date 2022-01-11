using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages.Init
{
	public class RegisterNewClientRequest : BaseMessage
	{
		public string RequestedUserName { get; set; }
	}
}
