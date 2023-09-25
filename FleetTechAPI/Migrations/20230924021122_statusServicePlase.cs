using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class statusServicePlase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 23, 22, 11, 22, 49, DateTimeKind.Local).AddTicks(6141));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 23, 22, 11, 21, 977, DateTimeKind.Local).AddTicks(5610), "AQAAAAIAAYagAAAAEOwWik83krY10xsOvqSctEObT55wIT8SXVTF0jECG0vsbS0lmsdUR6fC7Csc2GAXhw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 23, 22, 4, 22, 558, DateTimeKind.Local).AddTicks(6566));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 23, 22, 4, 22, 495, DateTimeKind.Local).AddTicks(6657), "AQAAAAIAAYagAAAAENqwcW5T776c5oyIWm0+8JmRviJyxdDvvHPuMov+EThrp3Vx5Z2ny47z1Zq9nIKAuA==" });
        }
    }
}
