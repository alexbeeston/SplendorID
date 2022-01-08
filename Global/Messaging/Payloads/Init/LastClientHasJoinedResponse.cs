using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Payloads.Init
{
	/// <summary>
	/// Sent by the admin to inform server if all clients have been added
	/// </summary>
	public class LastClientHasJoinedResponse : BaseResponsePayload
	{
		public bool LastClientHasJoined { get; set; }
	}
}
