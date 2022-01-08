using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages.State
{
	public class GetGamesResponse : BaseResponsePayload
	{
		public List<string> GameIds { get; set; }
	}
}
