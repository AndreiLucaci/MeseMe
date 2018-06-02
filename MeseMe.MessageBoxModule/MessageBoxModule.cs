using System;
using MeseMe.Infrastructure.Constants;
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

		public MessageBoxModule(IRegionManager regionManager)
		{
			_regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
		}

		public void Initialize()
		{
			this._regionManager.RegisterViewWithRegion(RegionNames.ShellBottomRegion, typeof(MessageBox));
		}
	}
}
