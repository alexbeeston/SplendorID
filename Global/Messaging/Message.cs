using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging
{
	public class Message
	{
		public string AuthToken { get; set; }
		public EventCode EventCode { get; set; }
		public string RequestId { get; set; }
		public string SerializedPayload { get; set; }
	}
}
