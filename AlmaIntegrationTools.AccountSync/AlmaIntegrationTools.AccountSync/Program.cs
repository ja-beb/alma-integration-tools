using AlmaIntegrationTools.AccountSync.Models;
using AlmaIntegrationTools.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AlmaIntegrationTools.AccountSync
{

    /// <summary>
    /// Main program execution.
    /// </summary>
    public class Program
    {

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
                        .AddHostedService<ConsoleHostedService<UserCollection>>()
                        .AddSingleton<ISyncService<UserCollection>, Services.SyncService>();

                    services.AddOptions<AlmaIntegrationTools.Settings.SyncSettings>().Bind(hostContext.Configuration.GetSection("Sync"));
                    services.AddOptions<Settings.SyncSettings>().Bind(hostContext.Configuration.GetSection("Sync"));
                })
                .RunConsoleAsync();
        }


    }
}
