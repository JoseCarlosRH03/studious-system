using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 21, 4, 1, 682, DateTimeKind.Local).AddTicks(8882), "AQAAAAIAAYagAAAAEK5ILfATdWvKkrkEodhoCSfwB0D8X6c3FjelucNT8W9/oSihA/rGyurqRQbRbHqfaQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 20, 10, 46, 3, 868, DateTimeKind.Local).AddTicks(2966), "AQAAAAIAAYagAAAAEGSg3IhKZrNKR+uU4Cao10sX781HJ/OObjT2LMcj+9YedZO2FDby50zX4YQ6jCXTCA==" });
        }
    }
}
