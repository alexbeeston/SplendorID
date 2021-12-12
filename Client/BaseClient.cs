using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Global.Messaging.Messages;

namespace Client
{
	abstract class BaseClient
	{
		protected ClientState State { get; set; }
		protected string AuthorizationKey { get; set; }
		protected Messenger Messenger { get; set; }
		private Task Listener { get; set; }

		public BaseClient()
		{
			State = new ClientState();
		}

		public void Run()
		{
			EstablishMessengerConnection();
			GreetClient();
			EstablishListener();
			RegisterClient(GetUserName());
			Listener.Wait();
		}

		private void EstablishMessengerConnection()
		{
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
			Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(remoteEP);
			Messenger = new Messenger(socket);
		}

		private void EstablishListener()
		{
			Listener = Task.Run(() =>
			{
				while (true)
				{
					DispatchMessage(Messenger.ReceiveMessage());
				}
			});
		}

		protected void DispatchMessage(Message message)
		{
			switch (message.EventCode)
			{
				case nameof(RegisterNewClientResponse):
					AcceptClientRegistration(message);
					break;
				default:
					Console.WriteLine("Event code not recognized");
					break;
			}
		}

		protected void RegisterClient(string userName)
		{
			var payload = new RegisterNewClientRequest()
			{
				UserName = userName,
			};
			Messenger.SendMessage(Message.CreateMessage(string.Empty, payload));
		}


		// Event Handlers
		protected void AcceptClientRegistration(Message message)
		{
			var payload = JsonConvert.DeserializeObject<RegisterNewClientResponse>(message.SerializedPayload);
			if (payload.Success)
			{
				State.ClientId = payload.ClientId;
				State.UserName = payload.UserName;
				AuthorizationKey = payload.AuthorizationKey;
				Console.WriteLine($"Successfully registered on the server with the user name {State.UserName}");
			}
			else
			{
				HandleServerError(payload.ErrorCode);
			}
		}

		// Concrete Methods
		protected abstract void HandleServerError(ErrorCode code); // might have a lot of duplicate code (?)
		protected abstract void GreetClient();
		protected abstract string GetUserName();
	}
}
