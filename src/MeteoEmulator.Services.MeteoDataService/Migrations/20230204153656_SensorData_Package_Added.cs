using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeteoEmulator.Services.MeteoDataService.Migrations
{
    /// <inheritdoc />
    public partial class SensorDataPackageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorsData_MeteoStationsData_MeteoDataPackageDTOPackageID_~",
                table: "SensorsData");

            migrationBuilder.DropIndex(
                name: "IX_SensorsData_MeteoDataPackageDTOPackageID_MeteoDataPackageDT~",
                table: "SensorsData");

            migrationBuilder.DropColumn(
                name: "MeteoDataPackageDTOMeteoStationName",
                table: "SensorsData");

            migrationBuilder.DropColumn(
                name: "MeteoDataPackageDTOPackageID",
                table: "SensorsData");

            migrationBuilder.AddColumn<long>(
                name: "PackageID",
                table: "SensorsData",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "PackageMeteoStationName",
                table: "SensorsData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SensorsData_PackageID_PackageMeteoStationName",
                table: "SensorsData",
                columns: new[] { "PackageID", "PackageMeteoStationName" });

            migrationBuilder.AddForeignKey(
                name: "FK_SensorsData_MeteoStationsData_PackageID_PackageMeteoStation~",
                table: "SensorsData",
                columns: new[] { "PackageID", "PackageMeteoStationName" },
                principalTable: "MeteoStationsData",
                principalColumns: new[] { "PackageID", "MeteoStationName" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorsData_MeteoStationsData_PackageID_PackageMeteoStation~",
                table: "SensorsData");

            migrationBuilder.DropIndex(
                name: "IX_SensorsData_PackageID_PackageMeteoStationName",
                table: "SensorsData");

            migrationBuilder.DropColumn(
                name: "PackageID",
                table: "SensorsData");

            migrationBuilder.DropColumn(
                name: "PackageMeteoStationName",
                table: "SensorsData");

            migrationBuilder.AddColumn<string>(
                name: "MeteoDataPackageDTOMeteoStationName",
                table: "SensorsData",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MeteoDataPackageDTOPackageID",
                table: "SensorsData",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SensorsData_MeteoDataPackageDTOPackageID_MeteoDataPackageDT~",
                table: "SensorsData",
                columns: new[] { "MeteoDataPackageDTOPackageID", "MeteoDataPackageDTOMeteoStationName" });

            migrationBuilder.AddForeignKey(
                name: "FK_SensorsData_MeteoStationsData_MeteoDataPackageDTOPackageID_~",
                table: "SensorsData",
                columns: new[] { "MeteoDataPackageDTOPackageID", "MeteoDataPackageDTOMeteoStationName" },
                principalTable: "MeteoStationsData",
                principalColumns: new[] { "PackageID", "MeteoStationName" });
        }
    }
}
