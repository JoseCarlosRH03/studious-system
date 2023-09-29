using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updata2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Mechanics",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Mechanics",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Mechanics",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 29, 12, 4, 34, 597, DateTimeKind.Local).AddTicks(4708));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 29, 12, 4, 34, 532, DateTimeKind.Local).AddTicks(4741), "AQAAAAIAAYagAAAAECyFw6ZBiTF+4XsAjSRf42ckXYDh9g0V96J2NWHQFJ7WOQ0MaK50ZvIgbw5xiPronw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Mechanics");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Mechanics");

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                table: "Mechanics",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 28, 21, 3, 9, 87, DateTimeKind.Local).AddTicks(5787));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 28, 21, 3, 9, 20, DateTimeKind.Local).AddTicks(7186), "AQAAAAIAAYagAAAAEBOPxgE+UlXBKLhvFSyTjcXdU8qXVAGEINNfoo1bq8/lOY+AMPtam5M8EM18EEHJGQ==" });
        }
    }
}
