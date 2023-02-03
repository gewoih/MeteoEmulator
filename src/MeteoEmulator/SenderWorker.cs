using MeteoEmulator.Handlers;
using MeteoEmulator.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace MeteoEmulator
{
    public class SenderWorker : BackgroundService
    {
        private readonly ILogger<SenderWorker> _logger;
        private readonly DataSender _sender;
        private readonly StateStorage _stateStorage;

        private readonly int _sleepDelay;

        public SenderWorker(ILogger<SenderWorker> logger, DataSender sender, StateStorage stateStorage, IOptions<Arguments> argOptions)
        {
            _logger = logger;
            _sender = sender;
            _stateStorage = stateStorage;
            _sleepDelay = argOptions.Value.SleepInterval < 1 ? 1 : argOptions.Value.SleepInterval;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SenderWorker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                var data = _stateStorage.Get();

                if (data != null)
                {
                    _logger.LogDebug("Sending data...");

                    var dataSent = await _sender.Send(data.SensorData, false, stoppingToken);
                    var noiseDataSent = await _sender.Send(data.SensorDataWithNoise, true, stoppingToken);

                    _logger.LogDebug(dataSent && noiseDataSent ? "Data sent successfully" : "Unable to send data");
                }

                await Task.Delay(1000 * _sleepDelay, stoppingToken);
            }
        }
    }
}