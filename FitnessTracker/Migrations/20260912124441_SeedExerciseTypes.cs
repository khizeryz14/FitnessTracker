using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class SeedExerciseTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ExerciseTypes",
                columns: new[] { "Id", "MetValue", "Name" },
                values: new object[,]
                {
                    { new Guid("0565c351-6a66-460e-9e11-4092d380b33a"), 3.5, "Walking" },
                    { new Guid("4ce70da8-8645-4494-a989-76f6ee85f5d4"), 8.0, "Swimming" },
                    { new Guid("eb6e6e2f-0663-404a-8c1d-fd2f89c448b5"), 7.5, "Cycling" },
                    { new Guid("ef861371-49cb-4fda-9cf7-f66b293a91d1"), 9.8000000000000007, "Running" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ExerciseTypes",
                keyColumn: "Id",
                keyValue: new Guid("0565c351-6a66-460e-9e11-4092d380b33a"));

            migrationBuilder.DeleteData(
                table: "ExerciseTypes",
                keyColumn: "Id",
                keyValue: new Guid("4ce70da8-8645-4494-a989-76f6ee85f5d4"));

            migrationBuilder.DeleteData(
                table: "ExerciseTypes",
                keyColumn: "Id",
                keyValue: new Guid("eb6e6e2f-0663-404a-8c1d-fd2f89c448b5"));

            migrationBuilder.DeleteData(
                table: "ExerciseTypes",
                keyColumn: "Id",
                keyValue: new Guid("ef861371-49cb-4fda-9cf7-f66b293a91d1"));
        }
    }
}
