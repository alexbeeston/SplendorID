using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging
{
	public class Message
	{
		public string ClientId { get; set; }
		public EventCode EventCode { get; set; }
		public string SerializedPayload { get; set; } // TODO: replace with base payload, and then serilize (? works for sending, how about receiving?)

		public Message(string clientId, EventCode eventCode, string serializedPayload)
		{
			ClientId = clientId;
			EventCode = eventCode;
			SerializedPayload = serializedPayload;
		}
	}
}
