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

		public bool AddClient(Socket socket, bool isAdmin)
		{
			var userName = CollectClientUserName(socket);
			var clientId = new Guid().ToString();
			var authKey = new Guid().ToString();

			Clients.Add(new Client
			{
				ClientId = clientId,
				AuthKey = authKey,
				Socket = socket,
				UserName = userName,
				IsAdmin = isAdmin,
			});

			MessagingUtils.SendMessage(socket, new RegisterNewClientResponse
			{
				ClientId = clientId,
				AuthorizationKey = authKey,
				IsAdmin = isAdmin,
			});

			if (Clients.Count < 4)
			{
				Socket adminSocket = Clients.First(x => x.IsAdmin = true).Socket;
				MessagingUtils.SendMessage(adminSocket, new LastClientHasJoinedRequest());
				var rawResponse = MessagingUtils.ReceiveMessage(adminSocket);
				var response = MessagingUtils.Parse<LastClientHasJoinedResponse>(rawResponse);
				return response.LastClientHasJoined;
			}
			else
			{
				return true;
			}
		}

		private string CollectClientUserName(Socket socket)
		{
			string requestedUserName = string.Empty;
			bool userNameIsTaken = true;
			do
			{
				var inboundMessage = MessagingUtils.ReceiveMessage(socket);
				var inboundPayload = MessagingUtils.Parse<RegisterNewClientRequest>(inboundMessage);
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
			} while (!userNameIsTaken);
			return requestedUserName;
		}
	}
}
