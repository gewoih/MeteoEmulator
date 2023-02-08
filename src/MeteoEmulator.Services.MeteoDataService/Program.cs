using Microsoft.EntityFrameworkCore;
using MeteoEmulator.Services.MeteoDataService.DAL.DBContexts;
using MeteoEmulator.Services.MeteoDataService.Services.Interfaces;

namespace MeteoEmulator.Services.MeteoDataService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole(c => c.TimestampFormat = "[HH:mm:ss]");

            var postgreConnectionString = builder.Configuration.GetConnectionString("PostgreSQL");
            builder.Services.AddDbContext<MeteoDataDBContext>(options => options.UseNpgsql(postgreConnectionString));
            builder.Services.AddTransient<IMeteoDataService, Services.MeteoDataService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}