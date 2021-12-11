﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	class ClientFactory
	{
		static void Main(string[] args)
		{
			BaseClient client;
			switch (args[0])
			{
				case "h":
					client = new HumanClient();
					break;
				case "b":
					client = new BotClient();
					break;
				default:
					throw new Exception("selection does not map to a type of client");
			}

			var task = Task.Run(() => client.Run());
			task.Wait();
			Console.WriteLine("Shouldn't see this");
		}
	}
}
