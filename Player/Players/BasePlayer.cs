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

namespace Client.Players
{
	abstract class BasePlayer
	{
		protected string ClientId { get; set; }
		protected string AuthorizationKey { get; set; }
		protected Messenger Messenger { get; set; }

		public void Run()
		{
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
			Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			sender.Connect(remoteEP);
			Messenger = new Messenger(sender);

			var listenerTask = Task.Run(() =>
			{
				while (true)
				{
					Dispatcher(Messenger.ReceiveMessage());
				}
			});
			CreateNewClient();
			Task.WaitAll(new Task[] { listenerTask });
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

		protected abstract void CreateNewClient();
		protected abstract void HandleNewClientCreated(Message message);
	}
}
