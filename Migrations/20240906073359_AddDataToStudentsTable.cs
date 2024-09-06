using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToStudentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "Dob", "Email", "StudentName" },
                values: new object[,]
                {
                    { 1, "Lagos", new DateTime(2022, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Slymmd@gmail.com", "BabaMoh" },
                    { 2, "Ilorin", new DateTime(2023, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cashho@gmail.com", "Cashoo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
