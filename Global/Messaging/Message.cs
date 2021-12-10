using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Global.Messaging
{
	public class Message
	{
		public string ClientId { get; set; }
		public string EventCode { get; set; }
		public string SerializedPayload { get; set; }

		public static Message CreateMessage(string clientId, BasePayload payload)
		{
			return new Message
			{
				ClientId = clientId,
				EventCode = payload.GetType().Name,
				SerializedPayload = JsonConvert.SerializeObject(payload),
			};
		}
	}
}
