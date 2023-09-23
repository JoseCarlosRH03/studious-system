using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateLicenseFailIdUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LicenseFileId",
                table: "Drivers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 22, 22, 23, 0, 950, DateTimeKind.Local).AddTicks(9325));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 22, 22, 23, 0, 880, DateTimeKind.Local).AddTicks(5286), "AQAAAAIAAYagAAAAEE5xaUd/ylacMuBqGmpUzZpqUMVPRrvpzDUNyAEePPp9l2Hprjip3EmFBjqS+GaIyw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LicenseFileId",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 22, 20, 1, 13, 781, DateTimeKind.Local).AddTicks(3281));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 22, 20, 1, 13, 717, DateTimeKind.Local).AddTicks(5982), "AQAAAAIAAYagAAAAEMkAH1BxsNxtjvFyY/VtsfBOYfN2P3wDoMuowaR4Id0hUKVeXF+SDuiwRLNhMUsUEQ==" });
        }
    }
}
