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

		public MeteoDataController(ILogger<MeteoDataController> logger, MeteoDataDBContext meteoDataDBContext)
		{
			_logger = logger;
			_meteoDataDbContext = meteoDataDBContext;
		}

		[HttpPut]
		[Route("data")]
		public async Task SaveRegularMeteoData([FromBody] MeteoDataPackage meteoData)
		{
			var meteoStationId = meteoData.EmulatorID;

			_logger.LogInformation($"Data received from meteo station '{meteoStationId}'.");

			var findedPackage = await _meteoDataDbContext.RegularMeteoData
				.Include(data => data.SensorData)
				.SingleOrDefaultAsync(data => data.Equals(meteoData));

			if (findedPackage == null) 
			{
				_meteoDataDbContext.RegularMeteoData.Add(meteoData);

				_logger.LogInformation($"New meteo station '{meteoStationId}' added with {meteoData.SensorData.Count} sensors data.");
			}
			else
			{
				findedPackage.SensorData = meteoData.SensorData;

				_logger.LogInformation($"Sensors data updated for meteo station {meteoStationId}.");
			}

			await _meteoDataDbContext.SaveChangesAsync();
		}

		[HttpPut]
		[Route("noiseData")]
		public async Task SaveNoiseMeteoData([FromBody] MeteoDataPackage meteoData)
		{

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
	}
}
