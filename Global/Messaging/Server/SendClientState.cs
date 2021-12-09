using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Server
{
	public class SendClientState : BasePayload
	{
		public ClientState ClientState { get; set; }
	}
}
