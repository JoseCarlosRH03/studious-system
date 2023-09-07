using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class innit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanySettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeZone = table.Column<string>(type: "TEXT", nullable: false),
                    DatePattern = table.Column<string>(type: "TEXT", nullable: false),
                    TimePattern = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Demonym = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Extensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extensions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FuelType = table.Column<int>(type: "INTEGER", nullable: false),
                    DateFrom = table.Column<int>(type: "INTEGER", nullable: false),
                    DateTo = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelStations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    RNC = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mechanics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    JobTitle = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mechanics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    PolicyDescription = table.Column<string>(type: "TEXT", nullable: false),
                    PolicyNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PolicyReference = table.Column<string>(type: "TEXT", nullable: false),
                    PolicyExpiration = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Brand = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<string>(type: "TEXT", nullable: false),
                    LicensePlate = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: false),
                    FuelType = table.Column<int>(type: "INTEGER", nullable: false),
                    FuelCapacity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    FuelPerMonth = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Mileage = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Chassis = table.Column<string>(type: "TEXT", nullable: false),
                    Engine = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExtensionProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    ExtensionId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtensionProperties_Extensions_ExtensionId",
                        column: x => x.ExtensionId,
                        principalTable: "Extensions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Telephone = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    FuelStationId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_FuelStations_FuelStationId",
                        column: x => x.FuelStationId,
                        principalTable: "FuelStations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeCode = table.Column<string>(type: "TEXT", nullable: false),
                    IdentityDocument = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpirationOfTheLicense = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LicenseFileName = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    LicenseCategory_id = table.Column<int>(type: "INTEGER", nullable: false),
                    LicenseDriversId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_Licenses_LicenseDriversId",
                        column: x => x.LicenseDriversId,
                        principalTable: "Licenses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialConsumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    BrigadeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialConsumptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialConsumptions_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MechanicSpecialtys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    MechanicId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MechanicSpecialtys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MechanicSpecialtys_Mechanics_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PermissionAreaId = table.Column<int>(type: "INTEGER", nullable: false),
                    PermissionTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_PermissionAreas_PermissionAreaId",
                        column: x => x.PermissionAreaId,
                        principalTable: "PermissionAreas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Permissions_PermissionTypes_PermissionTypeId",
                        column: x => x.PermissionTypeId,
                        principalTable: "PermissionTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceSchedulings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mileage = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Description = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceSchedulings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceSchedulings_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    StateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    PermissionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    BranchTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false),
                    Locality = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_BranchTypes_BranchTypeId",
                        column: x => x.BranchTypeId,
                        principalTable: "BranchTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Branches_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    TaxRegistrationNumber = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine1 = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine2 = table.Column<string>(type: "TEXT", nullable: true),
                    AddressLine3 = table.Column<string>(type: "TEXT", nullable: true),
                    Region = table.Column<string>(type: "TEXT", nullable: false),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    UsernamePrefix = table.Column<string>(type: "TEXT", nullable: true),
                    CompanySettingsId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "TEXT", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_CompanySettings_CompanySettingsId",
                        column: x => x.CompanySettingsId,
                        principalTable: "CompanySettings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    ProfilePicture = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Document = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastLogin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastLoginFieldService = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateLastPasswordChange = table.Column<DateTime>(type: "TEXT", nullable: true),
                    BranchId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaterialExistences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialExistences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialExistences_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaterialExistences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PermissionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "BranchTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Comercial" },
                    { 2, "Tecnica" }
                });

            migrationBuilder.InsertData(
                table: "CompanySettings",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DatePattern", "LastModifiedBy", "LastModifiedOn", "TimePattern", "TimeZone" },
                values: new object[] { 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dd/MM/yyyy", null, null, "hh:mm tt", "America/Santo_Domingo" });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Demonym", "LastModifiedBy", "LastModifiedOn", "Name", "Status" },
                values: new object[,]
                {
                    { 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dominicano (a)", null, null, "Republica Dominicana", 0 },
                    { 2, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Estadounidense", null, null, "Estados Unidos", 0 },
                    { 3, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Canadiense", null, null, "Canada", 0 },
                    { 4, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mexicano (a)", null, null, "Mexico", 0 },
                    { 5, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Haitiano (a)", null, null, "Haiti", 0 },
                    { 6, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Puertorricense", null, null, "Puerto Rico", 0 },
                    { 7, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chino (a)", null, null, "China", 0 },
                    { 8, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Venezolano (a)", null, null, "Venezuela", 0 },
                    { 9, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Colombiano (a)", null, null, "Colombia", 0 }
                });

            migrationBuilder.InsertData(
                table: "Licenses",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Permiso de aprendizaje" },
                    { 2, "01a motocicletas y tricículos livianos" },
                    { 3, "01b motocicletas y tricículos pesados" },
                    { 4, "02 vehículos livianos" },
                    { 5, "02+R vehículos livianos con remolque" },
                    { 6, "03a vehículos pesados" },
                    { 7, "03+R vehículos pesados con remolque" },
                    { 8, "04 vehículos pesados de carga (patanas)" },
                    { 9, "05 vehículos especiales" }
                });

            migrationBuilder.InsertData(
                table: "PermissionAreas",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Companies" },
                    { 2, "Users" },
                    { 3, "Roles" },
                    { 4, "Permissions" },
                    { 5, "Cities" },
                    { 6, "Countries" },
                    { 7, "States" },
                    { 8, "Extensions" }
                });

            migrationBuilder.InsertData(
                table: "PermissionTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Create" },
                    { 2, "Update" },
                    { 3, "Read" },
                    { 4, "Delete" },
                    { 5, "Special" },
                    { 6, "Report" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "SuperAdmin" },
                    { 2, "Cajero" },
                    { 3, "Atención al Cliente" },
                    { 4, "Enc. Comercial" },
                    { 5, "Analista de Recaudo" },
                    { 6, "Enc. de Recaudo" },
                    { 7, "Enc. de Facturación" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BranchId", "DateCreated", "DateLastLogin", "DateLastLoginFieldService", "DateLastPasswordChange", "Document", "Email", "FirstName", "LastName", "LockoutEnd", "PasswordHash", "Phone", "ProfilePicture", "Status", "Username" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), 0, null, new DateTime(2023, 9, 6, 21, 48, 1, 45, DateTimeKind.Local).AddTicks(6211), null, null, null, null, "superadmin@gmail.com", "Super", "Admin", null, "AQAAAAIAAYagAAAAEOPhRARyv4p3qgRP5JXRLfwdLWp6PZ4jrrcQiUDYnOB76f1y+xYAD715Db9xRaQ+MA==", "(829) 123-4567", null, 1, "superadmin" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "PermissionAreaId", "PermissionTypeId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 1, 3 },
                    { 4, 1, 4 },
                    { 5, 1, 5 },
                    { 6, 1, 6 },
                    { 7, 2, 1 },
                    { 8, 2, 2 },
                    { 9, 2, 3 },
                    { 10, 2, 4 },
                    { 11, 2, 5 },
                    { 12, 2, 6 },
                    { 13, 3, 1 },
                    { 14, 3, 2 },
                    { 15, 3, 3 },
                    { 16, 3, 4 },
                    { 17, 3, 5 },
                    { 18, 3, 6 },
                    { 19, 4, 1 },
                    { 20, 4, 2 },
                    { 21, 4, 3 },
                    { 22, 4, 4 },
                    { 23, 4, 5 },
                    { 24, 4, 6 },
                    { 25, 5, 1 },
                    { 26, 5, 2 },
                    { 27, 5, 3 },
                    { 28, 5, 4 },
                    { 29, 5, 5 },
                    { 30, 5, 6 },
                    { 31, 6, 1 },
                    { 32, 6, 2 },
                    { 33, 6, 3 },
                    { 34, 6, 4 },
                    { 35, 6, 5 },
                    { 36, 6, 6 },
                    { 37, 7, 1 },
                    { 38, 7, 2 },
                    { 39, 7, 3 },
                    { 40, 7, 4 },
                    { 41, 7, 5 },
                    { 42, 7, 6 },
                    { 43, 8, 1 },
                    { 44, 8, 2 },
                    { 45, 8, 3 },
                    { 46, 8, 4 },
                    { 47, 8, 5 },
                    { 48, 8, 6 }
                });

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 1, 30, 2 },
                    { 2, 31, 2 },
                    { 3, 32, 2 },
                    { 4, 33, 2 },
                    { 5, 34, 2 },
                    { 6, 35, 2 }
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "CountryId", "CreatedBy", "CreatedOn", "LastModifiedBy", "LastModifiedOn", "Name", "Status" },
                values: new object[,]
                {
                    { 1, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Azua", 0 },
                    { 2, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bahoruco", 0 },
                    { 3, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Barahona", 0 },
                    { 4, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Dajabón", 0 },
                    { 5, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Distrito Nacional", 0 },
                    { 6, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Duarte", 0 },
                    { 7, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Elías Piña", 0 },
                    { 8, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "El Seibo", 0 },
                    { 9, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Espaillat", 0 },
                    { 10, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hato Mayor", 0 },
                    { 11, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hermanas Mirabal", 0 },
                    { 12, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Independencia", 0 },
                    { 13, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "La Altagracia", 0 },
                    { 14, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "La Romana", 0 },
                    { 15, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "La Vega", 0 },
                    { 16, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "María Trinidad Sánchez", 0 },
                    { 17, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Monseñor Nouel", 0 },
                    { 18, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Monte Cristi", 0 },
                    { 19, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Monte Plata", 0 },
                    { 20, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pedernales", 0 },
                    { 21, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Peravia", 0 },
                    { 22, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Puerto Plata", 0 },
                    { 23, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Samaná", 0 },
                    { 24, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Cristóbal", 0 },
                    { 25, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San José de Ocoa", 0 },
                    { 26, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Juan", 0 },
                    { 27, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Pedro de Macorís", 0 },
                    { 28, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sánchez Ramírez", 0 },
                    { 29, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santiago", 0 },
                    { 30, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santiago Rodríguez", 0 },
                    { 31, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santo Domingo", 0 },
                    { 32, 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Valverde", 0 }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "RoleId", "UserId" },
                values: new object[] { 1, 1, new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "LastModifiedBy", "LastModifiedOn", "Name", "StateId", "Status" },
                values: new object[,]
                {
                    { 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santo Domingo", 5, 0 },
                    { 2, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Boca Chica", 31, 0 },
                    { 3, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Los Alcarrizos", 31, 0 },
                    { 4, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pedro Brand", 31, 0 },
                    { 5, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Antonio de Guerra", 31, 0 },
                    { 6, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santo Domingo Este", 31, 0 },
                    { 7, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santo Domingo Norte", 31, 0 },
                    { 8, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santo Domingo Oeste", 31, 0 },
                    { 9, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Puerto Plata", 22, 0 },
                    { 10, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Altamira", 22, 0 },
                    { 11, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Guananico", 22, 0 },
                    { 12, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Imbert", 22, 0 },
                    { 13, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Los Hidalgos", 22, 0 },
                    { 14, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Luperón", 22, 0 },
                    { 15, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sosúa", 22, 0 },
                    { 16, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa Isabela", 22, 0 },
                    { 17, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa Montellano", 22, 0 },
                    { 18, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Baitoa", 29, 0 },
                    { 19, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Jánico", 29, 0 },
                    { 20, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Licey al Medio", 29, 0 },
                    { 21, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Puñal", 29, 0 },
                    { 22, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sabana Iglesia", 29, 0 },
                    { 23, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San José de las Matas", 29, 0 },
                    { 24, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santiago", 29, 0 },
                    { 25, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tamboril", 29, 0 },
                    { 26, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa Bisonó", 29, 0 },
                    { 27, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa González", 29, 0 }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "AddressLine1", "AddressLine2", "AddressLine3", "CityId", "Code", "CompanySettingsId", "CreatedBy", "CreatedOn", "Email", "LastModifiedBy", "LastModifiedOn", "Name", "Phone", "PostalCode", "Region", "TaxRegistrationNumber", "UsernamePrefix" },
                values: new object[] { 1, "Ave. Abraham Lincoln", "Edificio Blue Mall", "Piso 23", 1, "DT", 1, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ppe@prueba.com", null, null, "DobarTec", "8095864007", "10130", "", "9999", null });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BranchTypeId",
                table: "Branches",
                column: "BranchTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CityId",
                table: "Branches",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CityId",
                table: "Companies",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanySettingsId",
                table: "Companies",
                column: "CompanySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_FuelStationId",
                table: "Contacts",
                column: "FuelStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_LicenseDriversId",
                table: "Drivers",
                column: "LicenseDriversId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtensionProperties_ExtensionId",
                table: "ExtensionProperties",
                column: "ExtensionId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceSchedulings_VehicleId",
                table: "MaintenanceSchedulings",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialConsumptions_MaterialId",
                table: "MaterialConsumptions",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialExistences_MaterialId",
                table: "MaterialExistences",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialExistences_UserId",
                table: "MaterialExistences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MechanicSpecialtys_MechanicId",
                table: "MechanicSpecialtys",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionAreaId",
                table: "Permissions",
                column: "PermissionAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_PermissionTypeId",
                table: "Permissions",
                column: "PermissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_PermissionId",
                table: "RoleClaims",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_PermissionId",
                table: "UserClaims",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_BranchId",
                table: "Users",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "ExtensionProperties");

            migrationBuilder.DropTable(
                name: "FuelPrices");

            migrationBuilder.DropTable(
                name: "MaintenanceSchedulings");

            migrationBuilder.DropTable(
                name: "MaterialConsumptions");

            migrationBuilder.DropTable(
                name: "MaterialExistences");

            migrationBuilder.DropTable(
                name: "MechanicSpecialtys");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "CompanySettings");

            migrationBuilder.DropTable(
                name: "FuelStations");

            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Extensions");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Mechanics");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PermissionAreas");

            migrationBuilder.DropTable(
                name: "PermissionTypes");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "BranchTypes");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
