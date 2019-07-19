using Microsoft.Extensions.Configuration;

namespace CosmoTablePoc.Core
{
    public class AppSettings
    {
        public string StorageConnectionString { get; set; }

        public static AppSettings LoadAppSettings()
        {
            IConfigurationRoot configRoot = new ConfigurationBuilder().AddJsonFile("Settings.json").Build();
            return configRoot.Get<AppSettings>();
        }

    }
}
