using MeseMe.Infrastructure.Constants;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace MeseMe.MessageBoxModule
{
	/// <summary>
	/// Register components of module with UnityPrism
	/// </summary>
	[Module(ModuleName = ModuleNames.MessageBoxModule)]
	public class MessageBoxModule : IModule
	{
		private readonly IRegionManager _regionManager;
		private readonly IUnityContainer _container;

		public MessageBoxModule(IRegionManager regionManager, IUnityContainer container)
		{
			_regionManager = regionManager;
			_container = container;
		}

		public void Initialize()
		{
			this._regionManager.RegisterViewWithRegion(RegionNames.ShellBottomRegion, typeof(MessageBox));
		}
	}
}
