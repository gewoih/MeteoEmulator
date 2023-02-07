using MeteoEmulator.Libraries.SharedLibrary.Models;
using MeteoEmulator.Services.MeteoDataService.DAL.DBContexts;

namespace MeteoEmulator.Services.MeteoDataService.Services.Interfaces
{
    public interface IMeteoDataService
    {
        public Task SaveDefaultMeteoDataAsync(MeteoDataPackage package);
        public Task SaveNoiseMeteoDataAsync(MeteoDataPackage package);
        public Task SaveSmoothMeteoDataAsync(MeteoDataPackage package, int smoothPeriod);
        public Task<string> GetMeteoStationCSVDataAsync(string meteoStationName);
        public Task<MeteoStationsStatistics> GetMeteoStationsStatisticsAsync();
    }
}
