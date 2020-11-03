using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Client.Con
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddTransient<ILoggerFactory, LoggerFactory>();
            services.AddTransient<IScetiService, ScetiFromOcelotService>(); // ScetiService
            services.AddTransient<IConfiguration, Configuration>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var scetiService = serviceProvider.GetService<IScetiService>();

            // get service from consul 
            // scetiService.GetServices();

            while (true)
            {
                await scetiService.CreateConsign();
                var result = await scetiService.GetConsigns();
                Console.WriteLine(result);
                var s = Console.ReadLine();
                if (s == "exit")
                    break;
            }
        }
    }
}
