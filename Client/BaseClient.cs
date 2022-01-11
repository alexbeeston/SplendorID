using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Global.Messaging;
using Global.Messaging.Payloads;
using Global.Messaging.Payloads.Init;

namespace Client
{
	public abstract class BaseClient
	{
		protected Socket Socket { get; set; }
		protected string UserName { get; set; }
		protected string ClientId { get; set; }

		public void Run()
		{
			SetSocket();
			RegisterWithServer();
			GreetClient();
			Console.WriteLine("all done, press enter to finish");
			Console.ReadLine();
		}

		private void SetSocket()
		{
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
			Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(remoteEP);
			Socket = socket;
			Console.WriteLine("Just opened socket connection");
		}

		private void RegisterWithServer()
		{
			RegisterNewClientResponse serverPayload;
			string userName;
			do
			{
				Console.WriteLine("About to collect userName");
				userName = GetUserName();
				MessagingUtils.SendMessage(Socket, new RegisterNewClientRequest
				{
					RequestedUserName = userName
				});
				var serverMessage = MessagingUtils.ReceiveMessage(Socket);
				serverPayload = MessagingUtils.Parse<RegisterNewClientResponse>(serverMessage);
				if (!serverPayload.Success && serverPayload.Error != ErrorCode.UserNameTaken)
				{
					throw new Exception($"Failed registration with code {serverPayload.Error}");
				}
			} while (!serverPayload.Success);

			UserName = userName;
			ClientId = serverPayload.ClientId;
			Console.WriteLine("Successfully registered with server");
		}

		// Abstract Methods
		protected abstract void GreetClient();
		protected abstract string GetUserName();
	}
}
