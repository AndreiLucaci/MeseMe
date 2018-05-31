using System.Windows;
using Prism.Modularity;
using Prism.Unity;

namespace MeseMe.WPF
{
	public class Bootstrapper : UnityBootstrapper
    {
	    //protected override DependencyObject CreateShell()
	    //{
		   // return new Shell();
	    //}

	    protected override IModuleCatalog CreateModuleCatalog()
	    {
		    return base.CreateModuleCatalog();
		    //var moduleCatalog = (ModuleCatalog) ModuleCatalog;
		    //moduleCatalog.AddModule();
	    }

	    protected override void InitializeShell()
	    {
		    base.InitializeShell();

		    Application.Current.MainWindow = (Shell) Shell;
		    Application.Current.MainWindow?.Show();
	    }
    }
}
