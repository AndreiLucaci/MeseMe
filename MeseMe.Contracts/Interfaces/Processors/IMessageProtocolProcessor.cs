using System.Threading.Tasks;
using MeseMe.Contracts.Interfaces.Models;

namespace MeseMe.Contracts.Interfaces.Processors
{
	public interface IMessageProtocolProcessor
	{
		Task ProcessAsync(IMessageProtocol messageProtocol);
	}
}
