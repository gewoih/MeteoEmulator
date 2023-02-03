using MeteoEmulator.Handlers;
using MeteoEmulator.Models;
using MeteoEmulator.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MeteoEmulator
{
    public class GeneratorWorker : BackgroundService
    {
        private readonly ILogger<GeneratorWorker> _logger;
        private readonly DataGenerator _generator;
        private readonly StateStorage _stateStorage;

        public GeneratorWorker(ILogger<GeneratorWorker> logger, DataGenerator generator, StateStorage stateStorage)
        {
            _logger = logger;
            _generator = generator;
            _stateStorage = stateStorage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("GeneratorWorker started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _generator.UpdateSensorData();

                var storageData = new StateStorageData()
                {
                    SensorData = _generator.GetData(),
                    SensorDataWithNoise = _generator.GetDataWithNoise()
                };

                _stateStorage.Put(storageData);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}