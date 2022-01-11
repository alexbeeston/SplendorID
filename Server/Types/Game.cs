using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using Global.Messaging;
using Global.Messaging.Payloads.Init;

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
				var inboundMessage = MessagingUtils.ReceiveMessage(socket);
				var inboundPayload = MessagingUtils.Parse<RegisterNewClientRequest>(inboundMessage);
				lock (Clients)
				{
					if (Clients.Exists(x => x.UserName == inboundPayload.RequestedUserName))
					{
						var outboundPayload = new RegisterNewClientResponse
						{
							Success = false,
							Error = ErrorCode.UserNameTaken
						};
						MessagingUtils.SendMessage(socket, outboundPayload);
					}
					else
					{
						requestedUserName = inboundPayload.RequestedUserName;
						userNameIsTaken = false;
					}
				}
				Console.WriteLine($"Received user name {requestedUserName}. Is taken? {userNameIsTaken}");
			} while (userNameIsTaken);
			Console.WriteLine($"Going to assign username {requestedUserName}");
			return requestedUserName;
		}
	}
}
