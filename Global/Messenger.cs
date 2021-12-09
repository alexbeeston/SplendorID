﻿using System;
using System.Net.Http;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Global
{
	public class Messenger
	{
		private Socket Socket { get; set; }
		private int BytesAvailableForPayload { get { return InternalBufferSize - BytesRequiredForHeaders; } }

		private readonly Encoding Encoding;
		private readonly int InternalBufferSize;
		private readonly int BytesRequiredForHeaders;

		public Messenger(Socket socket)
		{
			Socket = socket;
			InternalBufferSize = 1024;
			BytesRequiredForHeaders = 1;
			Encoding = Encoding.UTF8;
		}

		public void SendPayload(string payload)
		{
			byte[] encodedPayload = Encoding.GetBytes(payload);
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

		public string ReceivePayload()
		{
			var stringBuilder = new StringBuilder();
			byte[] buffer = new byte[InternalBufferSize];
			do
			{
				int numReceivedBytes = Socket.Receive(buffer);
				string payload = Encoding.GetString(buffer, BytesRequiredForHeaders, numReceivedBytes - BytesRequiredForHeaders);
				stringBuilder.Append(payload);
			} while (buffer[0] == 1);

			return stringBuilder.ToString();
		}

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

