using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages.Init
{
	public class JoinGameResponse : BaseResponsePayload
	{
		public string GameId { get; set; }
	}
}
