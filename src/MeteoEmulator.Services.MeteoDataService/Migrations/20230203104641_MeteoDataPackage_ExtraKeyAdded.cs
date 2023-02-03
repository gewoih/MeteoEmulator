using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MeteoEmulator.Services.MeteoDataService.Migrations
{
    /// <inheritdoc />
    public partial class MeteoDataPackageExtraKeyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorData_MeteoDataPackage_MeteoDataPackageDataPackageID",
                table: "SensorData");

            migrationBuilder.DropIndex(
                name: "IX_SensorData_MeteoDataPackageDataPackageID",
                table: "SensorData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeteoDataPackage",
                table: "MeteoDataPackage");

            migrationBuilder.AddColumn<string>(
                name: "MeteoDataPackageEmulatorID",
                table: "SensorData",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DataPackageID",
                table: "MeteoDataPackage",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeteoDataPackage",
                table: "MeteoDataPackage",
                columns: new[] { "DataPackageID", "EmulatorID" });

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_MeteoDataPackageDataPackageID_MeteoDataPackageEm~",
                table: "SensorData",
                columns: new[] { "MeteoDataPackageDataPackageID", "MeteoDataPackageEmulatorID" });

            migrationBuilder.AddForeignKey(
                name: "FK_SensorData_MeteoDataPackage_MeteoDataPackageDataPackageID_M~",
                table: "SensorData",
                columns: new[] { "MeteoDataPackageDataPackageID", "MeteoDataPackageEmulatorID" },
                principalTable: "MeteoDataPackage",
                principalColumns: new[] { "DataPackageID", "EmulatorID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SensorData_MeteoDataPackage_MeteoDataPackageDataPackageID_M~",
                table: "SensorData");

            migrationBuilder.DropIndex(
                name: "IX_SensorData_MeteoDataPackageDataPackageID_MeteoDataPackageEm~",
                table: "SensorData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeteoDataPackage",
                table: "MeteoDataPackage");

            migrationBuilder.DropColumn(
                name: "MeteoDataPackageEmulatorID",
                table: "SensorData");

            migrationBuilder.AlterColumn<long>(
                name: "DataPackageID",
                table: "MeteoDataPackage",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeteoDataPackage",
                table: "MeteoDataPackage",
                column: "DataPackageID");

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_MeteoDataPackageDataPackageID",
                table: "SensorData",
                column: "MeteoDataPackageDataPackageID");

            migrationBuilder.AddForeignKey(
                name: "FK_SensorData_MeteoDataPackage_MeteoDataPackageDataPackageID",
                table: "SensorData",
                column: "MeteoDataPackageDataPackageID",
                principalTable: "MeteoDataPackage",
                principalColumn: "DataPackageID");
        }
    }
}
