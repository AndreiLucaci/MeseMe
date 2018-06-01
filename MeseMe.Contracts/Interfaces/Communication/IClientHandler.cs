using System;
using System.Threading.Tasks;
using MeseMe.Contracts.Implementations.Events;

namespace MeseMe.Server.Engine.Communication
{
	public interface IClientHandler
	{
		event EventHandler<MessageProtocolReceivedEventArgs> MessageProtocolReceived;
		Task StartListeningAsync();
	}
}