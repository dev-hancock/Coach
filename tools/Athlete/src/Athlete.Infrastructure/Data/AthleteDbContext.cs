using Microsoft.EntityFrameworkCore;
using Athlete.Domain.Athletes;
using Athlete.Domain.Goals;
using Athlete.Domain.TrainingPlans;
using Athlete.Domain.Activities;
using Athlete.Domain.Fatigue;
using Athlete.Domain.Injuries;
using Athlete.Domain.Recovery;
using Athlete.Domain.Equipment;
using Athlete.Domain.Coaching;

namespace Athlete.Infrastructure.Data;

/// <summary>
/// Database context for the Athlete MCP Server.
/// </summary>
public sealed class AthleteDbContext : DbContext
{
    public DbSet<Athlete> Athletes => Set<Athlete>();
    public DbSet<TrainingGoal> TrainingGoals => Set<TrainingGoal>();
    public DbSet<TrainingPlan> TrainingPlans => Set<TrainingPlan>();
    public DbSet<PlannedSession> PlannedSessions => Set<PlannedSession>();
    public DbSet<CompletedRun> CompletedRuns => Set<CompletedRun>();
    public DbSet<FatigueEntry> FatigueEntries => Set<FatigueEntry>();
    public DbSet<InjuryEntry> InjuryEntries => Set<InjuryEntry>();
    public DbSet<RecoveryEntry> RecoveryEntries => Set<RecoveryEntry>();
    public DbSet<Shoe> Shoes => Set<Shoe>();
    public DbSet<CoachDecision> CoachDecisions => Set<CoachDecision>();

    public AthleteDbContext(DbContextOptions<AthleteDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Athlete>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.ExperienceLevel)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.PreferredUnits)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(x => x.Notes)
                .HasMaxLength(2_000);
        });

        modelBuilder.Entity<TrainingGoal>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Distance)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Description)
                .HasMaxLength(1_000);

            entity.Property(x => x.TargetTime)
                .HasConversion(
                    v => v.HasValue ? (double?)v.Value.TotalSeconds : null,
                    v => v.HasValue ? TimeSpan.FromSeconds(v.Value) : null);

            entity.HasOne<Athlete>()
                .WithMany()
                .HasForeignKey(x => x.AthleteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TrainingPlan>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Phase)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.HasOne<Athlete>()
                .WithMany()
                .HasForeignKey(x => x.AthleteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<TrainingGoal>()
                .WithMany()
                .HasForeignKey(x => x.GoalId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(x => x.Sessions)
                .WithOne()
                .HasForeignKey(x => x.TrainingPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PlannedSession>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.SessionType)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Intensity)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Description)
                .HasMaxLength(2_000);
        });

        modelBuilder.Entity<CompletedRun>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Source)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.ExternalActivityId)
                .HasMaxLength(200);

            entity.Property(x => x.Notes)
                .HasMaxLength(2_000);

            entity.HasOne<Athlete>()
                .WithMany()
                .HasForeignKey(x => x.AthleteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<PlannedSession>()
                .WithMany()
                .HasForeignKey(x => x.PlannedSessionId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne<Shoe>()
                .WithMany()
                .HasForeignKey(x => x.ShoeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<FatigueEntry>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Level)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Severity)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Notes)
                .HasMaxLength(2_000);

            entity.HasOne<Athlete>()
                .WithMany()
                .HasForeignKey(x => x.AthleteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InjuryEntry>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Location)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Severity)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Notes)
                .HasMaxLength(2_000);

            entity.HasOne<Athlete>()
                .WithMany()
                .HasForeignKey(x => x.AthleteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RecoveryEntry>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Quality)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Severity)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Notes)
                .HasMaxLength(2_000);

            entity.HasOne<Athlete>()
                .WithMany()
                .HasForeignKey(x => x.AthleteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Shoe>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Brand)
                .HasMaxLength(100);

            entity.Property(x => x.Model)
                .HasMaxLength(100);

            entity.HasOne<Athlete>()
                .WithMany()
                .HasForeignKey(x => x.AthleteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CoachDecision>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.DecisionType)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Reason)
                .HasMaxLength(4_000)
                .IsRequired();

            entity.Property(x => x.Recommendation)
                .HasMaxLength(4_000);

            entity.HasOne<Athlete>()
                .WithMany()
                .HasForeignKey(x => x.AthleteId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
