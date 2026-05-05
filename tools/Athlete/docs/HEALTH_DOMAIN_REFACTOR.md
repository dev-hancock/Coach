# Health Domain Refactoring Summary

## Overview
Refactored the health tracking domain model from `ReadinessEntry` to `HealthEntry` to better align with user mental models and domain language.

## Key Changes

### Domain Model

#### Before (Confused naming):
- `ReadinessEntry` → Too specific, implied only pre-run readiness
- `ReadinessEntryType` with values: `GeneralFatigue, Soreness, Pain, Illness, Sleep, Stress, Injury`

#### After (Clear, user-focused):
- `HealthEntry` → Covers all health tracking
- `HealthEntryType` with values: `Pain, Fatigue, Illness, Sleep, Stress`
- Severity remains unchanged

### Why It Works

1. **Covers injury + fatigue + illness** - Comprehensive health tracking
2. **Easy to extend** - Can add new types as needed
3. **Matches how users think** - Natural language
4. **Not over-engineered** - Simple, straightforward model

### Files Renamed

#### Domain Layer
- `ReadinessEntry.cs` → `HealthEntry.cs`
- `ReadinessEntryType.cs` → `HealthEntryType.cs`
- `IReadinessEntryRepository.cs` → `IHealthEntryRepository.cs`
- `ReadinessEntryRepository.cs` → `HealthEntryRepository.cs`

#### Specifications
- `ReadinessEntriesBySeveritySpec.cs` → `HealthEntriesBySeveritySpec.cs`
- `ReadinessEntriesRequiringFollowUpSpec.cs` → `HealthEntriesRequiringFollowUpSpec.cs`
- `RecentReadinessEntriesSpec.cs` → `RecentHealthEntriesSpec.cs`

#### Application Layer
- `Application/Readiness/` folder → `Application/Health/`
- `AddReadinessEntry.cs` → `AddHealthEntry.cs`
- `GetReadinessSummary.cs` → `GetHealthSummary.cs`

### Updated Components

1. **DbContext** (`AthleteDbContext.cs`)
   - `ReadinessEntries` → `HealthEntries` DbSet
   - Updated entity configuration

2. **Dependency Injection** (`DependencyInjection.cs`)
   - `IReadinessEntryRepository` → `IHealthEntryRepository` registration
   - `ReadinessEntryRepository` → `HealthEntryRepository` implementation

3. **Application Handlers**
   - `AddReadinessEntryHandler` → `AddHealthEntryHandler`
   - `GetReadinessSummaryHandler` → `GetHealthSummaryHandler`
   - Updated coaching handlers:
     - `GenerateWeeklyReviewHandler` - now uses `HealthEntry`
     - `CheckFatigueRiskHandler` - now uses `HealthEntry` and `HealthEntryType.Fatigue`/`Pain`

4. **Request/Response Types**
   - `AddReadinessEntryRequest` → `AddHealthEntryRequest`
   - `AddReadinessEntryResponse` → `AddHealthEntryResponse`
   - `GetReadinessSummaryRequest` → `GetHealthSummaryRequest`
   - `ReadinessSummaryResponse` → `HealthSummaryResponse`
   - `ReadinessEntryDto` → `HealthEntryDto`

### Database Migration

**Migration**: `20260504120507_RenameReadinessToHealth`

The migration uses `RenameTable` to preserve existing data:

```csharp
migrationBuilder.RenameTable(
    name: "ReadinessEntries",
    newName: "HealthEntries");

migrationBuilder.RenameIndex(
    name: "IX_ReadinessEntries_AthleteId",
    table: "HealthEntries",
    newName: "IX_HealthEntries_AthleteId");
```

### Updated Business Logic

1. **RequiresFollowUp Logic** (in `HealthEntry` constructor):
   ```csharp
   // Before: Pain OR Injury required follow-up
   RequiresFollowUp = severity is Severity.High or Severity.Severe ||
                     entryType is ReadinessEntryType.Pain or ReadinessEntryType.Injury;

   // After: Only Pain requires follow-up (Injury no longer exists as separate type)
   RequiresFollowUp = severity is Severity.High or Severity.Severe ||
                     entryType is HealthEntryType.Pain;
   ```

2. **Fatigue Risk Analysis** (in `CheckFatigueRiskHandler`):
   ```csharp
   // Before: Checked for GeneralFatigue and Pain/Injury
   var fatigueEntries = readinessEntries
       .Where(e => e.EntryType == ReadinessEntryType.GeneralFatigue)
       .ToList();

   var painEntries = readinessEntries
       .Where(e => e.EntryType == ReadinessEntryType.Pain || 
                   e.EntryType == ReadinessEntryType.Injury)
       .ToList();

   // After: Simplified to Fatigue and Pain
   var fatigueEntries = healthEntries
       .Where(e => e.EntryType == HealthEntryType.Fatigue)
       .ToList();

   var painEntries = healthEntries
       .Where(e => e.EntryType == HealthEntryType.Pain)
       .ToList();
   ```

## Validation

✅ Build successful  
✅ All 8 tests passing  
✅ Migration created with data preservation  
✅ Primary constructor pattern maintained across all handlers

## Next Steps

1. Run migration on database: `dotnet ef database update`
2. Update any external documentation referencing "Readiness"
3. Consider adding health entry tests in the future
