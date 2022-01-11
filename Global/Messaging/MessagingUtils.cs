using System;
using System.Net.Sockets;
using System.Text;

using Newtonsoft.Json;

namespace Global.Messaging
{
	public static class MessagingUtils
	{
		public static void SendMessage(Socket socket, BasePayload payload)
		{
			var message = new Message
			{
				PayloadType = payload.GetType().Name,
				SerializedPayload = JsonConvert.SerializeObject(payload),
			};

			byte[] encodedPayload = Encoding.GetBytes(JsonConvert.SerializeObject(message));
			int indexOfNextWrite = 0;
			do
			{
				byte[] packet = new byte[Math.Min(InternalBufferSize, encodedPayload.Length - indexOfNextWrite + BytesRequiredForHeaders)];
				WriteHeaders(packet, encodedPayload, indexOfNextWrite);
				indexOfNextWrite = CopyDataToPacket(packet, encodedPayload, indexOfNextWrite);
				socket.Send(packet);
			}
			while (indexOfNextWrite < encodedPayload.Length);
		}

		public static Message ReceiveMessage(Socket socket)
		{
			var stringBuilder = new StringBuilder();
			byte[] buffer = new byte[InternalBufferSize];
			do
			{
				int numReceivedBytes = socket.Receive(buffer);
				string message = Encoding.GetString(buffer, BytesRequiredForHeaders, numReceivedBytes - BytesRequiredForHeaders);
				stringBuilder.Append(message);
			} while (buffer[0] == 1);

			return JsonConvert.DeserializeObject<Message>(stringBuilder.ToString());
		}

		public static T Parse<T>(Message message)
		{
			return JsonConvert.DeserializeObject<T>(message.SerializedPayload); // TODO: set mesage ID as a field in the message and just serialize the whole thing; they're all messages now
		}

		private static int BytesAvailableForPayload { get { return InternalBufferSize - BytesRequiredForHeaders; } }
		private static readonly Encoding Encoding = Encoding.UTF8;
		private static readonly int InternalBufferSize = 2048;
		private static readonly int BytesRequiredForHeaders = 1;

		private static void WriteHeaders(byte[] packet, byte[] encodedPayload, int indexOfNextWrite)
		{
			bool isLastPacket = encodedPayload.Length - indexOfNextWrite <= BytesAvailableForPayload;
			bool readAgain = !isLastPacket;

			packet[0] = (byte)(readAgain ? 1 : 0);
		}

		private static int CopyDataToPacket(byte[] packet, byte[] encodedPayload, int indexOfNextWrite)
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
