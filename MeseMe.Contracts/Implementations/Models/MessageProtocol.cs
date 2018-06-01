using MeseMe.Contracts.Interfaces.Models;

namespace MeseMe.Contracts.Implementations.Models
{
	public class MessageProtocol : IMessageProtocol
	{
		public MessageType Header { get; set; }

		public object Data { get; set; }

		public T GetDataAs<T>() where T: class
		{
			return (T) Data;
		}
	}
}
