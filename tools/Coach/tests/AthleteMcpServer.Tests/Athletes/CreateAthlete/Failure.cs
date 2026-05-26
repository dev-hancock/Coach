using AthleteMcpServer.Tests.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AthleteMcpServer.Tests.Athletes.CreateAthlete;

public class Failure
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
    [Arguments("")]
    [Arguments("   ")]
    public async Task CreateAthlete_WithEmptyOrWhitespaceName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var request = new CreateAthleteRequest(invalidName);

        // Act & Assert
        await Assert.That(async () => await _mediator.Send(request))
            .ThrowsException();
    }

    [Test]
    public async Task CreateAthlete_WithNullName_ThrowsArgumentException()
    {
        // Arrange
        var request = new CreateAthleteRequest(null!);

        // Act & Assert
        await Assert.That(async () => await _mediator.Send(request))
            .ThrowsException();
    }
}
