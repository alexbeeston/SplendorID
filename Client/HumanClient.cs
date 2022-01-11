using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using Global;
using Global.Messaging;

namespace Client
{
	public class HumanClient : BaseClient
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

		// Helpers
		private static T GetUserInput<T>(List<KeyValuePair<T, string>> options, string instructions = "")
		{
			int userInputAsInt;
			bool inputValidates;
			do
			{
				Console.WriteLine(instructions);
				for (int i = 0; i < options.Count; i++)
				{
					Console.WriteLine($"  {i + 1} - {options[i].Value}");
				}
				string userInputAsString = Console.ReadLine();
				bool parseSucceeded = int.TryParse(userInputAsString, out userInputAsInt);
				inputValidates = parseSucceeded && userInputAsInt > 0 && userInputAsInt <= options.Count;
				if (!inputValidates)
				{
					Console.WriteLine($"'{userInputAsString}' is not a valid selection in this context.");
				}
			} while (!inputValidates);

			return options[userInputAsInt].Key;
		}
	}
}
