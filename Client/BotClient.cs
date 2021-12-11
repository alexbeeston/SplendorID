using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Global.Messaging.Messages;

namespace Client
{
	class BotClient : BaseClient
	{
		protected Random RandNumberGenerator { get; set; }

		public BotClient()
		{
			RandNumberGenerator = new Random();
		}

		protected override string GetUserName()
		{
			var randomUserName = new List<string>
			{
				"Bob Vance - Vance Refrigeration",
				"Sparky Sparky Boom Man",
			};
			var selectedName = randomUserName[RandNumberGenerator.Next(randomUserName.Count)];
			Console.WriteLine($"For any humans out there watching my console, my name is {selectedName}, I am a bot, and I will use my AI powers to destroy you in Splendor");
			return selectedName;
		}

		protected override void GreetClient()
		{
			return;
		}
	}
}
