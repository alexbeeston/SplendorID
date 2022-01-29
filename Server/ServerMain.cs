using System;
using System.Net;
using System.Net.Sockets;

using Server.Types;
using Global;
using System.Threading.Tasks;

namespace Server
{
	class ServerMain
	{
		static void Main(string[] args)
		{
			try
			{
				ActualMain();
				Console.WriteLine("End of Main");
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.ReadLine();
			}
		}

		static void ActualMain()
		{
			const int NUM_PLAYERS = 4; // TODO: read from dotnet configuration app file
			Game game = new Game(GetListener());
			game.PlayGame(NUM_PLAYERS);
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
