using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class nicolchannge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseFileName",
                table: "Drivers");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 21, 22, 21, 19, 740, DateTimeKind.Local).AddTicks(1060));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 22, 21, 19, 652, DateTimeKind.Local).AddTicks(7915), "AQAAAAIAAYagAAAAELKAFP5EtNKVcIBHwwVfCp8omrsMukOZr5j3rDZA+U0JIkAEzhxHz7KRvucusi3Lsg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LicenseFileName",
                table: "Drivers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 21, 22, 12, 43, 835, DateTimeKind.Local).AddTicks(4172));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 22, 12, 43, 785, DateTimeKind.Local).AddTicks(27), "AQAAAAIAAYagAAAAEMe1wehdn6Exov7+hjJsDuL26AwAr7XRSgcwSGQTU2xN8amhRlNm9qu9CLq/VOrZcA==" });
        }
    }
}
