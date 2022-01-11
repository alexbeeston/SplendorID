using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Messaging
{
	public abstract class BaseResponsePayload : BasePayload
	{
		public bool Success { get; set; } = true;
		public ErrorCode Error { get; set; } = ErrorCode.None;
	}

	public enum ErrorCode
	{
		None,
		UserNameTaken,
	}
}
