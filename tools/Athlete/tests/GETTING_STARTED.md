# Your First TDD Test - CreateAthlete Use Case

Let's create your first test following the TDD workflow. We'll start with **CreateAthlete** since it's the simplest.

## Step 1: Understand the Use Case

**File**: `src/AthleteMcpServer/Application/Athletes/CreateAthlete.cs`

**Request Signature**:
```csharp
public sealed record CreateAthleteRequest(string Name) 
    : IRequest<CreateAthleteResponse>;
```

**Response**:
```csharp
public sealed record CreateAthleteResponse(Guid AthleteId, string Name);
```

**Domain Constructor**:
```csharp
public Athlete(string name)
{
    ArgumentException.ThrowIfNullOrWhiteSpace(name);
    Name = name;
}
```

## Step 2: Create Success Test File

Create: `tests/AthleteMcpServer.Tests/Athletes/CreateAthlete/Success.cs`

```csharp
using AthleteMcpServer.Application.Athletes;
using AthleteMcpServer.Tests.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TUnit.Assertions;
using TUnit.Core;

namespace AthleteMcpServer.Tests.Athletes.CreateAthlete;

public class Success
{
    private TestWebApplicationFactory _factory = null!;
    private IMediator _mediator = null!;

    [Before(Test)]
    public async Task Setup()
    {
        _factory = new TestWebApplicationFactory();
        var scope = _factory.Services.CreateScope();
        _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    }

    [After(Test)]
    public async Task Cleanup()
    {
        await _factory.DisposeAsync();
    }

    [Test]
    [Arguments("John Doe")]
    [Arguments("Jane Smith")]
    [Arguments("Miguel Ángel")]
    [Arguments("李明")]
    public async Task CreateAthlete_WithValidName_ReturnsAthleteIdAndName(string name)
    {
        // Arrange
        var request = new CreateAthleteRequest(name);

        // Act
        var result = await _mediator.Send(request);

        // Assert
        await Assert.That(result.AthleteId).IsNotEqualTo(Guid.Empty);
        await Assert.That(result.Name).IsEqualTo(name);
    }

    [Test]
    public async Task CreateAthlete_CreatesRecordInDatabase()
    {
        // Arrange
        var request = new CreateAthleteRequest("Database Test");

        // Act
        var result = await _mediator.Send(request);

        // Assert - verify we can retrieve it
        var dbContext = _factory.Services.CreateScope().ServiceProvider
            .GetRequiredService<AthleteMcpServer.Data.AthleteDbContext>();

        var athlete = await dbContext.Athletes.FindAsync(result.AthleteId);

        await Assert.That(athlete).IsNotNull();
        await Assert.That(athlete!.Name).IsEqualTo("Database Test");
    }
}
```

## Step 3: Create Failure Test File

Create: `tests/AthleteMcpServer.Tests/Athletes/CreateAthlete/Failure.cs`

```csharp
using AthleteMcpServer.Application.Athletes;
using AthleteMcpServer.Tests.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TUnit.Assertions;
using TUnit.Core;

namespace AthleteMcpServer.Tests.Athletes.CreateAthlete;

public class Failure
{
    private TestWebApplicationFactory _factory = null!;
    private IMediator _mediator = null!;

    [Before(Test)]
    public async Task Setup()
    {
        _factory = new TestWebApplicationFactory();
        var scope = _factory.Services.CreateScope();
        _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    }

    [After(Test)]
    public async Task Cleanup()
    {
        await _factory.DisposeAsync();
    }

    [Test]
    [Arguments("")]
    [Arguments("   ")]
    public async Task CreateAthlete_WithEmptyOrWhitespaceName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var request = new CreateAthleteRequest(invalidName);

        // Act & Assert
        await Assert.That(async () => await _mediator.Send(request))
            .ThrowsException()
            .OfType<ArgumentException>();
    }

    [Test]
    public async Task CreateAthlete_WithNullName_ThrowsArgumentException()
    {
        // Arrange
        var request = new CreateAthleteRequest(null!);

        // Act & Assert
        await Assert.That(async () => await _mediator.Send(request))
            .ThrowsException()
            .OfType<ArgumentException>();
    }
}
```

## Step 4: Run the Tests (RED)

```powershell
dotnet test tests\AthleteMcpServer.Tests\AthleteMcpServer.Tests.csproj --filter "FullyQualifiedName~CreateAthlete"
```

**Expected**: Tests should PASS (since CreateAthlete is already implemented) ✅

If they fail, that's your RED phase - the code needs fixing.

## Step 5: Verify Implementation (GREEN)

Check that `CreateAthleteHandler` correctly:
1. ✅ Creates an Athlete entity
2. ✅ Saves to repository
3. ✅ Returns AthleteId and Name

## Step 6: Refactor (if needed)

Look for improvements:
- Any code duplication?
- Any performance issues?
- Any missing edge cases?

## Step 7: Document

Add XML comments or update README with:
- What the use case does
- Edge cases handled
- Validation rules

## Step 8: Commit

```bash
git add tests/AthleteMcpServer.Tests/Athletes/CreateAthlete/
git commit -m "test: add TDD tests for CreateAthlete use case

- Add Success tests with data-driven name validation
- Add Failure tests for null/empty/whitespace names
- Verify database persistence
- All tests passing"
```

---

## Quick Commands

**Build tests**:
```powershell
dotnet build tests\AthleteMcpServer.Tests\AthleteMcpServer.Tests.csproj
```

**Run all tests**:
```powershell
dotnet test tests\AthleteMcpServer.Tests\AthleteMcpServer.Tests.csproj
```

**Run specific test**:
```powershell
dotnet test --filter "FullyQualifiedName~CreateAthlete.Success"
```

**Run with verbose output**:
```powershell
dotnet test tests\AthleteMcpServer.Tests\AthleteMcpServer.Tests.csproj --logger "console;verbosity=detailed"
```

---

## Next Use Cases to Test

After CreateAthlete, continue with:

1. **CreateTrainingGoal** - Practice with more complex parameters
2. **LogCompletedRun** - Work with decimal, DateTimeOffset
3. **CreateTrainingPlan** - Multi-parameter validation
4. **AddReadinessEntry** - Work with enums and DateOnly
5. **CheckFatigueRisk** - Query with lookback logic

Follow this same pattern for each use case! 🎯
