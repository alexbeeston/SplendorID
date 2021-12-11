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

	}
}
