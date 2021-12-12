using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging
{
	public class BaseResponsePayload : BasePayload
	{
		public bool Success { get; set; } = true;
		public ErrorCode ErrorCode { get; set; }
	}
}
