using System.Linq;
using MeseMe.Communicator.Helpers;
using MeseMe.Contracts.Interfaces.Models;

namespace MeseMe.Communicator.Builder
{
	public class MessageByteBuilder
	{
		public byte[] Bytes;

		public void AddPart(byte[] bytes)
		{
			if (bytes != null && bytes.Length != 0)
			{
				Append(bytes);
			}
		}

		private void Append(byte[] array)
		{
			var rv = new byte[Bytes.Length + array.Length];
			var offset = 0;

			System.Buffer.BlockCopy(Bytes, 0, rv, offset, Bytes.Length);
			offset += Bytes.Length;
			System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);

			Bytes = rv;
		}

		public IMessageProtocol ToMessageProtocol()
		{
			return MessageCoder.Decode(Bytes.ToArray());
		}
	}
}
