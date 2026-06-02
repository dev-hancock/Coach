using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Athlete.Infrastructure.Data;
using Athlete.Mcp.Tools;
using Athlete.Infrastructure.Services;

namespace Athlete.Mcp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var app = CreateHostBuilder(args).Build();

        await MigrateDatabase(app);
       
        await app.RunAsync();
    }

    private static async Task MigrateDatabase(IHost host)
    {
        await using (var scope = host.Services.CreateAsyncScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AthleteDbContext>();

            await context.Database.MigrateAsync();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(ConfigureAppConfiguration)
            .ConfigureLogging(ConfigureLogging)
            .ConfigureServices(ConfigureServices);
    }

    private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder config)
    {
        config.AddEnvironmentVariables();
    }

    private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder logging)
    {
        logging.AddConsole(options =>
        {
            options.LogToStandardErrorThreshold = LogLevel.Trace;
        });
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        ConfigureDatabase(context, services);
        ConfigureRepositories(services);
        ConfigureMediatR(services);
        ConfigureMcpServer(services);
    }

    private static void ConfigureDatabase(HostBuilderContext context, IServiceCollection services)
    {
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Database connection string 'DefaultConnection' not found. " +
                "Set it in appsettings.json or via environment variable 'ConnectionStrings__DefaultConnection'.");

        services.AddDbContext<AthleteDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddRepositories();
    }

    private static void ConfigureMediatR(IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<Program>();
        });
    }

    private static void ConfigureMcpServer(IServiceCollection services)
    {
        services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithTools<RandomNumberTools>()
            .WithTools<AthleteTools>();
    }
}
