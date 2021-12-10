﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging.Messages
{
	public class CreateNewClientResponse : BasePayload
	{
		public string ClientId { get; set; }
		public string AuthorizationKey { get; set; }
	}
}