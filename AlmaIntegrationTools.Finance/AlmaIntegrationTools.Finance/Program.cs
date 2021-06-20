using AlmaIntegrationTools.Services;
using AlmaIntegrationTools.Settings;
using AlmaIntergrationTools.Finance.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AlmaIntergrationTools.Finance
{
    class Program
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
                        .AddHostedService<ConsoleHostedService<PaymentData>>()
                        .AddSingleton<ISyncService<PaymentData>, AlmaIntegrationTools.Bursar.Services.SyncService>();
                    services.AddOptions<SyncSettings>().Bind(hostContext.Configuration.GetSection("Sync"));
                })
                .RunConsoleAsync();
        }
    }
}
