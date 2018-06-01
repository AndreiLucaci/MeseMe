using MeseMe.Contracts.Interfaces.Communication;
using MeseMe.Contracts.Interfaces.DataStructure;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Server.Engine.Communication;
using MeseMe.Server.Engine.DataStructure;
using MeseMe.Server.Engine.Processors;
using Unity;
using Unity.Injection;

namespace MeseMe.ServerRunner.UnitiConfiguration
{
	public static class ServerContainerConfiguration
	{
		public static void ConfigureServer(IUnityContainer container)
		{
			container.RegisterSingleton<IClientsPool, MeseClientsPool>();

			container.RegisterType<IClientService, MeseClientService>();

			ConfigureMessageProtocolProcessor(container);

			container.RegisterType<Server.Server>(
				new InjectionConstructor(
					new ResolvedParameter<IClientService>(),
					new ResolvedParameter<IMessageProtocolProcessor>(),
					container.Resolve<IClientsPool>()
				)
			);
		}

		private static void ConfigureMessageProtocolProcessor(IUnityContainer container)
		{
			container.RegisterInstance<IMessageProtocolProcessor>(
				new MeseMessageProtocolProcessorDecorator(
					new DoNothingMessageProtocolProcessor(),
					container.Resolve<IClientsPool>()
				)
			);
		}
	}
}
