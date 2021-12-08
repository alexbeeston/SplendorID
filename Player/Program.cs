using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Player
{
	class Program
	{
		static void Main(string[] args)
		{
			byte[] bytes = new byte[1024];

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
				// Connect to Remote EndPoint
				sender.Connect(remoteEP);

				Console.WriteLine($"Socket connected to {sender.RemoteEndPoint}");

				// Encode the data string into a byte array.
				byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

				// Send the data through the socket.
				int bytesSent = sender.Send(msg);

				// Receive the response from the remote device.
				int bytesRec = sender.Receive(bytes);
				Console.WriteLine($"Echoed test = {Encoding.ASCII.GetString(bytes, 0, bytesRec)}");

				// Release the socket.
				sender.Shutdown(SocketShutdown.Both);
				sender.Close();

			}
			catch (ArgumentNullException e)
			{
				Console.WriteLine($"ArgumentNullException : {e}");
			}
			catch (SocketException e)
			{
				Console.WriteLine($"SocketException : {e}");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Unexpected exception : {e}");
			}
		}
	}
}
