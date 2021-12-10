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


namespace Player
{
	class ClientMain
	{
		// Can I make this non-static so that the ClientId and AuthorizationKey are instance-data?
		// Make several different clients that implement an interface?

		private static string ClientId { get; set; }
		private static string AuthorizationKey { get; set; }

		static void Main(string[] args)
		{
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
			Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			sender.Connect(remoteEP);
			var messenger = new Messenger(sender);

			var listenerTask = Task.Run(() =>
			{
				while (true)
				{
					HandleSocketInput(messenger.ReceiveMessage());
				}
			});

			var payload = new CreateNewClientRequest();
			messenger.SendMessage(Message.CreateMessage(string.Empty, payload));
			Task.WaitAll(new Task[] { listenerTask });
		}

		private static void HandleSocketInput(Message message)
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

		private static void HandleNewClientCreated(Message message)
		{
			var payload = JsonConvert.DeserializeObject<CreateNewClientResponse>(message.SerializedPayload);
			ClientId = payload.ClientId;
			AuthorizationKey = payload.AuthorizationKey;
			Console.WriteLine($"ClientId: {ClientId}\nAuthorization Key: {AuthorizationKey}");
		}
	}
}
