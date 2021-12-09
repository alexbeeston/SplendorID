using System;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Server.Types;

namespace Server
{
	static class ClientHandler
	{
		public static void HandleClient(object parametersAsObject)
		{
			var parameters = (ClientHandlerParameters)parametersAsObject;
			var dataProvider = parameters.DataProvider;

			var messenger = new Messenger(parameters.Socket);

			string authToken = dataProvider.AddNewPlayer();
			var payload = new EstalishAuthToken
			{
				AuthToken = authToken
			};
			var message = new Message
			{
				AuthToken = string.Empty,
				EventCode = EventCode.EstalishAuthToken,
				RequestId = string.Empty,
				SerializedPayload = JsonConvert.SerializeObject(payload)
			};
			messenger.SendPayload(message);

			while (true)
			{
				HandleSocketInput(messenger.ReceivePayload());
			}
			// TODO: close socket
		}

		private static void HandleSocketInput(Message socketInput)
		{
			switch (socketInput.EventCode)
			{
				case EventCode.EstalishAuthToken:
					Console.WriteLine("They want to ");
					break;
				default:
					Console.WriteLine($"They want to do something else. They said {socketInput}");
					break;
			}
			// respond to request for game state
			// respond to request for player state validation
			// respond to inquiry on game state
			// respond to answer to whether to start new game
			// respond to request for turn submission
		}
	}
}
