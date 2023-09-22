using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FleetTechAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateCityandState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Distrito Nacional");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Azua de Compostela", 1 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Estebanía", 1 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Guayabal", 1 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Las Charcas", 1 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Las Yayas de Viajama", 1 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Padre Las Casas", 1 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Peralta", 1 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Pueblo Viejo", 1 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Sabana Yegua", 1 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Tábara Arriba", 1 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Neiba", 2 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Galván", 2 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Los Ríos", 2 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Tamayo", 2 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Villa Jaragua", 2 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Barahona", 3 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Cabral", 3 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "El Peñón", 3 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Enriquillo", 3 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Fundación", 3 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Jaquimeyes", 3 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "La Ciénaga", 3 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Las Salinas", 3 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Paraíso", 3 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Polo", 3 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Vicente Noble", 3 });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "LastModifiedBy", "LastModifiedOn", "Name", "StateId", "Status" },
                values: new object[,]
                {
                    { 28, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Dajabón", 4, 0 },
                    { 29, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "El Pino", 4, 0 },
                    { 30, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Loma de Cabrera", 4, 0 },
                    { 31, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Partido", 4, 0 },
                    { 32, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Restauración", 4, 0 },
                    { 33, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Francisco de Macorís", 6, 0 },
                    { 34, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Arenoso", 6, 0 },
                    { 35, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Castillo", 6, 0 },
                    { 36, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Eugenio María de Hostos", 6, 0 },
                    { 37, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Las Guáranas", 6, 0 },
                    { 38, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pimentel", 6, 0 },
                    { 39, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa Riva", 6, 0 },
                    { 40, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "El Seibo", 8, 0 },
                    { 41, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Miches", 8, 0 },
                    { 42, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Comendador", 7, 0 },
                    { 43, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bánica", 7, 0 },
                    { 44, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "El Llano", 7, 0 },
                    { 45, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hondo Valle", 7, 0 },
                    { 46, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Juan Santiago", 7, 0 },
                    { 47, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pedro Santana", 7, 0 },
                    { 48, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Moca", 9, 0 },
                    { 49, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cayetano Germosén", 9, 0 },
                    { 50, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Gaspar Hernández", 9, 0 },
                    { 51, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Jamao al Norte", 9, 0 },
                    { 52, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hato Mayor del Rey", 10, 0 },
                    { 53, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "El Valle", 10, 0 },
                    { 54, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sabana de la Mar", 10, 0 },
                    { 55, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Salcedo", 11, 0 },
                    { 56, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tenares", 11, 0 },
                    { 57, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa Tapia", 11, 0 },
                    { 58, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Jimaní", 12, 0 },
                    { 59, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cristóbal", 12, 0 },
                    { 60, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Duvergé", 12, 0 },
                    { 61, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "La Descubierta", 12, 0 },
                    { 62, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mella", 12, 0 },
                    { 63, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Postrer Río", 12, 0 },
                    { 64, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Higüey", 13, 0 },
                    { 65, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Rafael del Yuma", 13, 0 },
                    { 66, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "La Romana", 14, 0 },
                    { 67, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Guaymate", 14, 0 },
                    { 68, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa Hermosa", 14, 0 },
                    { 69, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "La Concepción de La Vega", 15, 0 },
                    { 70, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Constanza", 15, 0 },
                    { 71, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Jarabacoa", 15, 0 },
                    { 72, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Jima Abajo", 15, 0 },
                    { 73, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nagua", 16, 0 },
                    { 74, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cabrera", 16, 0 },
                    { 75, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "El Factor", 16, 0 },
                    { 76, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Río San Juan", 16, 0 },
                    { 77, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bonao", 17, 0 },
                    { 78, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Maimón", 17, 0 },
                    { 79, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Piedra Blanca", 17, 0 },
                    { 80, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Montecristi", 18, 0 },
                    { 81, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Castañuela", 18, 0 },
                    { 82, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Guayubín", 18, 0 },
                    { 83, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Las Matas de Santa Cruz", 18, 0 },
                    { 84, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pepillo Salcedo", 18, 0 },
                    { 85, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa Vásquez", 18, 0 },
                    { 86, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Monte Plata", 19, 0 },
                    { 87, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bayaguana", 19, 0 },
                    { 88, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Peralvillo", 19, 0 },
                    { 89, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sabana Grande de Boyá", 19, 0 },
                    { 90, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Yamasá", 19, 0 },
                    { 91, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pedernales", 20, 0 },
                    { 92, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Oviedo", 20, 0 },
                    { 93, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Baní", 21, 0 },
                    { 94, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nizao", 21, 0 },
                    { 95, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Puerto Plata", 22, 0 },
                    { 96, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Altamira", 22, 0 },
                    { 97, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Guananico", 22, 0 },
                    { 98, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Imbert", 22, 0 },
                    { 99, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Los Hidalgos", 22, 0 },
                    { 100, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Luperón", 22, 0 },
                    { 101, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sosúa", 22, 0 },
                    { 102, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa Isabela", 22, 0 },
                    { 103, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa Montellano", 22, 0 },
                    { 104, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Samaná", 23, 0 },
                    { 105, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Las Terrenas", 23, 0 },
                    { 106, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sánchez", 23, 0 },
                    { 107, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Cristóbal", 25, 0 },
                    { 108, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bajos de Haina", 25, 0 },
                    { 109, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cambita Garabito", 25, 0 },
                    { 110, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Los Cacaos", 25, 0 },
                    { 111, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sabana Grande de Palenque", 25, 0 },
                    { 112, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Gregorio de Nigua", 25, 0 },
                    { 113, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa Altagracia", 25, 0 },
                    { 114, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Yaguate", 25, 0 },
                    { 115, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San José de Ocoa", 26, 0 },
                    { 116, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Rancho Arriba", 26, 0 },
                    { 117, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sabana Larga", 26, 0 },
                    { 118, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Juan de la Maguana", 27, 0 },
                    { 119, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bohechío", 27, 0 },
                    { 120, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "El Cercado", 27, 0 },
                    { 121, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Juan de Herrera", 27, 0 },
                    { 122, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Las Matas de Farfán", 27, 0 },
                    { 123, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Vallejuelo", 27, 0 },
                    { 124, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Pedro de Macorís", 28, 0 },
                    { 125, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Consuelo", 28, 0 },
                    { 126, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Guayacanes", 28, 0 },
                    { 127, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Quisqueya", 28, 0 },
                    { 128, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ramón Santana", 28, 0 },
                    { 129, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San José de Los Llanos", 28, 0 },
                    { 130, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cotuí", 24, 0 },
                    { 131, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cevicos", 24, 0 },
                    { 132, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Fantino", 24, 0 },
                    { 133, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "La Mata", 24, 0 },
                    { 134, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santiago", 29, 0 },
                    { 135, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bisonó", 29, 0 },
                    { 136, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Jánico", 29, 0 },
                    { 137, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Licey al Medio", 29, 0 },
                    { 138, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Puñal", 29, 0 },
                    { 139, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sabana Iglesia", 29, 0 },
                    { 140, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San José de las Matas", 29, 0 },
                    { 141, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tamboril", 29, 0 },
                    { 142, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Villa González", 29, 0 },
                    { 143, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Ignacio de Sabaneta", 30, 0 },
                    { 144, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Los Almácigos", 30, 0 },
                    { 145, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Monción", 30, 0 },
                    { 146, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santo Domingo Este", 31, 0 },
                    { 147, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Boca Chica", 31, 0 },
                    { 148, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Los Alcarrizos", 31, 0 },
                    { 149, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pedro Brand", 31, 0 },
                    { 150, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Antonio de Guerra", 31, 0 },
                    { 151, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santo Domingo Norte", 31, 0 },
                    { 152, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Santo Domingo Oeste", 31, 0 },
                    { 153, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mao", 32, 0 },
                    { 154, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Esperanza", 32, 0 },
                    { 155, new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2023, 9, 21, 21, 30, 3, 788, DateTimeKind.Local).AddTicks(531), null, null, "Laguna Salada", 32, 0 }
                });

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "Sánchez Ramírez");

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "San Cristóbal");

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "San José de Ocoa");

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "San Juan");

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "Id",
                keyValue: 28,
                column: "Name",
                value: "San Pedro de Macorís");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 21, 30, 3, 721, DateTimeKind.Local).AddTicks(8689), "AQAAAAIAAYagAAAAEHh4on/TklfBwbbsuEdt1UkQlRZjx/e1v4A05lqwr7q/78uIVr4LB/aLV67lwibsuQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Santo Domingo");

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Boca Chica", 31 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Los Alcarrizos", 31 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Pedro Brand", 31 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "San Antonio de Guerra", 31 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Santo Domingo Este", 31 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Santo Domingo Norte", 31 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Santo Domingo Oeste", 31 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Puerto Plata", 22 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Altamira", 22 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Guananico", 22 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Imbert", 22 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Los Hidalgos", 22 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Luperón", 22 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Sosúa", 22 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Villa Isabela", 22 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Villa Montellano", 22 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Baitoa", 29 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Jánico", 29 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Licey al Medio", 29 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Puñal", 29 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Sabana Iglesia", 29 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "San José de las Matas", 29 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Santiago", 29 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Tamboril", 29 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Villa Bisonó", 29 });

            migrationBuilder.UpdateData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Name", "StateId" },
                values: new object[] { "Villa González", 29 });

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "San Cristóbal");

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "San José de Ocoa");

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "San Juan");

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "San Pedro de Macorís");

            migrationBuilder.UpdateData(
                table: "States",
                keyColumn: "Id",
                keyValue: 28,
                column: "Name",
                value: "Sánchez Ramírez");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "DateCreated", "PasswordHash" },
                values: new object[] { new DateTime(2023, 9, 21, 19, 25, 51, 642, DateTimeKind.Local).AddTicks(4603), "AQAAAAIAAYagAAAAEAU3FYig7qodK5Cnkqs1CHpZ8FNrNwZ6dhuJhWYRw0g4ISbcgE9RN1ZtWnPS9733Bw==" });
        }
    }
}
