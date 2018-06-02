namespace MeseMe.Contracts.Interfaces.Settings
{
	public interface ISettings
	{
		bool UseUri { get; set; }
		string Uri { get; set; }
		ushort Port { get; set; }
	}
}
