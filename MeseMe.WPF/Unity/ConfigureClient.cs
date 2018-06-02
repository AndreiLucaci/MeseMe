using MeseMe.Client.Engine.Notifier;
using MeseMe.Client.Engine.Processors;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.WPF.Wrapper;
using Microsoft.Practices.Unity;
using Prism.Events;

namespace MeseMe.WPF.Unity
{
	public static class ConfigureClient
	{
		public static IUnityContainer WithClient(this IUnityContainer container)
		{
			WithNotifier(container);
			WithMessageProcessors(container);

			container.RegisterType<Client.Client>(
				new InjectionConstructor(
					container.Resolve<ITwoWayNotifier>(),
					container.Resolve<IMessageProtocolProcessor>()
				)
			);

			return container;
		}

		public static IUnityContainer WithClientWrapper(this IUnityContainer container)
		{
			container.RegisterInstance(new ClientWrapper(
					container.Resolve<IEventAggregator>(),
					container.Resolve<Client.Client>()
				),
				new ContainerControlledLifetimeManager()
			);

			return container;
		}

		private static void WithNotifier(IUnityContainer container)
		{
			container.RegisterInstance(typeof(ITwoWayNotifier), new TwoWayNotifier(), new ContainerControlledLifetimeManager());
		}

		private static void WithMessageProcessors(IUnityContainer container)
		{
			INotifierActions notifier = container.Resolve<ITwoWayNotifier>();

			container.RegisterInstance<IMessageProtocolProcessor>(
				new ClientConnectedMessageProtocolProcessDecorator(
					new ClientDisconnectedMessageProtocolProcessDecorator(
						new MessageReceivedMessageProtocolProcessorDecorator(
							new ConnectionEstablishedMessageProtocolProcessorDecorator(
								new DoNothingMessageProtocolProcessor(
									notifier),
								notifier),
							notifier),
						notifier),
					notifier)
			);
		}
	}
}
