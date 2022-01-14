using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using Global.Messaging;
using Global.State;
using Global.Messaging.Messages;

namespace Server.Types
{
	public class Game
	{
		public List<IdentifiedClient> Clients { get; set; }
		public Game()
		{
			Clients = new List<IdentifiedClient>();
		}

		public void AddClient(Socket socket)
		{
			IdentifiedClient client = null;
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
						client = new IdentifiedClient
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

		public void PlayGame()
		{
			bool isLastTurn = false;
			do
			{
				foreach (var client in Clients)
				{
					// ask client for its state
					// make sure that it matches what the server has on file
					// send game state
					// get Client's choice
					// validate the client can take the turn
					isLastTurn = client.Points >= 15;
				}

			} while (!isLastTurn);
		}
	}
}
