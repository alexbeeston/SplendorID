using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Global;
using Global.Messaging;

namespace Client
{
	public class BotClient : BaseClient
	{
		protected Random RandNumberGenerator { get; set; }

		public BotClient()
		{
			RandNumberGenerator = new Random();
		}

		protected override void GreetClient()
		{
			return;
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
	}
}
