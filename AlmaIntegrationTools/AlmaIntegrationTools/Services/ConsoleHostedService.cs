using AlmaIntegrationTools.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AlmaIntegrationTools.Services
{
    public class ConsoleHostedService<T> : IHostedService
    {
        private readonly ILogger Logger;
        private readonly IHostApplicationLifetime AppLifetime;
        private readonly ISyncService<T> SyncService;
        private readonly string BasePath;
        private int? ExitCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="appLifetime"></param>
        /// <param name="syncService"></param>
        /// <param name="syncOptions"></param>
        public ConsoleHostedService(
            ILogger<ConsoleHostedService<T>> logger,
            IHostApplicationLifetime appLifetime,
            ISyncService<T> syncService,
            IOptions<SyncSettings> syncOptions
        )
        {
            Logger = logger;
            AppLifetime = appLifetime;
            SyncService = syncService;
            BasePath = syncOptions.Value.BasePath;
        }

        /// <summary>
        /// Execute the ISyncService.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");
            _ = AppLifetime.ApplicationStarted.Register(() =>
            {
                _ = Task.Run(() =>
                {
                    DirectoryInfo directoryInfo = new(Path.Combine(BasePath, Guid.NewGuid().ToString("N")));
                    DirectoryInfo exportDirectory = new(Path.Combine(directoryInfo.FullName, DateTime.Now.ToString("yyyyMMddhhmmss")));
                    try
                    {
                        // Setup
                        Logger.LogDebug($"Create working directory: {directoryInfo.FullName}");
                        directoryInfo.Create();
                        
                        Logger.LogDebug($"Create export directory: {exportDirectory.FullName}");
                        exportDirectory.Create();

                        // download files.
                        Logger.LogDebug("Fetch files from SFTP server");
                        IReadOnlyList<FileInfo> files = SyncService.Fetch(directoryInfo);
                        Logger.LogDebug($"Total of {files.Count} fetched from server.");

                        // process files.
                        foreach (FileInfo file in files)
                        {
                            Logger.LogDebug($"Parse files: {file.FullName}");
                            SyncService.Parse(file, exportDirectory);
                        }

                        // send files.
                        Logger.LogDebug($"Zip and send contents: {exportDirectory.FullName}");
                        SyncService.Send(exportDirectory);

                        // exit.
                        Logger.LogDebug("program exit");
                        ExitCode = 0;
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Unhandled exception!");
                        ExitCode = 1;
                    }
                    finally
                    {
                        // Stop the application once the work is done
                        if (directoryInfo.Exists)
                        {
                            directoryInfo.Delete(true);
                        }
                        AppLifetime.StopApplication();
                    }
                });
            });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Stop async process.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogDebug($"Exiting with return code: {ExitCode}");

            // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
            Environment.ExitCode = ExitCode.GetValueOrDefault(-1);
            return Task.CompletedTask;
        }

    }
}
