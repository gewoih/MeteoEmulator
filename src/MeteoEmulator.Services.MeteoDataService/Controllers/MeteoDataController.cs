using MeteoEmulator.Libraries.SharedLibrary.Models;
using MeteoEmulator.Services.MeteoDataService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeteoEmulator.Services.MeteoDataService.Controllers
{
    public class MeteoDataController : Controller
    {
        private readonly IMeteoDataService _meteoDataService;

        public MeteoDataController(IMeteoDataService meteoDataService)
        {
            _meteoDataService = meteoDataService;
        }

        [HttpPut]
        [Route("data")]
        public async Task SaveDefaultMeteoData([FromBody] MeteoDataPackage package)
        {
            await _meteoDataService.SaveDefaultMeteoDataAsync(package);
        }

        [HttpPut]
        [Route("noiseData")]
        public async Task SaveNoiseMeteoData([FromBody] MeteoDataPackage package)
        {
            await _meteoDataService.SaveNoiseMeteoDataAsync(package);
            await _meteoDataService.SaveSmoothMeteoDataAsync(package, 10);
        }

        [HttpGet]
        [Route("getMeteoStationsStatistics")]
        public async Task<MeteoStationsStatistics> GetMeteoStationsStatistics()
        {
            return await _meteoDataService.GetMeteoStationsStatisticsAsync();
        }

        [HttpGet]
        [Route("getMeteoStationCSVData")]
        public async Task<string> GetMeteoStationCSVData(string meteoStationName)
        {
            return await _meteoDataService.GetMeteoStationCSVDataAsync(meteoStationName);
        }
    }
}
