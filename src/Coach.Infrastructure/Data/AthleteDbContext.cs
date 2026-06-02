using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Coach.Domain.Athletes;
using Coach.Domain.Common;
using Coach.Domain.Goals;
using Coach.Domain.Training;
using Coach.Domain.Activities;
using Coach.Domain.Fatigue;
using Coach.Domain.Injuries;
using Coach.Domain.Recovery;
using Coach.Domain.Gear;
using Coach.Domain.Coaching;
using Coach.Domain.Entities;
using Coach.Infrastructure.Identity;
using Coach.Application.Common;
using MediatR;

namespace Coach.Infrastructure.Data;

/// <summary>
/// Database context for the Coach application.
/// </summary>
public sealed class AthleteDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private readonly IPublisher? _publisher;

    public DbSet<Athlete> Athletes => Set<Athlete>();

    public DbSet<TrainingGoal> TrainingGoals => Set<TrainingGoal>();

    public DbSet<TrainingPlan> TrainingPlans => Set<TrainingPlan>();

    public DbSet<PlannedSession> PlannedSessions => Set<PlannedSession>();

    public DbSet<Activity> Activities => Set<Activity>();

    public DbSet<FatigueEntry> Fatigue => Set<FatigueEntry>();

    public DbSet<InjuryEntry> Injuries => Set<InjuryEntry>();

    public DbSet<BodyLocation> BodyLocations => Set<BodyLocation>();

    public DbSet<RecoveryEntry> RecoveryEntries => Set<RecoveryEntry>();

    public DbSet<Equipment> Equipment => Set<Equipment>();

    public DbSet<CoachDecision> CoachDecisions => Set<CoachDecision>();

    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();

    public DbSet<Integration> Integrations => Set<Integration>();

    public AthleteDbContext(DbContextOptions<AthleteDbContext> options, IPublisher? publisher = null)
        : base(options)
    {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Configure Identity tables

        modelBuilder.Entity<Athlete>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Experience)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Unit)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.Property(x => x.Notes)
                .HasMaxLength(2_000);

            // Distance value object conversions
            entity.Property(x => x.CurrentWeeklyDistance)
                .HasConversion(
                    v => v.Kilometers,
                    v => Distance.FromKilometers(v))
                .HasColumnName("CurrentWeeklyDistanceKm")
                .HasPrecision(18, 2);

            entity.Property(x => x.TypicalLongRunDistance)
                .HasConversion(
                    v => v.Kilometers,
                    v => Distance.FromKilometers(v))
                .HasColumnName("TypicalLongRunDistanceKm")
                .HasPrecision(18, 2);

            // Athlete-User relationship: unique index on UserId
            entity.HasIndex(x => x.UserId)
                .IsUnique();
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

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

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

            entity.HasOne<Equipment>()
                .WithMany()
                .HasForeignKey(x => x.EquipmentId)
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

            entity.Property(x => x.Severity)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Notes)
                .HasMaxLength(2_000);

            entity.HasOne<Athlete>()
                .WithMany()
                .HasForeignKey(x => x.AthleteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<BodyLocation>()
                .WithMany()
                .HasForeignKey(x => x.BodyLocationId)
                .OnDelete(DeleteBehavior.Restrict);
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

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

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

        modelBuilder.Entity<BodyLocation>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Region)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Discipline)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(x => x.Side)
                .HasConversion<string>()
                .HasMaxLength(50);
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

        modelBuilder.Entity<WorkoutPlan>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(1000);

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.Property(x => x.UpdatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<Integration>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.AccessToken)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.RefreshToken)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.Metadata)
                .HasMaxLength(4000);

            entity.HasOne(x => x.User)
                .WithMany(u => u.Integrations)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.UserId, x.Type })
                .IsUnique();
        });
    }

    /// <summary>
    /// Intercepts SaveChangesAsync to dispatch domain events from aggregate roots.
    /// This enables event-driven architecture within the domain layer.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Collect all domain events from aggregate roots before saving
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregateRoots
            .SelectMany(a => a.DomainEvents)
            .ToList();

        // Clear events from aggregates (they'll be dispatched below)
        foreach (var aggregate in aggregateRoots)
        {
            aggregate.ClearDomainEvents();
        }

        // Save changes to database
        var result = await base.SaveChangesAsync(cancellationToken);

        // Dispatch domain events AFTER successful save
        // Wrap each domain event in a DomainEventNotification for MediatR
        if (_publisher is not null)
        {
            foreach (var domainEvent in domainEvents)
            {
                var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
                var notification = Activator.CreateInstance(notificationType, domainEvent);

                if (notification is INotification mediatrNotification)
                {
                    await _publisher.Publish(mediatrNotification, cancellationToken);
                }
            }
        }

        return result;
    }
}
