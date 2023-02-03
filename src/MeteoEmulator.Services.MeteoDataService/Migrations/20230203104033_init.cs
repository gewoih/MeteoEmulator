using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MeteoEmulator.Services.MeteoDataService.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeteoDataPackage",
                columns: table => new
                {
                    DataPackageID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmulatorID = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeteoDataPackage", x => x.DataPackageID);
                });

            migrationBuilder.CreateTable(
                name: "SensorData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SensorName = table.Column<string>(type: "text", nullable: false),
                    SensorValue = table.Column<double>(type: "double precision", nullable: false),
                    MeteoDataPackageDataPackageID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorData_MeteoDataPackage_MeteoDataPackageDataPackageID",
                        column: x => x.MeteoDataPackageDataPackageID,
                        principalTable: "MeteoDataPackage",
                        principalColumn: "DataPackageID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_MeteoDataPackageDataPackageID",
                table: "SensorData",
                column: "MeteoDataPackageDataPackageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorData");

            migrationBuilder.DropTable(
                name: "MeteoDataPackage");
        }
    }
}
