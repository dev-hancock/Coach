using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AthleteMcpServer.Data;
using AthleteMcpServer.Infrastructure;

namespace AthleteMcpServer.Tests.Infrastructure;

public class TestHostFactory : IDisposable
{
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";
    private IHost? _host;

    public IServiceProvider Services
    {
        get
        {
            if (_host == null)
            {
                _host = CreateHost();
            }
            return _host.Services;
        }
    }

    private IHost CreateHost()
    {
        var builder = Host
            .CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddRepositories();

                services.AddMediatR(cfg =>
                    cfg.RegisterServicesFromAssemblyContaining<Program>());

                services.AddDbContext<AthleteDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_databaseName);
                });
            });

        return builder.Build();
    }

    public void Dispose()
    {
        _host?.Dispose();
    }
}
