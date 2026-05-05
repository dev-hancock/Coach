using AthleteMcpServer.Application.Athletes;
using AthleteMcpServer.Tests.Infrastructure;
using AthleteMcpServer.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TUnit.Assertions;
using TUnit.Core;

namespace AthleteMcpServer.Tests.Athletes.CreateAthlete;

public class Success
{
    private TestHostFactory _factory = null!;
    private IMediator _mediator = null!;

    [Before(Test)]
    public async Task Setup()
    {
        _factory = new TestHostFactory();
        var scope = _factory.Services.CreateScope();
        _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    }

    [After(Test)]
    public async Task Cleanup()
    {
        _factory.Dispose();
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

        // Assert - verify we can retrieve it using a new scope from the same factory
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AthleteDbContext>();
        var athlete = await dbContext.Athletes.FindAsync(result.AthleteId);

        await Assert.That(athlete).IsNotNull();
        await Assert.That(athlete!.Name).IsEqualTo("Database Test");
    }
}
