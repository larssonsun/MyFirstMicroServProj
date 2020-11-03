using System.IO;
using Microsoft.Extensions.Configuration;

namespace Client.Con
{
    public interface IConfiguration
    {
        string GetVal(string key);
    }

    public class Configuration : IConfiguration
    {
        private readonly IConfigurationRoot _configuration;
        public Configuration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        public string GetVal(string key)
        {
            return _configuration[key];
        }
    }
}