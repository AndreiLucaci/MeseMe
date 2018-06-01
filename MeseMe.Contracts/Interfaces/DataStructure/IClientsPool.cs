using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Models.Users;

namespace MeseMe.Contracts.Interfaces.DataStructure
{
	public interface IClientsPool
	{
		ConcurrentDictionary<Guid, IClient> ConnectedClients { get; }
		bool Add(IClient client);
		bool Remove(IClient client);
		bool Remove(Guid id);
		IClient FindClient(Guid id);
		IEnumerable<Guid> ComputeOthersGuids(Guid id);
		IEnumerable<User> ComputeOthersAsUsers(Guid id);
		IEnumerable<User> ComputeOthersAsUsers(User user);
		IEnumerable<IClient> ComputeOthersAsClients(User user);
		IEnumerable<IClient> ComputeOthersAsClients(Guid id);
	}
}