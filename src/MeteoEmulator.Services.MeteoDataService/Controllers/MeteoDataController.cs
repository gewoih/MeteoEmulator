using MeteoEmulator.Libraries.SharedLibrary.DAO;
using MeteoEmulator.Libraries.SharedLibrary.Enums;
using MeteoEmulator.Libraries.SharedLibrary.Extensions;
using MeteoEmulator.Libraries.SharedLibrary.Models;
using MeteoEmulator.Services.MeteoDataService.DAL.DBContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
		[Route("getMeteoStationCSVData")]
		public async Task<string> GetMeteoStationCSVData(string meteoStationId)
		{
			/*var csvStringBuilder = new StringBuilder("PackageID;SensorName;RegularData;NoiseData;SmoothData");

			var firstData = _meteoDataDbContext.MeteoStationsData
				.Where(data => data.MeteoStationName == meteoStationId);
				
			var allData = await firstData.SelectMany(data => data.SensorData)
				.GroupBy(data => data.Package.PackageID)
				.ToDictionaryAsync(data => data.Key, data => data.ToList());
			
			foreach (var data in allData) 
			{
				var regularValue = data.Value.Single(v => v.Type == SensorDataType.Default).SensorValue;
				var noiseValue = data.Value.Single(v => v.Type == SensorDataType.Noise).SensorValue;
				var smoothValue = data.Value.Single(v => v.Type == SensorDataType.Smooth).SensorValue;

                csvStringBuilder.Append(data.Key);
                csvStringBuilder.Append(";");
				csvStringBuilder.Append(regularValue);
				csvStringBuilder.Append(";");
                csvStringBuilder.Append(noiseValue);
                csvStringBuilder.Append(";");
                csvStringBuilder.Append(smoothValue);
			}

			return csvStringBuilder.ToString();*/
		}

		private async Task SmoothAndSaveMeteoData(MeteoDataPackageDAO noiseMeteoData)
		{
			var smoothedMeteoData = new MeteoDataPackageDAO
			{
				PackageID = noiseMeteoData.PackageID,
				MeteoStationName = noiseMeteoData.MeteoStationName,
				SensorData = new List<SensorDataDAO>()
			};

			var lastNoiseMeteoDataBuffer = await _meteoDataDbContext.MeteoStationsData
				.Where(data => data.MeteoStationName == noiseMeteoData.MeteoStationName)
				.OrderByDescending(data => data.PackageID)
				.Take(_sensorDataSmoothPeriod)
				.SelectMany(data => data.SensorData)
				.Where(data => data.Type == SensorDataType.Noise)
				.GroupBy(data => data.SensorName)
				.ToDictionaryAsync(data => data.Key, data => data.ToList());

			for (int i = 0; i < noiseMeteoData.SensorData.Count; i++)
			{
				var sensorName = noiseMeteoData.SensorData[i].SensorName;
				
				if (lastNoiseMeteoDataBuffer.TryGetValue(sensorName, out List<SensorDataDAO> sensorValues))
				{
					smoothedMeteoData.SensorData.Add(
						new SensorDataDAO
						{
							Package = smoothedMeteoData,
							Type = SensorDataType.Smooth,
							SensorName = sensorValues.First().SensorName,
							SensorValue = sensorValues.Average(value => value.SensorValue)
						});
				}
			}

			await SaveMeteoData(smoothedMeteoData);
        }

		private async Task SaveMeteoData(MeteoDataPackageDAO meteoData)
		{
            var findedPackage = await _meteoDataDbContext.MeteoStationsData
				.Include(data => data.SensorData)
                .SingleOrDefaultAsync(data => data.MeteoStationName == meteoData.MeteoStationName && data.PackageID == meteoData.PackageID);

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
						data.Package.PackageID == sensor.Package.PackageID &&
						data.SensorName == sensor.SensorName &&
						data.Type == sensor.Type);

					if (findedSensor is null)
						findedPackage.SensorData.Add(sensor);
					else
						findedSensor.SensorValue = sensor.SensorValue;
				}
			}

            await _meteoDataDbContext.SaveChangesAsync();
        }
	}
}
