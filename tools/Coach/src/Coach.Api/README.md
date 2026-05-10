# Coach.Api

A RESTful API for managing workout plans, built using the REPR (Request-Endpoint-Response) pattern with minimal APIs in ASP.NET Core.

## Architecture

This project follows the REPR pattern, which organizes code by feature rather than by technical layer. Each feature is self-contained in its own file with:

- **Request**: The input data structure (Query/Command)
- **Endpoint**: The HTTP endpoint definition
- **Response**: The output data structure (DTO)

Additionally, each feature includes:
- **Handler**: MediatR request handler for business logic
- **Validator**: FluentValidation rules (where applicable)

## Features

### WorkoutPlans

Complete CRUD operations for workout plans:

- **GET /api/workout-plans** - Get all workout plans
- **GET /api/workout-plans/{id}** - Get a specific workout plan by ID
- **POST /api/workout-plans** - Create a new workout plan
- **PUT /api/workout-plans/{id}** - Update an existing workout plan
- **DELETE /api/workout-plans/{id}** - Delete a workout plan

## Tech Stack

- **.NET 10** - Latest .NET framework
- **ASP.NET Core Minimal APIs** - Lightweight HTTP APIs
- **MediatR** - Mediator pattern implementation
- **FluentValidation** - Input validation
- **Entity Framework Core** - ORM for database access
- **SQLite** - Lightweight database for development
- **Swashbuckle** - OpenAPI/Swagger documentation

## Project Structure

```
Coach.Api/
├── Data/
│   └── CoachDbContext.cs          # Entity Framework DbContext
├── Features/
│   └── WorkoutPlans/
│       ├── GetAllWorkoutPlans.cs   # Get all endpoint
│       ├── GetWorkoutPlanById.cs   # Get by ID endpoint
│       ├── CreateWorkoutPlan.cs    # Create endpoint with validation
│       ├── UpdateWorkoutPlan.cs    # Update endpoint with validation
│       └── DeleteWorkoutPlan.cs    # Delete endpoint
├── Program.cs                      # Application entry point
└── appsettings.json               # Configuration
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Visual Studio 2026 or VS Code

### Running the API

1. Navigate to the project directory:
   ```bash
   cd src/Coach.Api
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Access Swagger UI:
   ```
   https://localhost:<port>/swagger
   ```

The database (coach.db) will be created automatically on first run using SQLite.

## Configuration

Database connection string can be configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=coach.db"
  }
}
```

## API Examples

### Create a Workout Plan

```bash
POST /api/workout-plans
Content-Type: application/json

{
  "name": "Beginner Running Plan",
  "description": "A 12-week plan for beginner runners"
}
```

### Get All Workout Plans

```bash
GET /api/workout-plans
```

### Update a Workout Plan

```bash
PUT /api/workout-plans/{id}
Content-Type: application/json

{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Updated Running Plan",
  "description": "Updated description"
}
```

## Validation Rules

### Workout Plan

- **Name**: Required, maximum 200 characters
- **Description**: Optional, maximum 1000 characters

## Development

### Adding a New Feature

To add a new feature following the REPR pattern:

1. Create a new folder under `Features/` for your feature
2. Create a file for each endpoint (e.g., `GetItems.cs`, `CreateItem.cs`)
3. In each file, define:
   - Request record (Query or Command)
   - Response DTO record
   - Validator class (if needed)
   - Handler class
   - Endpoint mapping extension method
4. Register the endpoint in `Program.cs`

### Example Feature Structure

```csharp
public static class CreateItem
{
    public record Command(string Name) : IRequest<ItemDto>;
    public record ItemDto(Guid Id, string Name);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, ItemDto>
    {
        // Implementation
    }

    public static IEndpointRouteBuilder MapCreateItem(this IEndpointRouteBuilder endpoints)
    {
        // Endpoint mapping
    }
}
```

## Dependencies

- Coach.Domain - Domain entities and business rules

## Future Enhancements

- Add authentication and authorization
- Implement caching
- Add pagination for list endpoints
- Implement filtering and sorting
- Add unit and integration tests
- Switch to SQL Server or PostgreSQL for production
- Add logging and telemetry
- Implement CQRS with separate read and write models
