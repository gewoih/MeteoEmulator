using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MeteoEmulator.Services.MeteoDataService.Migrations
{
    /// <inheritdoc />
    public partial class MeteoDataPackageIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorsData_MeteoStationsData_PackageID_PackageMeteoStation~",
                table: "SensorsData");

            migrationBuilder.DropIndex(
                name: "IX_SensorsData_PackageID_PackageMeteoStationName",
                table: "SensorsData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeteoStationsData",
                table: "MeteoStationsData");

            migrationBuilder.DropColumn(
                name: "PackageMeteoStationName",
                table: "SensorsData");

            migrationBuilder.RenameColumn(
                name: "PackageID",
                table: "SensorsData",
                newName: "PackageId");

            migrationBuilder.AlterColumn<int>(
                name: "PackageId",
                table: "SensorsData",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MeteoStationsData",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeteoStationsData",
                table: "MeteoStationsData",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SensorsData_PackageId",
                table: "SensorsData",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorsData_MeteoStationsData_PackageId",
                table: "SensorsData",
                column: "PackageId",
                principalTable: "MeteoStationsData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorsData_MeteoStationsData_PackageId",
                table: "SensorsData");

            migrationBuilder.DropIndex(
                name: "IX_SensorsData_PackageId",
                table: "SensorsData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeteoStationsData",
                table: "MeteoStationsData");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MeteoStationsData");

            migrationBuilder.RenameColumn(
                name: "PackageId",
                table: "SensorsData",
                newName: "PackageID");

            migrationBuilder.AlterColumn<long>(
                name: "PackageID",
                table: "SensorsData",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "PackageMeteoStationName",
                table: "SensorsData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeteoStationsData",
                table: "MeteoStationsData",
                columns: new[] { "PackageID", "MeteoStationName" });

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
    }
}
