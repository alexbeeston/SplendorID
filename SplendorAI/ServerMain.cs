using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Global;

using Server.DataProviders;
using Server.Types;

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

			while (true)
			{
				Socket socket = listener.Accept();
				var clientParameters = new ClientHandlerParameters
				{
					Socket = socket,
					DataProvider = dataProvider
				};
				ThreadPool.QueueUserWorkItem(ClientHandler.HandleClient, clientParameters);
			}
		}
	}
}
