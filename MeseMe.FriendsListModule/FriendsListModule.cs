using System;
using MeseMe.FriendsListModule.Views;
using MeseMe.Infrastructure.Constants;
using Prism.Modularity;
using Prism.Regions;

namespace MeseMe.FriendsListModule
{
	[Module(ModuleName = ModuleNames.FriendsListModule)]
	public class FriendsListModule : IModule
	{
		private readonly IRegionManager _regionManager;

		public FriendsListModule(IRegionManager regionManager)
		{
			_regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
		}

		public void Initialize()
		{
			this._regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, typeof(FriendsListBox));
		}
	}
}
