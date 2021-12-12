using System;
using System.Net.Http;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Global.Messaging;


namespace Global
{
	public delegate void SocketHandler(string socketInput);

	/// <summary>
	/// A helper class for communicating over a socket
	/// </summary>
	public class Messenger
	{
		public Messenger(Socket socket)
		{
			Socket = socket;
			InternalBufferSize = 1024;
			BytesRequiredForHeaders = 1;
			Encoding = Encoding.UTF8;
		}

		public void SendMessage(string clientId, BasePayload payload)
		{
			var message = new Message
			{
				ClientId = clientId,
				EventCode = payload.GetType().Name,
				SerializedPayload = JsonConvert.SerializeObject(payload),
			};

			byte[] encodedPayload = Encoding.GetBytes(JsonConvert.SerializeObject(message));
			int indexOfNextWrite = 0;
			do
			{
				byte[] packet = new byte[Math.Min(InternalBufferSize, encodedPayload.Length - indexOfNextWrite + BytesRequiredForHeaders)];
				WriteHeaders(packet, encodedPayload, indexOfNextWrite);
				indexOfNextWrite = CopyDataToPacket(packet, encodedPayload, indexOfNextWrite);
				Socket.Send(packet);
			}
			while (indexOfNextWrite < encodedPayload.Length);
		}

		public Message ReceiveMessage()
		{
			var stringBuilder = new StringBuilder();
			byte[] buffer = new byte[InternalBufferSize];
			do
			{
				int numReceivedBytes = Socket.Receive(buffer);
				string message = Encoding.GetString(buffer, BytesRequiredForHeaders, numReceivedBytes - BytesRequiredForHeaders);
				stringBuilder.Append(message);
			} while (buffer[0] == 1);

			return JsonConvert.DeserializeObject<Message>(stringBuilder.ToString());
		}

		private Socket Socket { get; set; }
		private int BytesAvailableForPayload { get { return InternalBufferSize - BytesRequiredForHeaders; } }
		private readonly Encoding Encoding;
		private readonly int InternalBufferSize;
		private readonly int BytesRequiredForHeaders;

		private void WriteHeaders(byte[] packet, byte[] encodedPayload, int indexOfNextWrite)
		{
			bool isLastPacket = encodedPayload.Length - indexOfNextWrite <= BytesAvailableForPayload;
			bool readAgain = !isLastPacket;

			packet[0] = (byte)(readAgain ? 1 : 0);
		}

		private int CopyDataToPacket(byte[] packet, byte[] encodedPayload, int indexOfNextWrite)
		{
			int bytesRemaining = encodedPayload.Length - indexOfNextWrite;
			int bytesToRead = Math.Min(BytesAvailableForPayload, bytesRemaining);
			for (int i = 0; i < bytesToRead; i++)
			{
				packet[BytesRequiredForHeaders + i] = encodedPayload[indexOfNextWrite + i];
			}
			return indexOfNextWrite + bytesToRead;
		}
	}
}

/*
 * # Documentation
 * 
 * ## Headers
 * index 0: readAgain (1 for read another packet, 0 for this was the last packet)
 */


