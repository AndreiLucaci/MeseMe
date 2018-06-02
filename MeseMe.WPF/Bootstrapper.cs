using System.Threading.Tasks;
using System.Windows;
using MeseMe.Client;
using MeseMe.WPF.Unity;
using MeseMe.WPF.Wrapper;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Unity;

namespace MeseMe.WPF
{
	public class Bootstrapper : UnityBootstrapper
	{
		protected override void ConfigureModuleCatalog()
	    {
		    base.ConfigureModuleCatalog();
			var moduleCatalog = (ModuleCatalog)ModuleCatalog;
			moduleCatalog.AddModule(typeof(MessageBoxModule.MessageBoxModule));
		    moduleCatalog.AddModule(typeof(FriendsListModule.FriendsListModule));
		    moduleCatalog.AddModule(typeof(MessageListModule.MessageListModule));
	    }

		protected override void ConfigureContainer()
		{
			base.ConfigureContainer();

			Container.WithClient().WithClientWrapper();
			var clientwrapper = Container.Resolve<ClientWrapper>();
		}

		protected override void InitializeShell()
	    {
		    base.InitializeShell();

			Container.RegisterInstance<IEventAggregator>(new EventAggregator());

		    Application.Current.MainWindow = (Shell) Shell;
		    Application.Current.MainWindow?.Show();
	    }
	}
}
