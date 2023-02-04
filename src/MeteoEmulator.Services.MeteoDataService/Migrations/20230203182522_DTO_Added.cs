using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeteoEmulator.Services.MeteoDataService.Migrations
{
    /// <inheritdoc />
    public partial class DTOAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorsData_MeteoDataPackage_MeteoDataPackageDataPackageID_~",
                table: "SensorsData");

            migrationBuilder.DropTable(
                name: "MeteoDataPackage");

            migrationBuilder.RenameColumn(
                name: "MeteoDataPackageEmulatorID",
                table: "SensorsData",
                newName: "MeteoDataPackageDTOMeteoStationName");

            migrationBuilder.RenameColumn(
                name: "MeteoDataPackageDataPackageID",
                table: "SensorsData",
                newName: "MeteoDataPackageDTOPackageID");

            migrationBuilder.RenameIndex(
                name: "IX_SensorsData_MeteoDataPackageDataPackageID_MeteoDataPackageE~",
                table: "SensorsData",
                newName: "IX_SensorsData_MeteoDataPackageDTOPackageID_MeteoDataPackageDT~");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "SensorsData",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MeteoStationsData",
                columns: table => new
                {
                    PackageID = table.Column<long>(type: "bigint", nullable: false),
                    MeteoStationName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeteoStationsData", x => new { x.PackageID, x.MeteoStationName });
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SensorsData_MeteoStationsData_MeteoDataPackageDTOPackageID_~",
                table: "SensorsData",
                columns: new[] { "MeteoDataPackageDTOPackageID", "MeteoDataPackageDTOMeteoStationName" },
                principalTable: "MeteoStationsData",
                principalColumns: new[] { "PackageID", "MeteoStationName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorsData_MeteoStationsData_MeteoDataPackageDTOPackageID_~",
                table: "SensorsData");

            migrationBuilder.DropTable(
                name: "MeteoStationsData");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "SensorsData");

            migrationBuilder.RenameColumn(
                name: "MeteoDataPackageDTOPackageID",
                table: "SensorsData",
                newName: "MeteoDataPackageDataPackageID");

            migrationBuilder.RenameColumn(
                name: "MeteoDataPackageDTOMeteoStationName",
                table: "SensorsData",
                newName: "MeteoDataPackageEmulatorID");

            migrationBuilder.RenameIndex(
                name: "IX_SensorsData_MeteoDataPackageDTOPackageID_MeteoDataPackageDT~",
                table: "SensorsData",
                newName: "IX_SensorsData_MeteoDataPackageDataPackageID_MeteoDataPackageE~");

            migrationBuilder.CreateTable(
                name: "MeteoDataPackage",
                columns: table => new
                {
                    DataPackageID = table.Column<long>(type: "bigint", nullable: false),
                    EmulatorID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeteoDataPackage", x => new { x.DataPackageID, x.EmulatorID });
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SensorsData_MeteoDataPackage_MeteoDataPackageDataPackageID_~",
                table: "SensorsData",
                columns: new[] { "MeteoDataPackageDataPackageID", "MeteoDataPackageEmulatorID" },
                principalTable: "MeteoDataPackage",
                principalColumns: new[] { "DataPackageID", "EmulatorID" });
        }
    }
}
