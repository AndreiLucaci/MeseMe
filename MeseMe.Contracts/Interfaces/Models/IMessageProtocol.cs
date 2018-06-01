using MeseMe.Contracts.Implementations.Models;

namespace MeseMe.Contracts.Interfaces.Models
{
	public interface IMessageProtocol
	{
		MessageType Header { get; set; }
		object Data { get; set; }
		T GetDataAs<T>() where T: class;
	}
}