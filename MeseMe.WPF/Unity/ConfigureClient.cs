using MeseMe.Client.Engine.Notifier;
using MeseMe.Client.Engine.Processors;
using MeseMe.Contracts.Interfaces.Processors;
using MeseMe.Contracts.Interfaces.Settings;
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
			container.WithSettings();

			container.RegisterInstance(new ClientWrapper(
					container.Resolve<IEventAggregator>(),
					container.Resolve<Client.Client>(),
					container.Resolve<ISettings>()
				),
				new ContainerControlledLifetimeManager()
			);

			return container;
		}

		public static IUnityContainer WithSettings(this IUnityContainer container)
		{
			container.RegisterType<ISettings, Settings.Settings>();

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
