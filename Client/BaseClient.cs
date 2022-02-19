using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Global.Messaging;
using Global.Messaging.Messages;
using Global.Types;

namespace Client
{
	public abstract class BaseClient : AccessibleClient
	{
		public void Run()
		{
			SetSocket();
			RegisterWithServer();
			GreetClient();
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
		}

		private void RegisterWithServer()
		{
			RegisterNewClientResponse response;
			string userName;
			do
			{
				userName = GetUserName();
				MessagingUtils.SendMessage(Socket, new RegisterNewClientRequest
				{
					RequestedUserName = userName
				});
				response = MessagingUtils.ReceiveMessage<RegisterNewClientResponse>(Socket);
				if (!response.Success && response.Error != ErrorCode.UserNameTaken)
				{
					throw new Exception($"Failed registration with code {response.Error}");
				}
			} while (!response.Success);

			UserName = userName;
			ClientId = response.ClientId;
			Console.WriteLine($"{UserName}: Just connected to server");
		}

		// Abstract Methods
		protected abstract void GreetClient();
		protected abstract string GetUserName();
	}
}
