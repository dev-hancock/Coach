using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AthleteMcpServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Athletes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ExperienceLevel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PreferredUnits = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TrainingDaysPerWeek = table.Column<int>(type: "integer", nullable: false),
                    PreferredLongRunDay = table.Column<int>(type: "integer", nullable: true),
                    CurrentWeeklyDistanceKm = table.Column<decimal>(type: "numeric", nullable: false),
                    TypicalLongRunDistanceKm = table.Column<decimal>(type: "numeric", nullable: false),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Athletes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoachDecisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AthleteId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DecisionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Reason = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Recommendation = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachDecisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoachDecisions_Athletes_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadinessEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AthleteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    EntryType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BodyLocation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AffectedRunning = table.Column<bool>(type: "boolean", nullable: false),
                    RequiresFollowUp = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadinessEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadinessEntries_Athletes_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AthleteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FirstUsedOn = table.Column<DateOnly>(type: "date", nullable: true),
                    RetireAfterKm = table.Column<decimal>(type: "numeric", nullable: false),
                    DistanceLoggedKm = table.Column<decimal>(type: "numeric", nullable: false),
                    IsRetired = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shoes_Athletes_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingGoals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AthleteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Distance = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TargetDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TargetTime = table.Column<double>(type: "double precision", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingGoals_Athletes_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AthleteId = table.Column<Guid>(type: "uuid", nullable: false),
                    GoalId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phase = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TargetWeeklyDistanceKm = table.Column<decimal>(type: "numeric", nullable: false),
                    TrainingDaysPerWeek = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPlans_Athletes_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingPlans_TrainingGoals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "TrainingGoals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PlannedSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrainingPlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    SessionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Intensity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TargetDistanceKm = table.Column<decimal>(type: "numeric", nullable: true),
                    TargetDuration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    TargetPacePerKm = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CompletedRunId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannedSessions_TrainingPlans_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AthleteId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlannedSessionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShoeId = table.Column<Guid>(type: "uuid", nullable: true),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DistanceKm = table.Column<decimal>(type: "numeric", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    MovingTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    AveragePacePerKm = table.Column<TimeSpan>(type: "interval", nullable: true),
                    AverageHeartRate = table.Column<int>(type: "integer", nullable: true),
                    MaxHeartRate = table.Column<int>(type: "integer", nullable: true),
                    AverageCadence = table.Column<int>(type: "integer", nullable: true),
                    ElevationGainMeters = table.Column<decimal>(type: "numeric", nullable: true),
                    RatePerceivedExertion = table.Column<int>(type: "integer", nullable: true),
                    Source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExternalActivityId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedRuns_Athletes_AthleteId",
                        column: x => x.AthleteId,
                        principalTable: "Athletes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedRuns_PlannedSessions_PlannedSessionId",
                        column: x => x.PlannedSessionId,
                        principalTable: "PlannedSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CompletedRuns_Shoes_ShoeId",
                        column: x => x.ShoeId,
                        principalTable: "Shoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachDecisions_AthleteId",
                table: "CoachDecisions",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedRuns_AthleteId",
                table: "CompletedRuns",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedRuns_PlannedSessionId",
                table: "CompletedRuns",
                column: "PlannedSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedRuns_ShoeId",
                table: "CompletedRuns",
                column: "ShoeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedSessions_TrainingPlanId",
                table: "PlannedSessions",
                column: "TrainingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadinessEntries_AthleteId",
                table: "ReadinessEntries",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_AthleteId",
                table: "Shoes",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingGoals_AthleteId",
                table: "TrainingGoals",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlans_AthleteId",
                table: "TrainingPlans",
                column: "AthleteId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlans_GoalId",
                table: "TrainingPlans",
                column: "GoalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachDecisions");

            migrationBuilder.DropTable(
                name: "CompletedRuns");

            migrationBuilder.DropTable(
                name: "ReadinessEntries");

            migrationBuilder.DropTable(
                name: "PlannedSessions");

            migrationBuilder.DropTable(
                name: "Shoes");

            migrationBuilder.DropTable(
                name: "TrainingPlans");

            migrationBuilder.DropTable(
                name: "TrainingGoals");

            migrationBuilder.DropTable(
                name: "Athletes");
        }
    }
}
