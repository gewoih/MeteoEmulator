using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeteoEmulator.Services.MeteoDataService.Migrations
{
    /// <inheritdoc />
    public partial class ModelsNamingChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SensorValue",
                table: "SensorsData",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "SensorName",
                table: "SensorsData",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PackageID",
                table: "MeteoStationsData",
                newName: "PackageNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "SensorsData",
                newName: "SensorValue");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "SensorsData",
                newName: "SensorName");

            migrationBuilder.RenameColumn(
                name: "PackageNumber",
                table: "MeteoStationsData",
                newName: "PackageID");
        }
    }
}
