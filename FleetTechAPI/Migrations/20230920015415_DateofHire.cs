using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class DateofHire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfHire",
                table: "Drivers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 19, 21, 54, 14, 927, DateTimeKind.Local).AddTicks(1375), "AQAAAAIAAYagAAAAEPee0HYIatE0X+o00VA/BxY2yjh5uwljFgwUOF/NSaQJyEH55SgrSpB3eVfMdyGyrw==" });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_FuelTypeId",
                table: "Vehicles",
                column: "FuelTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_FuelTypes_FuelTypeId",
                table: "Vehicles",
                column: "FuelTypeId",
                principalTable: "FuelTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_FuelTypes_FuelTypeId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_FuelTypeId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DateOfHire",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Drivers");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 18, 20, 31, 6, 840, DateTimeKind.Local).AddTicks(4427), "AQAAAAIAAYagAAAAEII8jIB7OOMwh/jbVF70x2PLwz5kLCVCpF61QeVg3OM9VcQpQdttlxa/59BT/SYTpg==" });
        }
    }
}
