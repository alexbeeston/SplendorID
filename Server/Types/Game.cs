using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using Global.Messaging;
using Global.Messaging.Messages.Init;

namespace Server.Types
{
	public class Game
	{
		public List<Client> Clients { get; set; }
		public Game()
		{
			Clients = new List<Client>();
		}

		public void AddClient(Socket socket)
		{
			Client client = null;
			do
			{
				var request = MessagingUtils.ReceiveMessage<RegisterNewClientRequest>(socket);
				lock (Clients)
				{
					if (Clients.Exists(x => x.UserName == request.RequestedUserName))
					{
						var response = new RegisterNewClientResponse
						{
							Success = false,
							Error = ErrorCode.UserNameTaken
						};
						MessagingUtils.SendMessage(socket, response);
					}
					else
					{
						client = new Client
						{
							ClientId = new Guid().ToString(),
							Socket = socket,
							UserName = request.RequestedUserName
						};
						Clients.Add(client);
					}
				}
			} while (client == null);

			Console.WriteLine($"Just added {client.UserName}");

			MessagingUtils.SendMessage(socket, new RegisterNewClientResponse
			{
				ClientId = client.ClientId
			});
		}
	}
}
