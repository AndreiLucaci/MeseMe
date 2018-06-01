using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MeseMe.Contracts.Interfaces.DataStructure;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Models.Users;

namespace MeseMe.Server.Engine.DataStructure
{
	public class MeseClientsPool : IClientsPool
	{
		public ConcurrentDictionary<Guid, IClient> ConnectedClients { get; } = new ConcurrentDictionary<Guid, IClient>();

		public bool Add(IClient client)
		{
			return client != null && ConnectedClients.TryAdd(client.User.Id, client);
		}

		public bool Remove(IClient client)
		{
			return client != null && ConnectedClients.TryRemove(client.User.Id, out client);
		}

		public bool Remove(Guid id)
		{
			return ConnectedClients.TryRemove(id, out var _);
		}

		public IClient FindClient(Guid id)
		{
			return ConnectedClients.TryGetValue(id, out var meseClient) ? meseClient : null;
		}

		public IEnumerable<Guid> ComputeOthersGuids(Guid id)
		{
			return ComputeOthersAsUsers(id).Select(i => i.Id);
		}

		public IEnumerable<User> ComputeOthersAsUsers(Guid id)
		{
			return ConnectedClients.Where(i => i.Key != id).Select(i => i.Value.User);
		}

		public IEnumerable<User> ComputeOthersAsUsers(User user)
		{
			return ComputeOthersAsUsers(user.Id);
		}

		public IEnumerable<IClient> ComputeOthersAsClients(User user)
		{
			return ComputeOthersAsClients(user.Id);
		}

		public IEnumerable<IClient> ComputeOthersAsClients(Guid id)
		{
			return ConnectedClients.Where(i => i.Key != id).Select(i => i.Value);
		}
	}
}
