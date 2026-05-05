# Athlete MCP Server - Test Suite

## Test Infrastructure ✅

The following test infrastructure has been successfully created:

### Test Project Setup
- **Project File**: `tests/AthleteMcpServer.Tests/AthleteMcpServer.Tests.csproj`
- **Framework**: TUnit 0.6.0
- **Test Style**: Integration tests using `WebApplicationFactory`
- **Database**: In-Memory EF Core for fast test execution
- **Structure**: `/tests/<feature>/<action>/Success.cs` and `Failure.cs`

### Core Infrastructure Files
1. **TestWebApplicationFactory.cs** - Configures the test host with in-memory database
2. **AthleteBuilder.cs** - Test data builder for Athlete entities
3. **TrainingGoalBuilder.cs** - Test data builder for TrainingGoal entities

## Test Coverage Created

The following test files have been scaffolded (⚠️ need parameter signature fixes):

### Athletes Feature
- ✅ `Athletes/CreateAthlete/Success.cs`
- ✅ `Athletes/CreateAthlete/Failure.cs`

### Goals Feature
- ✅ `Goals/CreateTrainingGoal/Success.cs`
- ✅ `Goals/CreateTrainingGoal/Failure.cs`

### Activities Feature
- ✅ `Activities/LogCompletedRun/Success.cs`
- ✅ `Activities/LogCompletedRun/Failure.cs`

### TrainingPlans Feature
- ✅ `TrainingPlans/CreateTrainingPlan/Success.cs`
- ✅ `TrainingPlans/CreateTrainingPlan/Failure.cs`

### Readiness Feature
- ✅ `Readiness/AddReadinessEntry/Success.cs`
- ✅ `Readiness/AddReadinessEntry/Failure.cs`

### Coaching Feature
- ✅ `Coaching/CheckFatigueRisk/Success.cs`
- ✅ `Coaching/CheckFatigueRisk/Failure.cs`

## ⚠️ Known Issues

### Parameter Signature Mismatches

The test files were created with assumed parameter names that don't match the actual use case implementations. The following corrections are needed:

#### CreateAthleteRequest
**Expected**: `(string Name, DateTime DateOfBirth, ExperienceLevel ExperienceLevel, UnitSystem PreferredUnits)`
**Actual in code**: Check `src/AthleteMcpServer/Application/Athletes/CreateAthlete.cs`

#### CreateTrainingGoalRequest  
**Expected**: `(Guid AthleteId, RaceDistance RaceDistance, DateTime TargetDate, TimeSpan? TargetTime)`
**Actual in code**: Uses `RaceDistance Distance` and `DateOnly TargetDate`

#### LogCompletedRunRequest
**Expected**: `(Guid AthleteId, DateTime Date, double DistanceMeters, TimeSpan Duration, RunSource Source, int PerceivedExertion, Guid? ShoeId)`
**Actual in code**: Uses `DateTimeOffset StartedAt`, `decimal DistanceKm`, `int? RatePerceivedExertion`

#### CreateTrainingPlanRequest
**Expected**: `(Guid GoalId, DateTime StartDate, int DurationWeeks, double PeakWeeklyMileage)`
**Actual in code**: Check actual signature

#### AddReadinessEntryRequest
**Expected**: `(Guid AthleteId, ReadinessEntryType EntryType, double Value, DateTime RecordedAt, HealthStatus? HealthStatus)`
**Actual in code**: Check actual signature

#### CheckFatigueRiskRequest
**Expected**: `(Guid AthleteId, int LookbackDays)`
**Actual in code**: Check actual signature

## Next Steps

### Immediate Actions Required

1. **Fix Request Signatures** - Update all test files to match actual use case parameter names and types
2. **Verify Enum Values** - Ensure all enum values used in tests match domain definitions ✅ (partially done for RaceDistance and ReadinessEntryType)
3. **Build Verification** - Run `dotnet build tests\AthleteMcpServer.Tests\AthleteMcpServer.Tests.csproj`
4. **Test Execution** - Run `dotnet test tests\AthleteMcpleteMcpServer.Tests\AthleteMcpServer.Tests.csproj`

