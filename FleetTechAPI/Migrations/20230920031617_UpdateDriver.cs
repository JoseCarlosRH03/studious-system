using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDriver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Licenses_LicenseDriversId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LicenseCategory_id",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "LicenseDriversId",
                table: "Drivers",
                newName: "LicenseCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Drivers_LicenseDriversId",
                table: "Drivers",
                newName: "IX_Drivers_LicenseCategoryId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 19, 23, 16, 17, 445, DateTimeKind.Local).AddTicks(7356), "AQAAAAIAAYagAAAAENxKQDC0HP9GNenzTIxa+gdJq2uh8vO0gHUdtTR5LO2dAg6kWV7emhrATHBUYDdWPQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Licenses_LicenseCategoryId",
                table: "Drivers",
                column: "LicenseCategoryId",
                principalTable: "Licenses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Licenses_LicenseCategoryId",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "LicenseCategoryId",
                table: "Drivers",
                newName: "LicenseDriversId");

            migrationBuilder.RenameIndex(
                name: "IX_Drivers_LicenseCategoryId",
                table: "Drivers",
                newName: "IX_Drivers_LicenseDriversId");

            migrationBuilder.AddColumn<int>(
                name: "LicenseCategory_id",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Licenses_LicenseDriversId",
                table: "Drivers",
                column: "LicenseDriversId",
                principalTable: "Licenses",
                principalColumn: "Id");
        }
    }
}
