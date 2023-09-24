using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateFuel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FuelType",
                table: "FuelPrices",
                newName: "Status");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTo",
                table: "FuelPrices",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "FuelPrices",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "FuelTypeId",
                table: "FuelPrices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 23, 22, 58, 50, 168, DateTimeKind.Local).AddTicks(7989));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 23, 22, 58, 50, 120, DateTimeKind.Local).AddTicks(4467), "AQAAAAIAAYagAAAAEKuK8Zi0R+LtfDrTNMbdz4YKiBxic4lRX4DCjSmZTA6ge+QOOjRkb/NFcsopsdbsxA==" });

            migrationBuilder.CreateIndex(
                name: "IX_FuelPrices_FuelTypeId",
                table: "FuelPrices",
                column: "FuelTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelPrices_FuelTypes_FuelTypeId",
                table: "FuelPrices",
                column: "FuelTypeId",
                principalTable: "FuelTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelPrices_FuelTypes_FuelTypeId",
                table: "FuelPrices");

            migrationBuilder.DropIndex(
                name: "IX_FuelPrices_FuelTypeId",
                table: "FuelPrices");

            migrationBuilder.DropColumn(
                name: "FuelTypeId",
                table: "FuelPrices");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "FuelPrices",
                newName: "FuelType");

            migrationBuilder.AlterColumn<int>(
                name: "DateTo",
                table: "FuelPrices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "DateFrom",
                table: "FuelPrices",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

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
    }
}
