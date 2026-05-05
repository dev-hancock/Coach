# Athlete Training Application - Architecture

## Project Structure

The solution has been restructured into a Clean Architecture layout with the following projects:

### **src/Athlete.Domain**
- **Purpose**: Core domain entities, value objects, enums, interfaces, and domain logic
- **Dependencies**: Ardalis.Specification (for repository pattern)
- **Namespace**: `Athlete.Domain`
- **Key Folders**:
  - `/Athletes` - Athlete aggregate
  - `/Activities` - CompletedRun aggregate
  - `/Goals` - TrainingGoal aggregate
  - `/TrainingPlans` - TrainingPlan and PlannedSession aggregates
  - `/Fatigue` - FatigueEntry aggregate
  - `/Injuries` - InjuryEntry aggregate
  - `/Recovery` - RecoveryEntry aggregate
  - `/Equipment` - Shoe aggregate
  - `/Coaching` - CoachDecision aggregate
  - `/Health` - Shared health value objects (Severity, etc.)
  - `/Repositories` - Repository interfaces
  - `/Specifications` - Specification pattern classes

### **src/Athlete.Application**
- **Purpose**: Application layer with CQRS command/query handlers using MediatR
- **Dependencies**: 
  - Athlete.Domain
  - MediatR
- **Namespace**: `Athlete.Application`
- **Key Folders**:
  - `/Athletes` - Athlete CRUD operations
  - `/Activities` - Run logging/importing
  - `/Goals` - Goal management
  - `/TrainingPlans` - Plan and session management
  - `/Fatigue` - Fatigue tracking
  - `/Injuries` - Injury tracking
  - `/Recovery` - Recovery tracking
  - `/Coaching` - Coaching analysis and decisions

### **src/Athlete.Infrastructure**
- **Purpose**: Infrastructure concerns - data access, EF Core, repositories
- **Dependencies**:
  - Athlete.Domain
  - Athlete.Application
  - Microsoft.EntityFrameworkCore
  - Npgsql.EntityFrameworkCore.PostgreSQL
  - Ardalis.Specification.EntityFrameworkCore
- **Namespace**: `Athlete.Infrastructure`
- **Key Folders**:
  - `/Data` - AthleteDbContext
  - `/Migrations` - EF Core migrations
  - `/Repositories` - EF repository implementations
  - `DependencyInjection.cs` - Service registration

### **src/Athlete.Api**
- **Purpose**: REST API for web/mobile clients
- **Dependencies**:
  - Athlete.Application
  - Athlete.Infrastructure
- **Namespace**: `Athlete.Api`
- **Status**: Template created, needs controller implementation

### **src/Athlete.Mcp**
- **Purpose**: Model Context Protocol server for AI integration
- **Dependencies**:
  - Athlete.Application
  - Athlete.Infrastructure
  - ModelContextProtocol
  - Microsoft.Extensions.Hosting
- **Namespace**: `Athlete.Mcp`
- **Key Files**:
  - `Program.cs` - MCP server host
  - `/Tools` - MCP tool implementations (AthleteTools, RandomNumberTools)
  - `appsettings.json` - Configuration

### **tests/AthleteMcpServer.Tests**
- **Purpose**: Integration tests
- **Status**: Needs updating to reference new projects

## Dependency Flow (Clean Architecture)

```
Athlete.Domain (no dependencies - core business logic)
      ↑
Athlete.Application (depends on Domain)
      ↑
Athlete.Infrastructure (depends on Domain + Application)
      ↑
Athlete.Api + Athlete.Mcp (depend on Application + Infrastructure)
```

## Next Steps

1. Fix namespace references across all projects
2. Update test project to reference new structure
3. Implement API controllers in Athlete.Api
4. Verify migrations work in new structure
5. Test both API and MCP server functionality

## Benefits

✅ **Separation of Concerns**: Domain logic isolated from infrastructure
✅ **Testability**: Easy to test domain and application layers
✅ **Flexibility**: Can swap infrastructure (e.g., change from PostgreSQL to SQL Server)
✅ **Multiple Interfaces**: Both traditional API and AI (MCP) can use same business logic
✅ **Maintainability**: Clear boundaries and dependencies
