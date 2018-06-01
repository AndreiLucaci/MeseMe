using System.Threading.Tasks;
using MeseMe.Contracts.Interfaces.Models;
using MeseMe.Server.Engine.Communication;

namespace MeseMe.Contracts.Interfaces.Communication
{
	public interface IClientService
	{
		Task<IClientHandler> CompleteHandshakeAsync(IClient meseClient);
	}
}