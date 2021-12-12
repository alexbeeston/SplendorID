using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages
{
	public class CreateGameResponse : BaseResponsePayload
	{
		public string GameId { get; set; }
	}
}
