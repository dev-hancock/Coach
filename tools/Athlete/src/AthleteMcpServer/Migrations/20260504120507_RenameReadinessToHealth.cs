using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AthleteMcpServer.Migrations
{
    /// <inheritdoc />
    public partial class RenameReadinessToHealth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ReadinessEntries",
                newName: "HealthEntries");

            migrationBuilder.RenameIndex(
                name: "IX_ReadinessEntries_AthleteId",
                table: "HealthEntries",
                newName: "IX_HealthEntries_AthleteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "HealthEntries",
                newName: "ReadinessEntries");

            migrationBuilder.RenameIndex(
                name: "IX_HealthEntries_AthleteId",
                table: "ReadinessEntries",
                newName: "IX_ReadinessEntries_AthleteId");
        }
    }
}
