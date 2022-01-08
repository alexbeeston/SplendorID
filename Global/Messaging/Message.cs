using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Global.Messaging
{
	public class Message
	{
		public string PayloadType { get; set; }
		public string SerializedPayload { get; set; }

	}
}
