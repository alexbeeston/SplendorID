using System;
using System.Collections.Generic;
using System.Text;

using Global;

namespace Server
{
	interface IDataProvider
	{
		/// <summary>
		/// Creates a new client
		/// </summary>
		/// <returns>The tuple (clientId, authorizationKey)</returns>
		public (string, string) AddNewClient();
		public ClientState GetPlayerState(string authToken);
	}
}
