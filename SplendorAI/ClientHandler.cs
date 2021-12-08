using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

using Global;

namespace Dealer
{
	static class ClientHandler
	{
		public static void HandleClient(Socket socket)
		{
			var messenger = new Messenger(socket);
			while (true)
			{
				Console.WriteLine(messenger.ReceivePayload());
				messenger.SendPayload(Console.ReadLine());
			}
		}
	}
}
