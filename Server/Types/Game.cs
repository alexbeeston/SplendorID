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
			var userName = CollectClientUserName(socket);
			var clientId = new Guid().ToString();

			Clients.Add(new Client
			{
				ClientId = clientId,
				Socket = socket,
				UserName = userName,
			});
			Console.WriteLine($"Just added {userName}");

			MessagingUtils.SendMessage(socket, new RegisterNewClientResponse
			{
				ClientId = clientId,
			});
		}

		private string CollectClientUserName(Socket socket)
		{
			string requestedUserName = string.Empty;
			bool userNameIsTaken = true;
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
						requestedUserName = request.RequestedUserName;
						userNameIsTaken = false;
					}
				}
			} while (userNameIsTaken);
			return requestedUserName;
		}
	}
}
