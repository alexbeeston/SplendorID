using System;
using System.Collections.Generic;
using System.Text;

using Global;
using Global.Messaging;

namespace Client
{
	class BotClient : BaseClient
	{
		protected Random RandNumberGenerator { get; set; }

		// Init
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

		protected override GameEntryMethod GetGameEntryMethod()
		{
			throw new NotImplementedException();
		}

		protected override string GetGameIdOfGameToJoin()
		{
			throw new NotImplementedException();
		}

		// Error Handling
		protected override void HandleServerError(ErrorCode code)
		{
			switch (code)
			{
				case ErrorCode.UserNameTaken:
					RequestRegistration();
					break;
				default:
					throw new Exception("Error code not registered on client");
			}
		}
	}
}
