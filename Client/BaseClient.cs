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
					Dispatcher(Messenger.ReceiveMessage());
				}
			});
		}

		protected void Dispatcher(Message message)
		{
			switch (message.EventCode)
			{
				case nameof(RegisterNewClientResponse):
					HandleRegisterClient(message);
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

		protected void HandleRegisterClient(Message message)
		{
			var payload = JsonConvert.DeserializeObject<RegisterNewClientResponse>(message.SerializedPayload);
			if (payload.Success)
			{
				State.ClientId = payload.ClientId;
				State.UserName = payload.UserName;
				AuthorizationKey = payload.AuthorizationKey;
				Console.WriteLine($"ClientId: {State.ClientId}\nAuthorizationKey: {AuthorizationKey}\n");
			}
			else
			{
				HandleError("That user name is already taken. Please try again", payload.ErrorCode);
				RegisterClient(GetUserName());
			}
		}

		protected void HandleError(string error, ErrorCode code)
		{
			Console.WriteLine(error);
		}

		protected abstract void GreetClient();
		protected abstract string GetUserName();
	}
}
