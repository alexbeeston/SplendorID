using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Global.Messaging.Messages;

namespace Client
{
	class HumanClient : BaseClient
	{
		// Init
		protected override void GreetClient()
		{
			Console.WriteLine("--- Welcome to Splendor ---");
		}

		protected override string GetUserName()
		{
			Console.WriteLine("Please enter a user name");
			return Console.ReadLine();
		}

		protected override GameEntryMethod GetGameEntryMethod()
		{
			var options = new List<KeyValuePair<GameEntryMethod, string>>
			{
				new KeyValuePair<GameEntryMethod, string>(GameEntryMethod.Create, "Create a game"),
				new KeyValuePair<GameEntryMethod, string>(GameEntryMethod.Join, "Join a game")
			};

			int selection = GetNumericSelection(options.Select(x => x.Value).ToList());
			return options[selection].Key;
		}


		protected override string GetGameIdOfGameToJoin()
		{
			Console.WriteLine("Please enter the ID of the game you would like to join");
			return Console.ReadLine();
		}

		// Helpers
		private int GetNumericSelection(List<string> options, string instructions = "Please make a selection")
		{
			int userInputAsInt;
			bool inputValidates;
			do
			{
				Console.WriteLine(instructions);
				for (int i = 0; i < options.Count; i++) Console.WriteLine($"  {i + 1} - {options[i]}");
				string userInputAsString = Console.ReadLine();
				bool parseSucceeded = int.TryParse(userInputAsString, out userInputAsInt);
				inputValidates = parseSucceeded && userInputAsInt > 0 && userInputAsInt <= options.Count;
				if (!inputValidates)
				{
					Console.WriteLine($"'{userInputAsString}' is not a valid selection in this context.");
				}
			} while (!inputValidates);
			return userInputAsInt - 1;

		}

		// Error Handling
		protected override void HandleServerError(ErrorCode code)
		{
			switch (code)
			{
				case ErrorCode.UserNameTaken:
					Console.WriteLine("The user name is taken. Please try again");
					RequestRegistration();
					break;
				default:
					throw new Exception("Error code not registered on client");
			}
		}
	}
}
