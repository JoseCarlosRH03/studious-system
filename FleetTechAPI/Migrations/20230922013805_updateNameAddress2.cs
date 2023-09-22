using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateNameAddress2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelStations_Addess_AddressId",
                table: "FuelStations");

            migrationBuilder.DropForeignKey(
                name: "FK_MechanicalWorkshop_Addess_AddressId",
                table: "MechanicalWorkshop");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Addess_AddressId",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "Addess");

            migrationBuilder.CreateTable(
                name: "Address",
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
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 21, 21, 38, 5, 227, DateTimeKind.Local).AddTicks(2727));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 21, 38, 5, 150, DateTimeKind.Local).AddTicks(3299), "AQAAAAIAAYagAAAAEJyVhPht5vq/JHL/vmtuOgOg0eOHkBlYhpnpwGv75Wx2QXrlR2830L/KI1Qu+ZtUiQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CityId",
                table: "Address",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelStations_Address_AddressId",
                table: "FuelStations",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicalWorkshop_Address_AddressId",
                table: "MechanicalWorkshop",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Address_AddressId",
                table: "Suppliers",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelStations_Address_AddressId",
                table: "FuelStations");

            migrationBuilder.DropForeignKey(
                name: "FK_MechanicalWorkshop_Address_AddressId",
                table: "MechanicalWorkshop");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Address_AddressId",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.CreateTable(
                name: "Addess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false),
                    AddressLine1 = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine2 = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine3 = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    PlainAddress = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addess_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 21, 21, 34, 27, 766, DateTimeKind.Local).AddTicks(5856));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 21, 34, 27, 691, DateTimeKind.Local).AddTicks(6089), "AQAAAAIAAYagAAAAEP7YVOXWEYUY19OjzQjIKk5aajlmTUWEljn83hEB3MNWYCPXff9zMNVCKmivxYkcmA==" });

            migrationBuilder.CreateIndex(
                name: "IX_Addess_CityId",
                table: "Addess",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelStations_Addess_AddressId",
                table: "FuelStations",
                column: "AddressId",
                principalTable: "Addess",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicalWorkshop_Addess_AddressId",
                table: "MechanicalWorkshop",
                column: "AddressId",
                principalTable: "Addess",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Addess_AddressId",
                table: "Suppliers",
                column: "AddressId",
                principalTable: "Addess",
                principalColumn: "Id");
        }
    }
}
