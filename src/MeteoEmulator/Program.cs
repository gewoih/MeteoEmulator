using MeteoEmulator.DI;
using MeteoEmulator.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MeteoEmulator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args)
                .UseConsoleLifetime()
                .Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((ctx, builder) =>
            {
                builder.Sources.Clear();
            })
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services.AddOptions<Arguments>().Configure(o =>
                    {
                        o.FromArgs(args);
                    });

                    services
                        .AddStateStorage() //добавляем хранилище состояния
                        .AddHandlers();

                    services.AddHostedService<GeneratorWorker>();
                    services.AddHostedService<SenderWorker>();
                })
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                    loggingBuilder.AddConsole();
                });
        }
    }
}