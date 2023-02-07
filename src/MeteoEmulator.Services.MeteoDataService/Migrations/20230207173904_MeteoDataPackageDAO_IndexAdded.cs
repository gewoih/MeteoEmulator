using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeteoEmulator.Services.MeteoDataService.Migrations
{
    /// <inheritdoc />
    public partial class MeteoDataPackageDAOIndexAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MeteoStationsData_MeteoStationName",
                table: "MeteoStationsData",
                column: "MeteoStationName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MeteoStationsData_MeteoStationName",
                table: "MeteoStationsData");
        }
    }
}
