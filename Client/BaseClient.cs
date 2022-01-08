using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Global.Messaging.Messages.Init;
using Global.Messaging.Messages.State;

namespace Client
{
	abstract class BaseClient
	{
		protected ClientState State { get; set; }
		protected string AuthorizationKey { get; set; }
		protected Messenger Messenger { get; set; }
		private Task Listener { get; set; }
		/// <summary>
		/// A list of message ids that shouldn't be handled by the default dispatcher
		/// </summary>
		protected List<string> ReservedMessages { get; set; }

		public BaseClient()
		{
			State = new ClientState();
		}

		public void Run()
		{
			EstablishMessengerConnection();
			GreetClient();
			EstablishDefaultListener();
			RequestRegistration();
			Listener.Wait();
		}

		// Init Methods

		private void EstablishMessengerConnection()
		{
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
			Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(remoteEP);
			Messenger = new Messenger(socket);
		}

		private void EstablishDefaultListener()
		{
			Listener = Task.Run(() =>
			{
				while (true)
				{
					var message = Messenger.ReceiveMessage();
					if (ReservedMessages.Contains(message.MessageId))
					{
						// notify everyone that this message just came through
					}
					else
					{
						DefaultDispatcher(message);
					}
				}
			});
		}

		protected void DefaultDispatcher(Message message)
		{
			switch (message.EventCode)
			{
				case nameof(RegisterNewClientResponse):
					AcceptRegisterNewClientResponse(message);
					break;
				case nameof(CreateGameResponse):
					AcceptCreateGameResponse(message);
					break;
				case nameof(JoinGameResponse):
					AcceptJoinGameResponse(message);
					break;
				case nameof(GetGamesResponse):
				default:
					Console.WriteLine("Event code not recognized");
					break;
			}
		}

		// Game Play
		protected void RequestRegistration()
		{
			var payload = new RegisterNewClientRequest()
			{
				UserName = GetUserName()
			};
			Messenger.SendMessage(string.Empty, payload);
		}

		protected void EnterGame()
		{
			GameEntryMethod entryMethod = GetGameEntryMethod();
			if (entryMethod == GameEntryMethod.Create)
			{
				CreateGame();
			}
			else if (entryMethod == GameEntryMethod.Join)
			{
				string gameId = GetGameIdOfGameToJoin();
				JoinGame(gameId);
			}
			else
			{
				throw new Exception("Game entry method not recognized");
			}
		}

		protected void CreateGame()
		{
			var payload = new CreateGameRequest();
			Messenger.SendMessage(State.ClientId, payload);
		}

		protected void JoinGame(string gameId)
		{
			var payload = new JoinGameRequest
			{
				GameId = gameId
			};
			Messenger.SendMessage(State.ClientId, payload);
		}

		// Server Event Handlers
		protected void AcceptRegisterNewClientResponse(Message message)
		{
			var payload = JsonConvert.DeserializeObject<RegisterNewClientResponse>(message.SerializedPayload);
			if (!payload.Success)
			{
				HandleServerError(payload.ErrorCode);
				return;
			}

			State.ClientId = payload.ClientId;
			State.UserName = payload.UserName;
			AuthorizationKey = payload.AuthorizationKey;
			EnterGame();
		}

		protected void AcceptCreateGameResponse(Message message)
		{
			var payload = JsonConvert.DeserializeObject<CreateGameResponse>(message.SerializedPayload);
			if (!payload.Success)
			{
				HandleServerError(payload.ErrorCode);
				return;
			}
			JoinGame(payload.GameId);
		}

		protected void AcceptJoinGameResponse(Message message)
		{
			var payload = JsonConvert.DeserializeObject<JoinGameResponse>(message.SerializedPayload);
			if (!payload.Success)
			{
				HandleServerError(payload.ErrorCode);
				return;
			}
			State.GameId = payload.GameId;
			Console.WriteLine($"Just joined game {payload.GameId}");
		}

		// Helpers
		protected Message WaitForMessage(string messageId)
		{
		}

		// Abstract Methods
		protected abstract void HandleServerError(ErrorCode code); // might have a lot of duplicate code
		protected abstract void GreetClient();
		protected abstract string GetUserName();
		protected abstract GameEntryMethod GetGameEntryMethod();
		protected abstract string GetGameIdOfGameToJoin();
	}
}
