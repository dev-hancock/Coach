# Use Case Request Signatures - Quick Reference

## Athletes

### CreateAthleteRequest
```csharp
public sealed record CreateAthleteRequest(string Name) 
    : IRequest<CreateAthleteResponse>;
```

## Goals

### CreateTrainingGoalRequest
```csharp
public sealed record CreateTrainingGoalRequest(
    Guid AthleteId,
    RaceDistance Distance,           // NOT RaceDistance
    DateOnly TargetDate,             // NOT DateTime
    TimeSpan? TargetTime = null,
    string? Description = null) 
    : IRequest<CreateTrainingGoalResponse>;
```

## Activities

### LogCompletedRunRequest
```csharp
public sealed record LogCompletedRunRequest(
    Guid AthleteId,
    DateTimeOffset StartedAt,        // NOT DateTime Date
    decimal DistanceKm,              // NOT double DistanceMeters
    TimeSpan Duration,
    int? RatePerceivedExertion = null,  // NOT int PerceivedExertion, NOT RunSource
    Guid? ShoeId = null,
    string? Notes = null) 
    : IRequest<LogCompletedRunResponse>;
```

## TrainingPlans

### CreateTrainingPlanRequest
```csharp
public sealed record CreateTrainingPlanRequest(
    Guid AthleteId,                  // NOT GoalId
    string Name,                     // REQUIRED
    DateOnly StartDate,              // NOT DateTime
    DateOnly EndDate,                // NOT DurationWeeks
    int TrainingDaysPerWeek,         // REQUIRED
    decimal TargetWeeklyDistanceKm,  // NOT PeakWeeklyMileage
    Guid? GoalId = null)             // Optional
    : IRequest<CreateTrainingPlanResponse>;
```

## Readiness

### AddReadinessEntryRequest
```csharp
public sealed record AddReadinessEntryRequest(
    Guid AthleteId,
    DateOnly Date,                   // NOT DateTime RecordedAt
    ReadinessEntryType EntryType,
    Severity Severity,               // REQUIRED, NOT double Value
    string? Notes = null,
    string? BodyLocation = null,
    bool AffectedRunning = false)    // NOT HealthStatus
    : IRequest<AddReadinessEntryResponse>;
```

## Coaching

### CheckFatigueRiskRequest
```csharp
public sealed record CheckFatigueRiskRequest(
    Guid AthleteId,
    int DaysToAnalyze = 14)          // NOT LookbackDays
    : IRequest<CheckFatigueRiskResponse>;
```

## Entity Construction Notes

### CompletedRun
- Uses constructor, NOT property initializers
- Check `Domain/Activities/CompletedRun.cs` for constructor parameters

### Shoe
- Uses constructor, NOT property initializers  
- Check `Domain/Equipment/Shoe.cs` for constructor parameters

### TrainingGoal
- Uses constructor, NOT property initializers
- Check `Domain/Goals/TrainingGoal.cs` for constructor parameters

## Key Differences from Tests

1. **DateOnly vs DateTime**: Most dates use `DateOnly`, not `DateTime`
2. **Decimal vs Double**: Distances/metrics use `decimal`, not `double`
3. **Constructor vs Initializers**: Domain entities use constructors, not object initializers
4. **Different Parameter Names**: Many parameters have different names than assumed
5. **Different Parameter Types**: Many parameters have different types (e.g., Severity instead of double Value)

## Next Steps

Use the TDD workflow (`TDD_WORKFLOW.md`) to properly create tests:

1. Read the actual use case file
2. Understand the request/response signatures  
3. Check domain entity constructors
4. Write Success.cs with correct parameters
5. Write Failure.cs with correct validation scenarios
6. Follow the RED → GREEN → REFACTOR cycle
