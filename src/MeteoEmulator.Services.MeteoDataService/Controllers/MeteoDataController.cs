using MeteoEmulator.Libraries.SharedLibrary.Models;
using MeteoEmulator.Services.MeteoDataService.DAL.DBContexts;
using Microsoft.AspNetCore.Mvc;

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
