using Global;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Server.DataProviders
{
	public class InMemoryDataProvider : IDataProvider
	{
		private readonly List<ClientState> Clients;
		private readonly Dictionary<string, string> AuthorizationMappings;

		public (string, string) AddNewClient()
		{
			var clientId = Guid.NewGuid().ToString();
			var authorizationKey = Guid.NewGuid().ToString();
			Clients.Add(new ClientState(clientId));
			AuthorizationMappings.Add(clientId, authorizationKey);

			return (clientId, authorizationKey);
		}

		public ClientState GetPlayerState(string authToken)
		{
			return Clients.Find(x => x.ClientId == authToken);
		}

		public InMemoryDataProvider()
		{
			Clients = new List<ClientState>();
			AuthorizationMappings = new Dictionary<string, string>();
		}
	}
}
