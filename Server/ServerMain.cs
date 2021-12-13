using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Global;

using Server.DataProviders;

namespace Server
{
	class ServerMain
	{
		static void Main(string[] args)
		{
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			listener.Bind(new IPEndPoint(ipAddress, 11000));
			const int MAX_REQUESTS = 10;
			listener.Listen(MAX_REQUESTS);
			IDataProvider dataProvider = new InMemoryDataProvider();
			dataProvider.CreateGame();

			while (true)
			{
				Socket socket = listener.Accept();
				Task.Run(() => // TODO: limit the number of requests that the server can concurrently handle
				{
					var clientHandler = new ClientHandler(new Messenger(socket), dataProvider);
					clientHandler.Run();
				});
			}
		}
	}
}
