using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Global;
using Global.Messaging;
using Global.Messaging.Messages.State;

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
			return GameEntryMethod.Join;
		}

		protected override string GetGameIdOfGameToJoin()
		{
			var gamesRequest = new GetGamesRequest();
			var messageId = Messenger.SendMessage(State.ClientId, gamesRequest);


			ReservedMessages.Add(messageId);
			ReservedMessages.Remove(messageId);
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
