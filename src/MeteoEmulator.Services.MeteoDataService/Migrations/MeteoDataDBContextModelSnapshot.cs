﻿// <auto-generated />
using System;
using MeteoEmulator.Services.MeteoDataService.DAL.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MeteoEmulator.Services.MeteoDataService.Migrations
{
    [DbContext(typeof(MeteoDataDBContext))]
    partial class MeteoDataDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MeteoEmulator.Libraries.SharedLibrary.DTO.MeteoDataPackageDTO", b =>
                {
                    b.Property<long>("PackageID")
                        .HasColumnType("bigint");

                    b.Property<string>("MeteoStationName")
                        .HasColumnType("text");

                    b.HasKey("PackageID", "MeteoStationName");

                    b.ToTable("MeteoStationsData");
                });

            modelBuilder.Entity("MeteoEmulator.Libraries.SharedLibrary.DTO.SensorDataDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("MeteoDataPackageDTOMeteoStationName")
                        .HasColumnType("text");

                    b.Property<long?>("MeteoDataPackageDTOPackageID")
                        .HasColumnType("bigint");

                    b.Property<string>("SensorName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("SensorValue")
                        .HasColumnType("double precision");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MeteoDataPackageDTOPackageID", "MeteoDataPackageDTOMeteoStationName");

                    b.ToTable("SensorsData");
                });

            modelBuilder.Entity("MeteoEmulator.Libraries.SharedLibrary.DTO.SensorDataDTO", b =>
                {
                    b.HasOne("MeteoEmulator.Libraries.SharedLibrary.DTO.MeteoDataPackageDTO", null)
                        .WithMany("SensorData")
                        .HasForeignKey("MeteoDataPackageDTOPackageID", "MeteoDataPackageDTOMeteoStationName");
                });

            modelBuilder.Entity("MeteoEmulator.Libraries.SharedLibrary.DTO.MeteoDataPackageDTO", b =>
                {
                    b.Navigation("SensorData");
                });
#pragma warning restore 612, 618
        }
    }
}
