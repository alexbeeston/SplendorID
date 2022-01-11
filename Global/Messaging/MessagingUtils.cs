using System;
using System.Net.Sockets;
using System.Text;

using Newtonsoft.Json;

namespace Global.Messaging
{
	public static class MessagingUtils
	{
		public static void SendMessage(Socket socket, BaseMessage message)
		{
			byte[] encodedMessage = Encoding.GetBytes(JsonConvert.SerializeObject(message));
			int indexOfNextWrite = 0;
			do
			{
				byte[] packet = new byte[Math.Min(InternalBufferSize, encodedMessage.Length - indexOfNextWrite + BytesRequiredForHeaders)];
				WriteHeaders(packet, encodedMessage, indexOfNextWrite);
				indexOfNextWrite = CopyDataToPacket(packet, encodedMessage, indexOfNextWrite);
				socket.Send(packet);
			}
			while (indexOfNextWrite < encodedMessage.Length);
		}

		public static T ReceiveMessage<T>(Socket socket)
		{
			var stringBuilder = new StringBuilder();
			byte[] buffer = new byte[InternalBufferSize];
			do
			{
				int numReceivedBytes = socket.Receive(buffer);
				string message = Encoding.GetString(buffer, BytesRequiredForHeaders, numReceivedBytes - BytesRequiredForHeaders);
				stringBuilder.Append(message);
			} while (buffer[0] == 1);

			return JsonConvert.DeserializeObject<T>(stringBuilder.ToString());
		}

		private static int BytesAvailableForMessage { get { return InternalBufferSize - BytesRequiredForHeaders; } }
		private static readonly Encoding Encoding = Encoding.UTF8;
		private static readonly int InternalBufferSize = 2048;
		private static readonly int BytesRequiredForHeaders = 1;

		private static void WriteHeaders(byte[] packet, byte[] encodedPayload, int indexOfNextWrite)
		{
			bool isLastPacket = encodedPayload.Length - indexOfNextWrite <= BytesAvailableForMessage;
			bool readAgain = !isLastPacket;

			packet[0] = (byte)(readAgain ? 1 : 0);
		}

		private static int CopyDataToPacket(byte[] packet, byte[] encodedMessage, int indexOfNextWrite)
		{
			int bytesRemaining = encodedMessage.Length - indexOfNextWrite;
			int bytesToRead = Math.Min(BytesAvailableForMessage, bytesRemaining);
			for (int i = 0; i < bytesToRead; i++)
			{
				packet[BytesRequiredForHeaders + i] = encodedMessage[indexOfNextWrite + i];
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
