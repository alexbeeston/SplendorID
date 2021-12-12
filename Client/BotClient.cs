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
				"Hades",
				"Frodo",
				"The Dragon Warrior",
			};
			var selectedName = randomUserName[RandNumberGenerator.Next(randomUserName.Count)];
			return selectedName;
		}

		protected override void GreetClient()
		{
			return;
		}
	}
}
