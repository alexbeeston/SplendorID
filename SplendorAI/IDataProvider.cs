using System;
using System.Collections.Generic;
using System.Text;

using Global;

namespace Server
{
	interface IDataProvider
	{
		/// <summary>
		/// Creates a new player and returns the auth token of the newly created player
		/// </summary>
		/// <returns></returns>
		public string AddNewPlayer();
		public PlayerState GetPlayerState(string authToken);
	}
}
