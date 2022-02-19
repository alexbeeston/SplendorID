using Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Server;

namespace Driver
{
	class Program
	{
		static void Main(string[] args)
		{
			const int NUM_PLAYERS = 2; // TODO: read from configuration file
			Game game = new Game(GetListener());
			Task.Run(() => game.PlayGame(NUM_PLAYERS));

			for (int i = 0; i < NUM_PLAYERS; i++)
			{
				var bot = new BotClient();
				Task.Run(() => bot.Run());
				Thread.Sleep(1500);
			}
		}

		static Socket GetListener()
		{
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			listener.Bind(new IPEndPoint(ipAddress, 11000));
			const int MAX_REQUESTS = 10;
			listener.Listen(MAX_REQUESTS);
			return listener;
		}
	}
}
