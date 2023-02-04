using MeteoEmulator.Libraries.SharedLibrary.DTO;
using Microsoft.EntityFrameworkCore;

namespace MeteoEmulator.Services.MeteoDataService.DAL.DBContexts
{
    public class MeteoDataDBContext : DbContext
    {
        public DbSet<MeteoDataPackageDTO> MeteoStationsData { get; set; }
        public DbSet<SensorDataDTO> SensorsData { get; set; }

        public MeteoDataDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeteoDataPackageDTO>()
                .HasKey(data => new { data.PackageID, data.MeteoStationName });
        }
    }
}
