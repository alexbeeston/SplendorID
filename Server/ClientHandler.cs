using System;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Global.Messaging.Messages;
using Server.Exceptions;

namespace Server
{
	class ClientHandler
	{
		private Messenger Messenger { get; set; }
		private IDataProvider DataProvider { get; set; }

		// Init
		public ClientHandler(Messenger messenger, IDataProvider dataProvider)
		{
			Messenger = messenger;
			DataProvider = dataProvider;
		}

		public void Run()
		{
			while (true)
			{
				DispatchMessage(Messenger.ReceiveMessage());
			}
		}

		private void DispatchMessage(Message message)
		{
			switch (message.EventCode)
			{
				case nameof(RegisterNewClientRequest):
					HandleRegisterNewClientRequest(message);
					break;
				case nameof(CreateGameRequest):
					HandleCreateGameRequest(message);
					break;
				default:
					Console.WriteLine($"Event code not recognized: {message.EventCode}");
					break;
			}
		}

		// Client Event Handlers
		private void HandleRegisterNewClientRequest(Message message) // add a return type for better self-documentation and then send the message from the caller? Could also save repeated code
		{
			var requestPayload = JsonConvert.DeserializeObject<RegisterNewClientRequest>(message.SerializedPayload);
			string clientId = string.Empty;
			string authorizationKey;
			RegisterNewClientResponse response;
			try
			{
				(clientId, authorizationKey) = DataProvider.AddNewClient(requestPayload.UserName);
				response = new RegisterNewClientResponse
				{
					AuthorizationKey = authorizationKey,
					ClientId = clientId,
					UserName = requestPayload.UserName
				};
			}
			catch (UserNameNotAvailable)
			{
				response = new RegisterNewClientResponse
				{
					Success = false,
					ErrorCode = ErrorCode.UserNameTaken
				};
			}
			Messenger.SendMessage(clientId, response);
		}

		private void HandleCreateGameRequest(Message message)
		{
			// TODO: verify user is registered
			string gameId = DataProvider.CreateGame();
			var response = new CreateGameResponse
			{
				GameId = gameId
			};
			Messenger.SendMessage(message.ClientId, response);
		}
	}
}
