using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages
{
	public class ClientStateResponse : BaseResponsePayload
	{
		public ClientState ClientState { get; set; }
	}
}
