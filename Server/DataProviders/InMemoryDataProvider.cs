using Global;
using System;
using System.Collections.Generic;
using System.Linq;

using Server.Exceptions;

namespace Server.DataProviders
{
	public class InMemoryDataProvider : IDataProvider
	{
		private readonly List<ClientState> Clients;
		private readonly Dictionary<string, string> AuthorizationMappings;

		public (string, string) AddNewClient(string userName)
		{
			string clientId;
			string authorizationKey;
			lock (Clients)
			{
				if (Clients.Exists(x => x.UserName == userName))
				{
					throw new UserNameTaken();
				}
				clientId = Guid.NewGuid().ToString();
				authorizationKey = Guid.NewGuid().ToString();
				Clients.Add(new ClientState(clientId, userName));
			}
			AuthorizationMappings.Add(clientId, authorizationKey);
			Console.WriteLine($"Created user {clientId}, with user name {userName} and authorization key {authorizationKey}");
			return (clientId, authorizationKey);
		}

		public ClientState GetPlayerState(string authToken)
		{
			lock (Clients)
			{
				return Clients.Find(x => x.ClientId == authToken);
			}
		}

		public InMemoryDataProvider()
		{
			Clients = new List<ClientState>();
			AuthorizationMappings = new Dictionary<string, string>();
		}
	}
}
