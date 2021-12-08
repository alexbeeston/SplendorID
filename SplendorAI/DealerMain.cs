using System;
using System.Net.Http;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Global;

namespace SplendorAI
{
	class DealerMain
	{
		static void Main(string[] args)
		{
			// Get Host IP Address that is used to establish a connection
			// In this case, we get one IP address of localhost that is IP : 127.0.0.1
			// If a host has multiple addresses, you will get a list of addresses
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

			try
			{
				Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				listener.Bind(localEndPoint);

				const int MAX_REQUESTS = 10;
				listener.Listen(MAX_REQUESTS);

				Console.WriteLine(">> Waiting for a connection...");
				Socket handler = listener.Accept();

				var messenger = new Messenger(handler);
				while (true)
				{
					Console.WriteLine(messenger.ReceivePayload());
					messenger.SendPayload(Console.ReadLine());
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			Console.WriteLine("\n Press any key to continue...");
			Console.ReadKey();
		}
	}
}