### Additional Test Coverage Needed

The following use cases still need tests:

#### Athletes
- [ ] GetAthleteRequest
- [ ] UpdateAthleteRequest  
- [ ] GetAthleteProfileRequest
- [ ] UpdateAthleteProfileRequest

#### Goals
- [ ] AbandonTrainingGoalRequest
- [ ] CompleteTrainingGoalRequest

#### TrainingPlans
- [ ] MoveSessionRequest
- [ ] ReplaceSessionRequest
- [ ] ScheduleSessionRequest
- [ ] SkipSessionRequest

#### Activities
- [ ] AttachRunToSessionRequest
- [ ] ImportFitRunRequest

#### Readiness
- [ ] GetReadinessSummaryRequest

#### Coaching
- [ ] GenerateWeeklyReviewRequest
- [ ] AdjustPlanAfterRunRequest

## Workflow for Adding New Tests

Use the TDD workflow documented in the root `TDD_WORKFLOW.md`:

1. Define the feature and expected behavior
2. Write Success.cs and Failure.cs with data-driven tests
3. Run tests (they should fail - RED)
4. Implement the use case code
5. Run tests (they should pass - GREEN)
6. Refactor and commit

## Test Conventions

- ✅ Use TUnit with `[Arguments(...)]` for data-driven tests
- ✅ Separate Success and Failure scenarios into different files
- ✅ Use `WebApplicationFactory` for integration testing
- ✅ Use In-Memory database for fast test execution
- ✅ Follow naming convention: `/tests/<Feature>/<UseCase>/<Outcome>.cs`
- ✅ Use test builders for complex entity construction

## Running Tests

```powershell
# Build the test project
dotnet build tests\AthleteMcpServer.Tests\AthleteMcpServer.Tests.csproj

# Run all tests
dotnet test tests\AthleteMcpServer.Tests\AthleteMcpServer.Tests.csproj

# Run specific test file
dotnet test tests\AthleteMcpServer.Tests\AthleteMcpServer.Tests.csproj --filter "FullyQualifiedName~CreateAthlete"
```

## Current Status

✅ **PROJECT REFERENCE WORKING** - The main project can now be referenced by tests!

### What Was Fixed
1. ✅ Made `SelfContained=true` conditional (only for Release/Publish)
2. ✅ Added `InternalsVisibleTo` attribute for test project
3. ✅ Made `Program` class accessible via `public partial class Program`
4. ✅ Updated test project to use proper `<ProjectReference>`

### ⚠️ Remaining Issues

**BUILD FAILING** - The scaffolded test files have **completely incorrect parameter signatures**. The actual use case implementations have very different parameters than what was assumed.

**Key Mismatches:**
- Parameters use `DateOnly` not `DateTime`
- Parameters use `decimal` not `double`  
- Parameter names are different (e.g., `Distance` not `RaceDistance`, `DaysToAnalyze` not `LookbackDays`)
- Parameter types are different (e.g., `Severity` not `double Value`)
- Domain entities use constructors, not object initializers

**See `USE_CASE_SIGNATURES.md` for actual signatures.**

### Recommended Approach

**Option 1: Start Fresh (Recommended)**
Delete the incorrect test files and use the TDD workflow to create proper tests one feature at a time:
```powershell
Remove-Item tests\AthleteMcpServer.Tests\Athletes\* -Recurse -Force
Remove-Item tests\AthleteMcpServer.Tests\Goals\* -Recurse -Force  
Remove-Item tests\AthleteMcpServer.Tests\Activities\* -Recurse -Force
Remove-Item tests\AthleteMcpServer.Tests\TrainingPlans\* -Recurse -Force
Remove-Item tests\AthleteMcpServer.Tests\Readiness\* -Recurse -Force
Remove-Item tests\AthleteMcpServer.Tests\Coaching\* -Recurse -Force
```

Then follow `TDD_WORKFLOW.md` to create each feature's tests properly.

**Option 2: Fix Systematically**
Keep the test files and fix each one by reading the actual use case implementation and updating parameters/assertions to match.


