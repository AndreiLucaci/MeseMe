using MeseMe.Client.Engine.Notifier;
using MeseMe.Client.Engine.Processors;
using MeseMe.Contracts.Interfaces.Processors;
using Unity;
using Unity.Injection;

namespace MeseMe.ConsoleClientRunner.UnityConfiguration
{
	public static class ClientContainerConfiguration
	{
		public static void ConfigureClient(IUnityContainer container)
		{
			ConfigureNotifier(container);
			ConfigureMessageProcessors(container);

			container.RegisterType<Client.Client>(
				new InjectionConstructor(
					container.Resolve<ITwoWayNotifier>(),
					container.Resolve<IMessageProtocolProcessor>()
				)
			);
		}

		private static void ConfigureNotifier(IUnityContainer container)
		{
			container.RegisterSingleton<ITwoWayNotifier, TwoWayNotifier>();
		}

		private static void ConfigureMessageProcessors(IUnityContainer container)
		{
			INotifierActions notifier = container.Resolve<ITwoWayNotifier>();

			container.RegisterInstance<IMessageProtocolProcessor>(
				new ClientDisconnectedMessageProtocolProcessDecorator(
					new MessageReceivedMessageProtocolProcessorDecorator(
						new ConnectionEstablishedMessageProtocolProcessorDecorator(
							new DoNothingMessageProtocolProcessor(
								notifier),
							notifier),
						notifier),
					notifier
				)
			);
		}
	}
}
