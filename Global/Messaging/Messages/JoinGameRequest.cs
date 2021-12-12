using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages
{
	public class JoinGameRequest : BasePayload
	{
		public string GameId { get; set; }
	}
}
