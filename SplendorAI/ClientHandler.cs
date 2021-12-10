using System;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Global.Messaging.Messages;
using Server.Types;

namespace Server
{
	// Can I make this non-static so that I have instance-access to the dataProvider and the socket?
	static class ClientHandler
	{
		private static Messenger Messenger { get; set; }
		private static IDataProvider DataProvider { get; set; }

		public static void HandleClient(object parametersAsObject) // will be the constructor
		{
			var parameters = (ClientHandlerParameters)parametersAsObject;
			DataProvider = parameters.DataProvider;
			Messenger = new Messenger(parameters.Socket);

			while (true)
			{
				HandleSocketInput(Messenger.ReceiveMessage());
			}
		}

		private static void HandleSocketInput(Message message)
		{
			switch (message.EventCode)
			{
				case nameof(CreateNewClientRequest):
					CreateNewClient();
					break;
				default:
					Console.WriteLine($"Event code not recognized: {message.EventCode}");
					break;
			}
		}

		private static void CreateNewClient() // add a return type for better self-documentation and then send the message from the caller? Could also save repeated code
		{
			(string clientId, string authorizationKey) = DataProvider.AddNewClient();
			var payload = new CreateNewClientResponse
			{
				AuthorizationKey = authorizationKey,
				ClientId = clientId,
			};
			Messenger.SendMessage(Message.CreateMessage(clientId, payload));
		}
	}
}
