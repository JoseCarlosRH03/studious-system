using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FuelType",
                table: "Vehicles",
                newName: "FuelTypeId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 18, 20, 31, 6, 840, DateTimeKind.Local).AddTicks(4427), "AQAAAAIAAYagAAAAEII8jIB7OOMwh/jbVF70x2PLwz5kLCVCpF61QeVg3OM9VcQpQdttlxa/59BT/SYTpg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FuelTypeId",
                table: "Vehicles",
                newName: "FuelType");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 15, 16, 3, 32, 325, DateTimeKind.Local).AddTicks(6823), "AQAAAAIAAYagAAAAEPOPKilqIpTDVkN3t5wli5g4/XsTu18BkQdH8yrwohI2YasctIJ8JAtEKDpYqZozYw==" });
        }
    }
}
