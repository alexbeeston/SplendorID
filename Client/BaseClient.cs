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
			EstablishListener();
			RegisterClientOnServer();
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
				case nameof(CreateNewClientResponse):
					HandleNewClientCreated(message);
					break;
				default:
					Console.WriteLine("Event code not recognized");
					break;
			}
		}

		protected abstract void RegisterClientOnServer();
		protected abstract void HandleNewClientCreated(Message message);
	}
}
