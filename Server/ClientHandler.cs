﻿using System;

using Global;
using Global.Messaging;
using Global.Messaging.Messages;

namespace Server
{
	class ClientHandler
	{
		private Messenger Messenger { get; set; }
		private IDataProvider DataProvider { get; set; }

		public ClientHandler(Messenger messenger, IDataProvider dataProvider)
		{
			Messenger = messenger;
			DataProvider = dataProvider;
		}

		public void Run()
		{
			while (true)
			{
				HandleSocketInput(Messenger.ReceiveMessage());
			}
		}

		private void HandleSocketInput(Message message)
		{
			switch (message.EventCode)
			{
				case nameof(RegisterNewClientRequest):
					RegisterNewClient();
					break;
				default:
					Console.WriteLine($"Event code not recognized: {message.EventCode}");
					break;
			}
		}

		private void RegisterNewClient() // add a return type for better self-documentation and then send the message from the caller? Could also save repeated code
		{
			(string clientId, string authorizationKey) = DataProvider.AddNewClient();
			var payload = new RegisterNewClientResponse
			{
				AuthorizationKey = authorizationKey,
				ClientId = clientId,
			};
			Messenger.SendMessage(Message.CreateMessage(clientId, payload));
		}
	}
}
