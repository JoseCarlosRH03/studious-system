using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateNameAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Addess",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "MainAddess",
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
                    table.PrimaryKey("PK_MainAddess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MainAddess_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedOn",
                value: new DateTime(2023, 9, 21, 21, 30, 3, 788, DateTimeKind.Local).AddTicks(531));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 21, 30, 3, 721, DateTimeKind.Local).AddTicks(8689), "AQAAAAIAAYagAAAAEHh4on/TklfBwbbsuEdt1UkQlRZjx/e1v4A05lqwr7q/78uIVr4LB/aLV67lwibsuQ==" });

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
    }
}
