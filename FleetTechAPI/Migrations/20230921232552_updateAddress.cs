using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "MechanicalWorkshop");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "FuelStations");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Suppliers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "MechanicalWorkshop",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "FuelStations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MainAddess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    PlainAddress = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine1 = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine2 = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine3 = table.Column<string>(type: "TEXT", nullable: true),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainAddess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainAddess_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 19, 25, 51, 642, DateTimeKind.Local).AddTicks(4603), "AQAAAAIAAYagAAAAEAU3FYig7qodK5Cnkqs1CHpZ8FNrNwZ6dhuJhWYRw0g4ISbcgE9RN1ZtWnPS9733Bw==" });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_AddressId",
                table: "Suppliers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicalWorkshop_AddressId",
                table: "MechanicalWorkshop",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_FuelStations_AddressId",
                table: "FuelStations",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_MainAddess_CityId",
                table: "MainAddess",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelStations_MainAddess_AddressId",
                table: "FuelStations",
                column: "AddressId",
                principalTable: "MainAddess",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicalWorkshop_MainAddess_AddressId",
                table: "MechanicalWorkshop",
                column: "AddressId",
                principalTable: "MainAddess",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_MainAddess_AddressId",
                table: "Suppliers",
                column: "AddressId",
                principalTable: "MainAddess",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelStations_MainAddess_AddressId",
                table: "FuelStations");

            migrationBuilder.DropForeignKey(
                name: "FK_MechanicalWorkshop_MainAddess_AddressId",
                table: "MechanicalWorkshop");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_MainAddess_AddressId",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "MainAddess");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_AddressId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_MechanicalWorkshop_AddressId",
                table: "MechanicalWorkshop");

            migrationBuilder.DropIndex(
                name: "IX_FuelStations_AddressId",
                table: "FuelStations");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "MechanicalWorkshop");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "FuelStations");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "MechanicalWorkshop",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "FuelStations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 18, 51, 33, 491, DateTimeKind.Local).AddTicks(8169), "AQAAAAIAAYagAAAAELAPhn7+f0taj8e/5o2WBMYzorW3M7vbqdtaU1Bq74RvudaJa0e4YUv3KDPoezKi2g==" });
        }
    }
}
