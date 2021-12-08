﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Global;

namespace Player
{
	class PlayerMain
	{
		static void Main(string[] args)
		{
			// Connect to a Remote server
			// Get Host IP Address that is used to establish a connection
			// In this case, we get one IP address of localhost that is IP : 127.0.0.1
			// If a host has multiple addresses, you will get a list of addresses
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

			// Create a TCP/IP  socket.
			Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

			// Connect the socket to the remote endpoint. Catch any errors.
			try
			{
				sender.Connect(remoteEP);
				var messenger = new Messenger(sender);
				Console.WriteLine($">> Socket connected to {sender.RemoteEndPoint}");
				while (true)
				{
					messenger.SendMessage(Console.ReadLine());
					Console.WriteLine(messenger.ReceiveMessage());
				}

				// Release the socket.
				sender.Shutdown(SocketShutdown.Both);
				sender.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
