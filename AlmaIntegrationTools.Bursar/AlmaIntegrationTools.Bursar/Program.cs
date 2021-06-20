using AlmaIntegrationTools.Bursar.Models;
using AlmaIntegrationTools.Bursar.Services;
using AlmaIntegrationTools.Services;
using AlmaIntegrationTools.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AlmaIntegrationTools.Bursar
{

    /// <summary>
    /// Main program execution. 
    /// </summary>
    class Program
    {
        /// <summary>
        ///  Main program execution.
        /// </summary>
        private static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseContentRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .ConfigureLogging(logging =>
                {
                    // Add any 3rd party loggers like NLog or Serilog
                })
                .ConfigureServices((hostContext, services) =>
                {
                    _ = services
                        .AddHostedService<ConsoleHostedService<BursarFeed>>()
                        .AddSingleton<ISyncService<BursarFeed>, SyncService>();
                    services.AddOptions<SyncSettings>().Bind(hostContext.Configuration.GetSection("Sync"));
                })
                .RunConsoleAsync();
        }
    }
}
