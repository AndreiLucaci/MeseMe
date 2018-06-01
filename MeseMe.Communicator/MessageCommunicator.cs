using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using MeseMe.Communicator.Builder;
using MeseMe.Communicator.Constants;
using MeseMe.Communicator.Helpers;
using MeseMe.Contracts.Interfaces.Models;

namespace MeseMe.Communicator
{
	public class MessageCommunicator
	{
		public static async Task<IMessageProtocol> ReadAsync(TcpClient client)
		{
			var bytes = new byte[Handshake.Message.BufferSize];
			var messageBuilder = new MessageByteBuilder();

			var ns = client.GetStream();

			int length;
			while ((length = await ns.ReadAsync(bytes, 0, bytes.Length)) != 0)
			{
				var incommingData = new byte[length];
				Array.Copy(bytes, 0, incommingData, 0, length);

				messageBuilder.AddPart(incommingData);
			}

			var messageProtocol = messageBuilder.ToMessageProtocol();

			return messageProtocol;
		}

		public static async Task<int> WriteAsync(TcpClient client, IMessageProtocol messageProtocol)
		{
			var ns = client.GetStream();

			if (ns.CanWrite)
			{
				var bytes = MessageCoder.Encode(messageProtocol);
				await ns.WriteAsync(bytes, 0, bytes.Length);

				return bytes.Length;
			}

			return 0;
		}

		public static async Task<int> WriteAsync(TcpClient client, byte[] bytes)
		{
			var ns = client.GetStream();

			if (ns.CanWrite)
			{
				await ns.WriteAsync(bytes, 0, bytes.Length);

				return bytes.Length;
			}

			return 0;
		}

		public static async Task BroadcastAsync(TcpClient[] clients, IMessageProtocol messageProtocol)
		{
			var bytes = MessageCoder.Encode(messageProtocol);

			foreach (var client in clients)
			{
				await WriteAsync(client, bytes);
			}
		}
	}
}
