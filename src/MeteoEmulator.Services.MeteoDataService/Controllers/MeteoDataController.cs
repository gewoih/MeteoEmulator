using MeteoEmulator.Libraries.SharedLibrary.DTO;
using MeteoEmulator.Libraries.SharedLibrary.Enums;
using MeteoEmulator.Libraries.SharedLibrary.Extensions;
using MeteoEmulator.Libraries.SharedLibrary.Models;
using MeteoEmulator.Services.MeteoDataService.DAL.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
		//[Route("data")]
		public async Task SaveRegularMeteoData([FromBody] MeteoDataPackage meteoData)
		{
			var meteoDataDTO = meteoData.ToDTO(SensorDataType.Default);
			var meteoStationName = meteoDataDTO.MeteoStationName;

			_logger.LogInformation($"Regular meteo data received from meteo station '{meteoStationName}'.");

			var findedPackage = await _meteoDataDbContext.MeteoStationsData
				.Include(data => data.SensorData)
				.SingleOrDefaultAsync(data => data.Equals(meteoDataDTO));

			if (findedPackage == null) 
			{
				_meteoDataDbContext.MeteoStationsData.Add(meteoDataDTO);

				_logger.LogInformation($"New meteo station '{meteoStationName}' added with {meteoDataDTO.SensorData.Count} regular sensors data.");
			}
			else
			{
				findedPackage.SensorData = meteoDataDTO.SensorData;

				_logger.LogInformation($"Regular sensors data updated for meteo station {meteoStationName}.");
			}

			await _meteoDataDbContext.SaveChangesAsync();
		}

		[HttpPut]
		[Route("noiseData")]
		public async Task SaveNoiseAndSmoothedMeteoData([FromBody] MeteoDataPackage meteoData)
		{
            var meteoDataDTO = meteoData.ToDTO(SensorDataType.Noise);
            var meteoStationName = meteoDataDTO.MeteoStationName;

            _logger.LogInformation($"Noise meteo data received from meteo station '{meteoStationName}'.");

            var findedPackage = await _meteoDataDbContext.MeteoStationsData
                .SingleOrDefaultAsync(data => data.Equals(meteoDataDTO));

            if (findedPackage == null)
            {
                _meteoDataDbContext.MeteoStationsData.Add(meteoDataDTO);
                _logger.LogInformation($"New meteo station '{meteoStationName}' added with {meteoDataDTO.SensorData.Count} noise sensors data.");
            }
            else
            {
                findedPackage.SensorData = meteoDataDTO.SensorData;
                _logger.LogInformation($"Noise sensors data updated for meteo station {meteoStationName}.");
            }

            await _meteoDataDbContext.SaveChangesAsync();

			await SmoothAndSaveMeteoData(meteoDataDTO);
        }

		[HttpGet]
		[Route("getTotalMeteoStationsStatistics")]
		public async Task GetTotalMeteoDataStatistics()
		{

		}

		[HttpGet]
		[Route("getMeteoStationCSVData")]
		public async Task GetMeteoStationCSVData(string meteoStationId)
		{

		}

		private async Task SmoothAndSaveMeteoData(MeteoDataPackageDTO noiseMeteoData)
		{
			var smoothedMeteoData = new MeteoDataPackageDTO
			{
				PackageID = noiseMeteoData.PackageID,
				MeteoStationName = noiseMeteoData.MeteoStationName,
				SensorData = new List<SensorDataDTO>()
			};

			var lastNoiseMeteoDataBuffer = await _meteoDataDbContext.MeteoStationsData
				.Where(data => data.MeteoStationName == noiseMeteoData.MeteoStationName)
				.OrderByDescending(data => data.PackageID)
				.Take(_sensorDataSmoothPeriod)
				.SelectMany(data => data.SensorData)
				.GroupBy(data => data.SensorName)
				.ToDictionaryAsync(data => data.Key, data => data.ToList());

			for (int i = 0; i < noiseMeteoData.SensorData.Count; i++)
			{
				var sensorName = noiseMeteoData.SensorData[i].SensorName;
				
				if (lastNoiseMeteoDataBuffer.TryGetValue(sensorName, out List<SensorDataDTO> sensorValues))
				{
					smoothedMeteoData.SensorData.Add(
						new SensorDataDTO
						{
							Type = SensorDataType.Smooth,
							SensorName = sensorValues.First().SensorName,
							SensorValue = sensorValues.Average(value => value.SensorValue)
						});
				}
			}

            var findedPackage = await _meteoDataDbContext.MeteoStationsData
                .SingleOrDefaultAsync(data => data.Equals(smoothedMeteoData));

			var meteoStationId = smoothedMeteoData.MeteoStationName;
            if (findedPackage == null)
            {
                _meteoDataDbContext.MeteoStationsData.Add(smoothedMeteoData);
                _logger.LogInformation($"New meteo station '{meteoStationId}' added with {smoothedMeteoData.SensorData.Count} smoothed sensors data.");
            }
            else
            {
                findedPackage.SensorData.AddRange(smoothedMeteoData.SensorData);
                _logger.LogInformation($"Smoothed sensors data updated for meteo station {meteoStationId}.");
            }

			await _meteoDataDbContext.SaveChangesAsync();
        }
	}
}
