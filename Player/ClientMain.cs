using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Global;
using Global.Messaging;

namespace Player
{
	class ClientMain
	{
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
				case EventCode.EstalishAuthToken:
					EstablishAuthToken(message);
					break;
				default:
					Console.WriteLine("Not sure what that message was");
					break;
			}
		}

		private static void EstablishAuthToken(Message message)
		{
			var payload = JsonConvert.DeserializeObject<EstalishAuthToken>(message.SerializedPayload);
			Console.WriteLine($"The server gave us the auth token of {payload.AuthToken}");
		}
	}
}
