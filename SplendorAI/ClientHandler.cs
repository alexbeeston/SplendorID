using System;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Global.Messaging.Server;
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

			// Create Client
			(string clientId, string authorizationKey) = dataProvider.AddNewClient();
			var payload = new NewClientCreated
			{
				AuthorizationKey = authorizationKey,
				ClientId = clientId,
			};
			var message = new Message(clientId, EventCode.NewClientCreated, JsonConvert.SerializeObject(payload));
			messenger.SendPayload(message);

			while (true)
			{
				HandleSocketInput(messenger.ReceivePayload());
			}
		}

		private static void HandleSocketInput(Message socketInput)
		{
			switch (socketInput.EventCode)
			{
				case EventCode.NewClientCreated:
					Console.WriteLine("They want to ");
					break;
				default:
					Console.WriteLine($"They want to do something else. They said {socketInput}");
					break;
			}
		}
	}
}
