using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateDriverFile2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_StorageFile_FileId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_FileId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Drivers");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 22, 14, 52, 38, 199, DateTimeKind.Local).AddTicks(1214));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 22, 14, 52, 38, 130, DateTimeKind.Local).AddTicks(4058), "AQAAAAIAAYagAAAAEJ4LVsEEgU3VHmkfDq3JlPq8aaLzoEVTJAMpzadBFH19o5db3vA4CFRry3n/eJbFOA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_LicenseFileId",
                table: "Drivers",
                column: "LicenseFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_StorageFile_LicenseFileId",
                table: "Drivers",
                column: "LicenseFileId",
                principalTable: "StorageFile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_StorageFile_LicenseFileId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_LicenseFileId",
                table: "Drivers");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Drivers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 22, 14, 39, 47, 389, DateTimeKind.Local).AddTicks(1083));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 22, 14, 39, 47, 298, DateTimeKind.Local).AddTicks(4378), "AQAAAAIAAYagAAAAELq63A0VUApW5UyzLZ/LcrGPFagJ6Qj6R+t0gRlpiuTO/dMTT6MmFdxmTt9FFZ6Vmw==" });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_FileId",
                table: "Drivers",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_StorageFile_FileId",
                table: "Drivers",
                column: "FileId",
                principalTable: "StorageFile",
                principalColumn: "Id");
        }
    }
}
