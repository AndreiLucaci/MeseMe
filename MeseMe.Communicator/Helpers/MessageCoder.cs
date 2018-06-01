using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using MeseMe.Contracts.Implementations.Models;
using MeseMe.Contracts.Interfaces.Models;

namespace MeseMe.Communicator.Helpers
{
	public static class MessageCoder
	{
		public static byte[] Encode(IMessageProtocol messageProtocol)
		{
			var bytes = new List<byte>();

			var header = BitConverter.GetBytes(messageProtocol.Header.Type);
			var payload = ObjectToByteArray(messageProtocol.Data);

			bytes.AddRange(header);
			bytes.AddRange(payload);

			return bytes.ToArray();
		}

		public static IMessageProtocol Decode(byte[] bytes)
		{
			var header = BitConverter.ToUInt16(bytes, 0);
			var payload = ByteArrayToObject(bytes, 2, bytes.Length - 2);

			var messageProtocol = new MessageProtocol
			{
				Header = new MessageType {Type = header},
				Data = payload
			};

			return messageProtocol;
		}

		private static IEnumerable<byte> ObjectToByteArray(object obj)
		{
			if (obj == null)
			{
				return null;
			}

			var bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}

		private static object ByteArrayToObject(byte[] bytes, int startIndex, int count)
		{
			if (bytes == null || !bytes.Any())
			{
				return null;
			}

			var bf = new BinaryFormatter();
			using (var ms = new MemoryStream(bytes, startIndex, count))
			{
				var obj = bf.Deserialize(ms);
				return obj;
			}
		}
	}
}
