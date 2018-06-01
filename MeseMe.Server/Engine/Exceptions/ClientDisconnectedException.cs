using System;
using System.Runtime.Serialization;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Server.Engine.Models;

namespace MeseMe.Server.Engine.Exceptions
{
	public class ClientDisconnectedException : Exception
	{
		public IClient Client;

		public ClientDisconnectedException()
		{
		}

		public ClientDisconnectedException(string message) : base(message)
		{
		}

		public ClientDisconnectedException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ClientDisconnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		public ClientDisconnectedException(IClient client, Exception innerException):
			base(client.ToString(), innerException)
		{
			Client = client;
		}
	}
}
