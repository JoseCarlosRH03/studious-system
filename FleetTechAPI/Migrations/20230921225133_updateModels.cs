using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MechanicSpecialtys_Mechanics_MechanicId",
                table: "MechanicSpecialtys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MechanicSpecialtys",
                table: "MechanicSpecialtys");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "MechanicSpecialtys");

            migrationBuilder.RenameTable(
                name: "MechanicSpecialtys",
                newName: "MechanicSpecialties");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Suppliers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "Suppliers",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_MechanicSpecialtys_MechanicId",
                table: "MechanicSpecialties",
                newName: "IX_MechanicSpecialties_MechanicId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MechanicalWorkshopId",
                table: "Contacts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MechanicId",
                table: "MechanicSpecialties",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MechanicSpecialties",
                table: "MechanicSpecialties",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FuelGestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Vehicle = table.Column<int>(type: "INTEGER", nullable: false),
                    Driver = table.Column<int>(type: "INTEGER", nullable: false),
                    FuelStation = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Mileage = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    FuelCapacity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelGestion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeCategory = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    CodeSubCategory = table.Column<string>(type: "TEXT", nullable: false),
                    SubCategory = table.Column<int>(type: "INTEGER", nullable: false),
                    CodeMaterial = table.Column<string>(type: "TEXT", nullable: false),
                    Materials = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplierId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Material_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MechanicalWorkshop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    RNC = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MechanicalWorkshop", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorksopSpecialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorksopId = table.Column<int>(type: "INTEGER", nullable: false),
                    MechanicalWorkshopId = table.Column<int>(type: "INTEGER", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorksopSpecialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorksopSpecialties_MechanicalWorkshop_MechanicalWorkshopId",
                        column: x => x.MechanicalWorkshopId,
                        principalTable: "MechanicalWorkshop",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 18, 51, 33, 491, DateTimeKind.Local).AddTicks(8169), "AQAAAAIAAYagAAAAELAPhn7+f0taj8e/5o2WBMYzorW3M7vbqdtaU1Bq74RvudaJa0e4YUv3KDPoezKi2g==" });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_MechanicalWorkshopId",
                table: "Contacts",
                column: "MechanicalWorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_Material_SupplierId",
                table: "Material",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_WorksopSpecialties_MechanicalWorkshopId",
                table: "WorksopSpecialties",
                column: "MechanicalWorkshopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_MechanicalWorkshop_MechanicalWorkshopId",
                table: "Contacts",
                column: "MechanicalWorkshopId",
                principalTable: "MechanicalWorkshop",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicSpecialties_Mechanics_MechanicId",
                table: "MechanicSpecialties",
                column: "MechanicId",
                principalTable: "Mechanics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_MechanicalWorkshop_MechanicalWorkshopId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_MechanicSpecialties_Mechanics_MechanicId",
                table: "MechanicSpecialties");

            migrationBuilder.DropTable(
                name: "FuelGestion");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "WorksopSpecialties");

            migrationBuilder.DropTable(
                name: "MechanicalWorkshop");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_MechanicalWorkshopId",
                table: "Contacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MechanicSpecialties",
                table: "MechanicSpecialties");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "MechanicalWorkshopId",
                table: "Contacts");

            migrationBuilder.RenameTable(
                name: "MechanicSpecialties",
                newName: "MechanicSpecialtys");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Suppliers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Suppliers",
                newName: "code");

            migrationBuilder.RenameIndex(
                name: "IX_MechanicSpecialties_MechanicId",
                table: "MechanicSpecialtys",
                newName: "IX_MechanicSpecialtys_MechanicId");

            migrationBuilder.AlterColumn<int>(
                name: "MechanicId",
                table: "MechanicSpecialtys",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "MechanicSpecialtys",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MechanicSpecialtys",
                table: "MechanicSpecialtys",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 20, 10, 46, 3, 868, DateTimeKind.Local).AddTicks(2966), "AQAAAAIAAYagAAAAEGSg3IhKZrNKR+uU4Cao10sX781HJ/OObjT2LMcj+9YedZO2FDby50zX4YQ6jCXTCA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_MechanicSpecialtys_Mechanics_MechanicId",
                table: "MechanicSpecialtys",
                column: "MechanicId",
                principalTable: "Mechanics",
                principalColumn: "Id");
        }
    }
}
