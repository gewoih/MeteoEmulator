using MeteoEmulator.Libraries.SharedLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MeteoEmulator.Services.MeteoDataService.DAL.DBContexts
{
    public class MeteoDataDBContext : DbContext
    {
        public DbSet<MeteoDataPackage> RegularMeteoData { get; set; }
        public DbSet<MeteoDataPackage> NoiseMeteoData { get; set; }
        public DbSet<SensorData> SensorsData { get; set; }

        public MeteoDataDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeteoDataPackage>()
                .HasKey(data => new { data.DataPackageID, data.EmulatorID });
        }
    }
}
