using System;
using System.Net.Http;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Global
{
	public class Messenger
	{
		private Socket Socket { get; set; }

		public Messenger(Socket socket)
		{
			Socket = socket;
		}

		public void SendMessage(string message)
		{
			// TODO: check message length and send multiple packets if necessary
			Socket.Send(Encoding.ASCII.GetBytes(message));
		}

		public string ReceiveMessage()
		{
			// TODO: check the headers on the stream and see if we need to keep reading from the stream; concat all payloads if so
			byte[] bytes = new byte[1024];
			int numReceivedBytes = Socket.Receive(bytes);
			return Encoding.ASCII.GetString(bytes, 0, numReceivedBytes);
		}
	}
}
