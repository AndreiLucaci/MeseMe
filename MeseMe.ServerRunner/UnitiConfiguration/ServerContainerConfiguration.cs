using MeseMe.Contracts.Interfaces.Communication;
using MeseMe.Contracts.Interfaces.DataStructure;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Contracts.Interfaces.Settings;
using MeseMe.Server.Engine.Communication;
using MeseMe.Server.Engine.DataStructure;
using MeseMe.Server.Engine.Processors;
using Unity;
using Unity.Injection;

namespace MeseMe.ServerRunner.UnitiConfiguration
{
	public static class ServerContainerConfiguration
	{
		public static IUnityContainer WithServer(this IUnityContainer container)
		{
			container.RegisterSingleton<IClientsPool, MeseClientsPool>();

			container.RegisterType<IClientService, MeseClientService>();

			ConfigureMessageProtocolProcessor(container);

			container.RegisterType<Server.Server>(
				new InjectionConstructor(
					new ResolvedParameter<IClientService>(),
					new ResolvedParameter<IMessageProtocolProcessor>(),
					container.Resolve<IClientsPool>(),
					new ResolvedParameter<ISettings>()
				)
			);

			return container;
		}

		public static IUnityContainer WithSettings(this IUnityContainer container)
		{
			container.RegisterType<ISettings, Settings.Settings>();

			return container;
		}

		private static void ConfigureMessageProtocolProcessor(IUnityContainer container)
		{
			var clienstPool = container.Resolve<IClientsPool>();

			container.RegisterInstance<IMessageProtocolProcessor>(
				new MeseDisconnectMessageProtocolDecorator(
					new MeseMessageProtocolProcessorDecorator(
						new DoNothingMessageProtocolProcessor(),
						clienstPool
					),
					clienstPool
				)
			);
		}
	}
}
