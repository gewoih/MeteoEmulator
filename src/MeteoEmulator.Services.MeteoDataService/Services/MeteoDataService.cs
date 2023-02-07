using MeteoEmulator.Libraries.SharedLibrary.DAO;
using MeteoEmulator.Libraries.SharedLibrary.Enums;
using MeteoEmulator.Libraries.SharedLibrary.Extensions;
using MeteoEmulator.Libraries.SharedLibrary.Models;
using MeteoEmulator.Services.MeteoDataService.DAL.DBContexts;
using MeteoEmulator.Services.MeteoDataService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MeteoEmulator.Services.MeteoDataService.Services
{
    public class MeteoDataService : IMeteoDataService
    {
        private readonly MeteoDataDBContext _meteoDataDBContext;
        private readonly ILogger<MeteoDataService> _logger;

        public MeteoDataService(ILogger<MeteoDataService> logger, MeteoDataDBContext meteoDataDBContext)
        {
            _logger = logger;
            _meteoDataDBContext = meteoDataDBContext;
        }

        public async Task SaveDefaultMeteoDataAsync(MeteoDataPackage package)
        {
            var meteoDataDAO = package.ToDAO(SensorDataType.Default);

            _logger.LogInformation($"Saving regular meteo data... ({package})");
            await SaveMeteoData(meteoDataDAO);
        }

        public async Task SaveNoiseMeteoDataAsync(MeteoDataPackage package)
        {
            var meteoDataDAO = package.ToDAO(SensorDataType.Noise);

            _logger.LogInformation($"Saving noise meteo data... ({package})");
            await SaveMeteoData(meteoDataDAO);
        }

        public async Task SaveSmoothMeteoDataAsync(MeteoDataPackage package, int smoothPeriod)
        {
            var noisePackage = package.ToDAO(SensorDataType.Noise);

            _logger.LogInformation($"Saving smoothed meteo data... ({package})");

            var smoothedMeteoData = new MeteoDataPackageDAO
            {
                PackageNumber = noisePackage.PackageNumber,
                MeteoStationName = noisePackage.MeteoStationName,
                SensorData = new List<SensorDataDAO>()
            };

            var lastNoiseMeteoDataBuffer = await _meteoDataDBContext.MeteoStationsData
                .AsNoTracking()
                .Where(data => data.MeteoStationName == noisePackage.MeteoStationName)
                .OrderByDescending(data => data.PackageNumber)
                .Take(smoothPeriod)
                .SelectMany(data => data.SensorData)
                .Where(data => data.Type == SensorDataType.Noise)
                .GroupBy(data => data.Name)
                .ToDictionaryAsync(data => data.Key, data => data.ToList());

            for (int i = 0; i < noisePackage.SensorData.Count; i++)
            {
                var sensorName = noisePackage.SensorData[i].Name;

                if (lastNoiseMeteoDataBuffer.TryGetValue(sensorName, out List<SensorDataDAO>? sensorValues))
                {
                    smoothedMeteoData.SensorData.Add(
                        new SensorDataDAO
                        {
                            Package = smoothedMeteoData,
                            Type = SensorDataType.Smooth,
                            Name = sensorValues.First().Name,
                            Value = sensorValues.Average(value => value.Value)
                        });
                }
            }

            await SaveMeteoData(smoothedMeteoData);
        }

        public async Task<string> GetMeteoStationCSVDataAsync(string meteoStationName)
        {
            _logger.LogInformation($"Forming CSV data for meteo station '{meteoStationName}'...");

            var csvStringBuilder = new StringBuilder();
            csvStringBuilder.AppendLine("PackageID;SensorName;RegularData;NoiseData;SmoothData");

            const int defaultValue = 0;

            var dataPackages = await _meteoDataDBContext.MeteoStationsData
                .AsNoTracking()
                .Where(data => data.MeteoStationName == meteoStationName)
                .SelectMany(data => data.SensorData)
                .GroupBy(data => data.Package.PackageNumber)
                .ToDictionaryAsync(data => data.Key, data => data.ToList());

            foreach (var dataPackage in dataPackages)
            {
                var sensorsNames = dataPackage.Value
                    .Select(v => v.Name)
                    .Distinct();

                foreach (var sensorName in sensorsNames)
                {
                    var regularValue = dataPackage.Value.FirstOrDefault(v => v.Name == sensorName && v.Type == SensorDataType.Default);
                    var noiseValue = dataPackage.Value.FirstOrDefault(v => v.Name == sensorName && v.Type == SensorDataType.Noise);
                    var smoothValue = dataPackage.Value.FirstOrDefault(v => v.Name == sensorName && v.Type == SensorDataType.Smooth);

                    csvStringBuilder.Append(dataPackage.Key);
                    csvStringBuilder.Append(';');
                    csvStringBuilder.Append(sensorName);
                    csvStringBuilder.Append(';');
                    csvStringBuilder.Append(regularValue is not null ? regularValue.Value : defaultValue);
                    csvStringBuilder.Append(';');
                    csvStringBuilder.Append(noiseValue is not null ? noiseValue.Value : defaultValue);
                    csvStringBuilder.Append(';');
                    csvStringBuilder.Append(smoothValue is not null ? smoothValue.Value : defaultValue);

                    csvStringBuilder.AppendLine(";");
                }
            }

            _logger.LogInformation($"CSV data for meteo station '{meteoStationName}' was formed. ({csvStringBuilder})");

            return csvStringBuilder.ToString();
        }

        public async Task<MeteoStationsStatistics> GetMeteoStationsStatisticsAsync()
        {
            _logger.LogInformation("Forming statistics for all meteo stations...");

            var meteoStationsStataistics = new MeteoStationsStatistics();

            meteoStationsStataistics.SensorsDataCount = await _meteoDataDBContext.SensorsData
                .AsNoTracking()
                .GroupBy(sensor => sensor.Name)
                .ToDictionaryAsync(sensor => sensor.Key, sensor => sensor.Count());

            meteoStationsStataistics.MeteoStationsSensorsCount = await _meteoDataDBContext.SensorsData
                .AsNoTracking()
                .Include(sensor => sensor.Package)
                .GroupBy(sensor => sensor.Package.MeteoStationName)
                .ToDictionaryAsync(sensor => sensor.Key, sensor => sensor.Count());

            meteoStationsStataistics.TotalMeteoStationsSensorsCount = meteoStationsStataistics.SensorsDataCount.Keys.Count;
            meteoStationsStataistics.TotalMeteoStationsCount = meteoStationsStataistics.MeteoStationsSensorsCount.Keys.Count;

            _logger.LogInformation($"Statistics for all meteo stations was formed ({meteoStationsStataistics})");

            return meteoStationsStataistics;
        }

        private async Task SaveMeteoData(MeteoDataPackageDAO meteoData)
        {
            _logger.LogInformation($"Saving meteo data ({meteoData})");

            var findedPackage = await _meteoDataDBContext.MeteoStationsData
                .Include(data => data.SensorData)
                .FirstOrDefaultAsync(data => 
                    data.MeteoStationName == meteoData.MeteoStationName && 
                    data.PackageNumber == meteoData.PackageNumber);

            if (findedPackage == null)
            {
                _meteoDataDBContext.MeteoStationsData.Add(meteoData);
            }
            else
            {
                foreach (var sensor in meteoData.SensorData)
                {
                    var findedSensor = findedPackage.SensorData.FirstOrDefault(data =>
                        data.Name == sensor.Name &&
                        data.Type == sensor.Type);

                    if (findedSensor is null)
                        findedPackage.SensorData.Add(sensor);
                    else
                        findedSensor.Value = sensor.Value;
                }
            }

            await _meteoDataDBContext.SaveChangesAsync();

            _logger.LogInformation($"Meteo data was saved.");
        }
    }
}
