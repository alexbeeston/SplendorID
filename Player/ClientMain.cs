using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Global;
using Global.Messaging;
using Global.Messaging.Client;
using Global.Messaging.Server;


namespace Player
{
	class ClientMain
	{
		private static string ClientId { get; set;}
		private static string AuthorizationKey { get; set; }

		static void Main(string[] args)
		{
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
			Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			sender.Connect(remoteEP);
			var messenger = new Messenger(sender);

			while (true)
			{
				HandleSocketInput(messenger.ReceivePayload());
			}
		}

		private static void HandleSocketInput(Message message)
		{
			switch (message.EventCode)
			{
				case EventCode.NewClientCreated:
					HandleNewClientCreated(message);
					break;
				default:
					Console.WriteLine("EventCode not recognized");
					break;
			}
		}

		private static void HandleNewClientCreated(Message message)
		{
			var payload = JsonConvert.DeserializeObject<NewClientCreated>(message.SerializedPayload);
			ClientId = payload.ClientId;
			AuthorizationKey = payload.AuthorizationKey;
			Console.WriteLine($"ClientId: {ClientId}\nAuthorization Key: {AuthorizationKey}");
		}
	}
}
