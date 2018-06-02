using MeseMe.Infrastructure.Constants;
using MeseMe.MessageListModule.Views;
using Prism.Modularity;
using Prism.Regions;

namespace MeseMe.MessageListModule
{
	[Module(ModuleName = ModuleNames.MessageListModule)]
	public class MessageListModule : IModule
	{
		private readonly IRegionManager _regionManager;

		public MessageListModule(IRegionManager regionManager)
		{
			_regionManager = regionManager;
		}

		public void Initialize()
		{
			this._regionManager.RegisterViewWithRegion(RegionNames.ShellMiddleRegion, typeof(MessageListBox));
		}
	}
}
