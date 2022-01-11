using System;

namespace Client
{
	class ClientMain
	{
		static void Main(string[] args)
		{
			BaseClient client = args[0] switch
			{
				"h" => new HumanClient(),
				"b" => new BotClient(),
				_ => throw new Exception("selection does not map to a type of client"),
			};
			try
			{
				client.Run();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.ReadLine();
			}
		}
	}
}
