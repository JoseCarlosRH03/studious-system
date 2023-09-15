using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class fuel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FuelTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Gasolina Premium" },
                    { 2, "Gasolina Regular" },
                    { 3, "Diesel Premium" },
                    { 4, "Diesel Regular" },
                    { 5, "Gasoil Optimo" },
                    { 6, "Gasoil Regular" },
                    { 7, "Kerosene" },
                    { 8, "Gas Licuado (GLP)" },
                    { 9, "Gas Natural (GNV)" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 15, 16, 3, 32, 325, DateTimeKind.Local).AddTicks(6823), "AQAAAAIAAYagAAAAEPOPKilqIpTDVkN3t5wli5g4/XsTu18BkQdH8yrwohI2YasctIJ8JAtEKDpYqZozYw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelTypes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 6, 21, 48, 1, 45, DateTimeKind.Local).AddTicks(6211), "AQAAAAIAAYagAAAAEOPhRARyv4p3qgRP5JXRLfwdLWp6PZ4jrrcQiUDYnOB76f1y+xYAD715Db9xRaQ+MA==" });
        }
    }
}
