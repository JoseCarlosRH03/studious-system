using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateDriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LicenseFileId",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 22, 0, 23, 19, 44, DateTimeKind.Local).AddTicks(3600));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 22, 0, 23, 18, 979, DateTimeKind.Local).AddTicks(823), "AQAAAAIAAYagAAAAEOLBjdJu9/aK7y8X/Cu3tcx+7E+mzXbLkhEk+KSrATgkLj0HyMdqmwzj5u7oKlQgTA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseFileId",
                table: "Drivers");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 21, 22, 45, 29, 846, DateTimeKind.Local).AddTicks(1139));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 22, 45, 29, 780, DateTimeKind.Local).AddTicks(3004), "AQAAAAIAAYagAAAAEB7aw4n4P5lu4kMtdMM431dFIBycXBka8wuLHyx90cGM6VYbDt+HsADRTyzJUVcS5w==" });
        }
    }
}
