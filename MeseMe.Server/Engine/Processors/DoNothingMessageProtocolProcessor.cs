using System.Threading.Tasks;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Contracts.Interfaces.Processors;

namespace MeseMe.Server.Engine.Processors
{
	public class DoNothingMessageProtocolProcessor : IMessageProtocolProcessor
	{
		public Task ProcessAsync(IMessageProtocol messageProtocol)
		{
			return Task.CompletedTask;
		}
	}
}
