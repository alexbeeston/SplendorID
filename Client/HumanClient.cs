using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Global.Messaging.Messages;

namespace Client
{
	class HumanClient : BaseClient
	{
		protected override string GetUserName()
		{
			Console.WriteLine("Please enter a user name");
			return Console.ReadLine();
		}

		protected override void GreetClient()
		{
			Console.WriteLine("--- Welcome to Splendor ---");
		}

		protected override void HandleServerError(ErrorCode code)
		{
			switch (code)
			{
				case ErrorCode.UserNameTaken:
					Console.WriteLine("The user name is taken. Please try again");
					RegisterClient(GetUserName());
					break;
				default:
					throw new Exception("Error code not registered on client");
			}
		}
	}
}
