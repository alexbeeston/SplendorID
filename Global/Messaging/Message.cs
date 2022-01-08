using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Global.Messaging
{
	public class Message
	{
		public string MessageId { get; set; }
		public string ClientId { get; set; }
		public string EventCode { get; set; }
		public string SerializedPayload { get; set; }

	}
}
