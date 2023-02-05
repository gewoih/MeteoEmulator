using MeteoEmulator.Libraries.SharedLibrary.DAO;
using MeteoEmulator.Libraries.SharedLibrary.Enums;
using MeteoEmulator.Libraries.SharedLibrary.Extensions;
using MeteoEmulator.Libraries.SharedLibrary.Models;
using MeteoEmulator.Services.MeteoDataService.DAL.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace MeteoEmulator.Services.MeteoDataService.Controllers
{
	public class MeteoDataController : Controller
	{
		private readonly ILogger<MeteoDataController> _logger;
		private readonly MeteoDataDBContext _meteoDataDbContext;
		private const int _sensorDataSmoothPeriod = 10;

		public MeteoDataController(ILogger<MeteoDataController> logger, MeteoDataDBContext meteoDataDBContext)
		{
			_logger = logger;
			_meteoDataDbContext = meteoDataDBContext;
		}

		[HttpPut]
		[Route("data")]
		public async Task SaveRegularMeteoData([FromBody] MeteoDataPackage meteoData)
		{
			var meteoDataDTO = meteoData.ToDTO(SensorDataType.Default);
			await SaveMeteoData(meteoDataDTO);
		}

		[HttpPut]
		[Route("noiseData")]
		public async Task SaveNoiseAndSmoothedMeteoData([FromBody] MeteoDataPackage meteoData)
		{
			var meteoDataDTO = meteoData.ToDTO(SensorDataType.Noise);
			await SaveMeteoData(meteoDataDTO);
			await SmoothAndSaveMeteoData(meteoDataDTO);
		}

		[HttpGet]
		[Route("getMeteoStationsStatistics")]
		public async Task<MeteoStationsStatistics> GetMeteoStationsStatistics()
		{
			var meteoStationsStataistics = new MeteoStationsStatistics
			{
				TotalMeteoStationsCount = await _meteoDataDbContext.MeteoStationsData
					.AsNoTracking()
					.Select(data => data.MeteoStationName)
					.Distinct()
					.CountAsync(),

				TotalMeteoStationsSensorsCount = await _meteoDataDbContext.SensorsData
					.AsNoTracking()
					.Select(sensor => sensor.Name)
                    .Distinct()
					.CountAsync(),

				SensorsDataCount = await _meteoDataDbContext.SensorsData
					.AsNoTracking()
                    .GroupBy(sensor => sensor.Name)
					.ToDictionaryAsync(sensor => sensor.Key, sensor => sensor.Count()),

				MeteoStationsSensorsCount = await _meteoDataDbContext.SensorsData
					.AsNoTracking()
					.Include(data => data.Package)
					.GroupBy(data => data.Package.MeteoStationName)
					.ToDictionaryAsync(data => data.Key, data => data.Count())
			};

			return meteoStationsStataistics;
		}

		[HttpGet]
		[Route("getMeteoStationCSVData")]
		public async Task<string> GetMeteoStationCSVData(string meteoStationName)
		{
			var csvStringBuilder = new StringBuilder();
			csvStringBuilder.AppendLine("PackageID;SensorName;RegularData;NoiseData;SmoothData");

			const int defaultValue = 0;

			var dataPackages = await _meteoDataDbContext.MeteoStationsData
				.Where(data => data.MeteoStationName == meteoStationName)
				.SelectMany(data => data.SensorData)
				.GroupBy(data => data.Package.PackageNumber)
				.ToDictionaryAsync(data => data.Key, data => data.ToList());

			foreach (var dataPackage in dataPackages)
			{
				var sensorsNames = dataPackage.Value.Select(v => v.Name).Distinct();

				foreach (var sensorName in sensorsNames)
				{
					var regularValue = dataPackage.Value.FirstOrDefault(v => v.Name == sensorName && v.Type == SensorDataType.Default);
                    var noiseValue = dataPackage.Value.FirstOrDefault(v => v.Name == sensorName && v.Type == SensorDataType.Noise);
                    var smoothValue = dataPackage.Value.FirstOrDefault(v => v.Name == sensorName && v.Type == SensorDataType.Smooth);

					csvStringBuilder.Append(dataPackage.Key);
					csvStringBuilder.Append(";");
					csvStringBuilder.Append(sensorName);
					csvStringBuilder.Append(";");
					csvStringBuilder.Append(regularValue is not null ? regularValue.Value : defaultValue);
                    csvStringBuilder.Append(";");
                    csvStringBuilder.Append(noiseValue is not null ? noiseValue.Value : defaultValue);
                    csvStringBuilder.Append(";");
                    csvStringBuilder.Append(smoothValue is not null ? smoothValue.Value : defaultValue);

                    csvStringBuilder.AppendLine(";");
				}
			}

			return csvStringBuilder.ToString();
		}

		private async Task SmoothAndSaveMeteoData(MeteoDataPackageDAO noiseMeteoData)
		{
			var smoothedMeteoData = new MeteoDataPackageDAO
			{
				PackageNumber = noiseMeteoData.PackageNumber,
				MeteoStationName = noiseMeteoData.MeteoStationName,
				SensorData = new List<SensorDataDAO>()
			};

			var lastNoiseMeteoDataBuffer = await _meteoDataDbContext.MeteoStationsData
				.Where(data => data.MeteoStationName == noiseMeteoData.MeteoStationName)
				.OrderByDescending(data => data.PackageNumber)
				.Take(_sensorDataSmoothPeriod)
				.SelectMany(data => data.SensorData)
				.Where(data => data.Type == SensorDataType.Noise)
				.GroupBy(data => data.Name)
				.ToDictionaryAsync(data => data.Key, data => data.ToList());

			for (int i = 0; i < noiseMeteoData.SensorData.Count; i++)
			{
				var sensorName = noiseMeteoData.SensorData[i].Name;
				
				if (lastNoiseMeteoDataBuffer.TryGetValue(sensorName, out List<SensorDataDAO> sensorValues))
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

		private async Task SaveMeteoData(MeteoDataPackageDAO meteoData)
		{
            var findedPackage = await _meteoDataDbContext.MeteoStationsData
				.Include(data => data.SensorData)
                .SingleOrDefaultAsync(data => data.MeteoStationName == meteoData.MeteoStationName && data.PackageNumber == meteoData.PackageNumber);

            var meteoStationId = meteoData.MeteoStationName;
			if (findedPackage == null)
			{
				_meteoDataDbContext.MeteoStationsData.Add(meteoData);
			}
			else
			{
				foreach (var sensor in meteoData.SensorData)
				{
					var findedSensor = findedPackage.SensorData.SingleOrDefault(data =>
						data.Package.PackageNumber == sensor.Package.PackageNumber &&
						data.Name == sensor.Name &&
						data.Type == sensor.Type);

					if (findedSensor is null)
						findedPackage.SensorData.Add(sensor);
					else
						findedSensor.Value = sensor.Value;
				}
			}

            await _meteoDataDbContext.SaveChangesAsync();
        }
	}
}
