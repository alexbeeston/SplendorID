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
		protected string ClientId { get; set; }
		protected string AuthorizationKey { get; set; }
		protected Messenger Messenger { get; set; }
		private Task Listener { get; set; }

		public void Run()
		{
			EstablishMessengerConnection();
			GreetClient();
			EstablishListener();
			RegisterClientOnServer(GetUserName());
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
					HandleNewClientCreated(message);
					break;
				default:
					Console.WriteLine("Event code not recognized");
					break;
			}
		}

		protected void RegisterClientOnServer(string userName)
		{
			// TODO: add userName to CreateNewClient - change name to register client
			var payload = new RegisterNewClientRequest();
			Messenger.SendMessage(Message.CreateMessage(string.Empty, payload));
		}

		protected void HandleNewClientCreated(Message message)
		{
			var payload = JsonConvert.DeserializeObject<RegisterNewClientResponse>(message.SerializedPayload);
			ClientId = payload.ClientId;
			AuthorizationKey = payload.AuthorizationKey;
			Console.WriteLine($"ClientId: {ClientId}\nAuthorizationKey: {AuthorizationKey}\n");
		}

		protected abstract void GreetClient();
		protected abstract string GetUserName();
	}
}
