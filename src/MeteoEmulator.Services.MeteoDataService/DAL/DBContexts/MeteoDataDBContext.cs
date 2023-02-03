﻿using MeteoEmulator.Libraries.SharedLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace MeteoEmulator.Services.MeteoDataService.DAL.DBContexts
{
    public class MeteoDataDBContext : DbContext
    {
        public DbSet<MeteoDataPackage> RegularMeteoData { get; set; }
        public DbSet<MeteoDataPackage> NoiseMeteoData { get; set; }

        public MeteoDataDBContext(DbContextOptions options) : base(options) { }
    }
}
