using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Global;

namespace Player
{
	class PlayerMain
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
				messenger.SendPayload(Console.ReadLine());
				Console.WriteLine(messenger.ReceivePayload());
			}
		}
	}
}
