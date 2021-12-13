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
		private readonly List<Game> Games;

		public InMemoryDataProvider()
		{
			Clients = new List<ClientState>();
			AuthorizationMappings = new Dictionary<string, string>();
			Games = new List<Game>();
		}

		public (string, string) AddNewClient(string userName)
		{
			string clientId;
			string authorizationKey;
			lock (Clients)
			{
				if (Clients.Exists(x => x.UserName == userName))
				{
					throw new UserNameNotAvailable();
				}
				clientId = Guid.NewGuid().ToString();
				authorizationKey = Guid.NewGuid().ToString();
				Clients.Add(new ClientState(clientId, userName));
			}
			AuthorizationMappings.Add(clientId, authorizationKey);
			Console.WriteLine($"Created user {userName}\n  id: {clientId}\n  auth key: {authorizationKey}");
			return (clientId, authorizationKey);
		}

		public ClientState GetPlayerState(string authToken)
		{
			lock (Clients)
			{
				return Clients.Find(x => x.ClientId == authToken);
			}
		}

		public string CreateGame()
		{
			lock (Games)
			{
				string gameId;
				bool addedGame = false;
				do
				{
					gameId = Guid.NewGuid().ToString().Substring(0, 5);
					if (Games.Find(x => x.GameId == gameId) == default)
					{
						Games.Add(new Game(gameId));
						Console.WriteLine($"Created game {gameId}");
						addedGame = true;
					}
				} while (!addedGame);

				return gameId;
			}
		}

		public void AddClientToGame(string gameId, string clientId)
		{
			lock (Games)
			{
				// TODO: validate/provide error message for games that already have four or more players or that have already started
				bool gameCanAcceptClient = true;
				if (gameCanAcceptClient)
				{
					Games.Find(x => x.GameId == gameId).ClientIds.Add(clientId);
					Console.WriteLine($"Added user to game\n  user: {clientId}\n  game: {gameId}");
				}
			}
		}
	}
}
