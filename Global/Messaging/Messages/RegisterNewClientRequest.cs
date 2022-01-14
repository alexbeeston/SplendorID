using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages
{
	public class RegisterNewClientRequest : BaseMessage
	{
		public string RequestedUserName { get; set; }
	}
}
