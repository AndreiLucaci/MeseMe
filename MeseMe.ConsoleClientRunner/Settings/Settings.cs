using System;
using System.Configuration;
using MeseMe.Contracts.Interfaces.Settings;

namespace MeseMe.ConsoleClientRunner.Settings
{
	public class Settings : ISettings
	{
		public bool UseUri { get; set; } = Convert.ToBoolean(ConfigurationManager.AppSettings["UseUri"]);
		public string Uri { get; set; } = ConfigurationManager.AppSettings["Uri"];
		public ushort Port { get; set; } = Convert.ToUInt16(ConfigurationManager.AppSettings["Port"]);
	}
}
