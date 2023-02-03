using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeteoEmulator.Services.MeteoDataService.Migrations
{
    /// <inheritdoc />
    public partial class SensorsDataAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorData_MeteoDataPackage_MeteoDataPackageDataPackageID_M~",
                table: "SensorData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SensorData",
                table: "SensorData");

            migrationBuilder.RenameTable(
                name: "SensorData",
                newName: "SensorsData");

            migrationBuilder.RenameIndex(
                name: "IX_SensorData_MeteoDataPackageDataPackageID_MeteoDataPackageEm~",
                table: "SensorsData",
                newName: "IX_SensorsData_MeteoDataPackageDataPackageID_MeteoDataPackageE~");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SensorsData",
                table: "SensorsData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorsData_MeteoDataPackage_MeteoDataPackageDataPackageID_~",
                table: "SensorsData",
                columns: new[] { "MeteoDataPackageDataPackageID", "MeteoDataPackageEmulatorID" },
                principalTable: "MeteoDataPackage",
                principalColumns: new[] { "DataPackageID", "EmulatorID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorsData_MeteoDataPackage_MeteoDataPackageDataPackageID_~",
                table: "SensorsData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SensorsData",
                table: "SensorsData");

            migrationBuilder.RenameTable(
                name: "SensorsData",
                newName: "SensorData");

            migrationBuilder.RenameIndex(
                name: "IX_SensorsData_MeteoDataPackageDataPackageID_MeteoDataPackageE~",
                table: "SensorData",
                newName: "IX_SensorData_MeteoDataPackageDataPackageID_MeteoDataPackageEm~");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SensorData",
                table: "SensorData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorData_MeteoDataPackage_MeteoDataPackageDataPackageID_M~",
                table: "SensorData",
                columns: new[] { "MeteoDataPackageDataPackageID", "MeteoDataPackageEmulatorID" },
                principalTable: "MeteoDataPackage",
                principalColumns: new[] { "DataPackageID", "EmulatorID" });
        }
    }
}
