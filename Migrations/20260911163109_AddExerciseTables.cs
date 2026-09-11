using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddExerciseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MetValue = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseLogEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DurationMinutes = table.Column<double>(type: "double precision", nullable: false),
                    LoggedTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseLogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseLogEntries_ExerciseTypes_ExerciseTypeId",
                        column: x => x.ExerciseTypeId,
                        principalTable: "ExerciseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseLogEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogEntries_ExerciseTypeId",
                table: "ExerciseLogEntries",
                column: "ExerciseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogEntries_UserId",
                table: "ExerciseLogEntries",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseLogEntries");

            migrationBuilder.DropTable(
                name: "ExerciseTypes");
        }
    }
}
