using MeteoEmulator.Libraries.SharedLibrary.DAO;
using Microsoft.EntityFrameworkCore;

namespace MeteoEmulator.Services.MeteoDataService.DAL.DBContexts
{
    public class MeteoDataDBContext : DbContext
    {
        public DbSet<MeteoDataPackageDAO> MeteoStationsData { get; set; }
        public DbSet<SensorDataDAO> SensorsData { get; set; }

        public MeteoDataDBContext(DbContextOptions options) : base(options) { }
    }
}
