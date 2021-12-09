using Global;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Server.DataProviders
{
	public class InMemoryDataProvider : IDataProvider
	{
		private readonly List<PlayerState> Players;

		public string AddNewPlayer()
		{
			var authToken = Guid.NewGuid().ToString();
			Players.Add(new PlayerState(authToken));
			return authToken;
		}

		public PlayerState GetPlayerState(string authToken)
		{
			return Players.Find(x => x.AuthToken == authToken);
		}

		public InMemoryDataProvider()
		{
			Players = new List<PlayerState>();
		}
	}
}
