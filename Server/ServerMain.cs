using System;
using System.Net;
using System.Net.Sockets;

using Server.Types;
using Global;

namespace Server
{
	class ServerMain
	{
		static void Main(string[] args)
		{
			var listener = GetListener();
			Game game = new Game();

			Socket socket = listener.Accept();
			game.AddClient(socket, true);
			bool lastClientHasJoined;
			do
			{
				socket = listener.Accept();
				lastClientHasJoined = game.AddClient(socket, false);
			} while (!lastClientHasJoined);
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
