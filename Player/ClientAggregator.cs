using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Client.Players;

namespace Player
{
	class ClientAggregator
	{
		static void Main(string[] args)
		{
			// read from config file while clients to add; for single player on their own machine, will just be one human player
			// TODO: spawn a new terminal for each player? That would be really nice, even for bots
			var players = new List<BasePlayer>
			{
				new HumanPlayer(),
				new RandomPlayer(),
			};

			var playerTasks = players.Select(x =>
			{
				return Task.Run(() => x.Run());
			});

			Task.WaitAll(playerTasks.ToArray());
		}
	}
}
