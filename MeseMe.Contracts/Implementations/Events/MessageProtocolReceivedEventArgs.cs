using System;
using MeseMe.Contracts.Interfaces.Models;

namespace MeseMe.Contracts.Implementations.Events
{
	public class MessageProtocolReceivedEventArgs : EventArgs
	{
		public IMessageProtocol MessageProtocol { get; }

		public MessageProtocolReceivedEventArgs(IMessageProtocol messageProtocol)
		{
			MessageProtocol = messageProtocol;
		}
	}
}
