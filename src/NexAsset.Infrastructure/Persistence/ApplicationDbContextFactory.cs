using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NexAsset.Infrastructure.Persistence;

/// <summary>
/// Keeps EF tooling independent of the API startup pipeline, which performs runtime-only
/// initialization such as database seeding.
/// </summary>
public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? ReadConnectionString();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new ApplicationDbContext(options);
    }

    private static string ReadConnectionString()
    {
        var workingDirectory = Directory.GetCurrentDirectory();
        var candidates = new[]
        {
            Path.Combine(workingDirectory, "NexAsset.API", "appsettings.json"),
            Path.Combine(workingDirectory, "appsettings.json"),
            Path.Combine(AppContext.BaseDirectory, "appsettings.json")
        };
        var settingsFile = candidates.FirstOrDefault(File.Exists)
            ?? throw new InvalidOperationException("Unable to locate NexAsset.API appsettings.json for EF Core design-time tooling.");

        using var document = JsonDocument.Parse(File.ReadAllText(settingsFile));
        if (!document.RootElement.TryGetProperty("ConnectionStrings", out var connections) ||
            !connections.TryGetProperty("DefaultConnection", out var connection) ||
            string.IsNullOrWhiteSpace(connection.GetString()))
        {
            throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required for EF Core design-time tooling.");
        }

        return connection.GetString()!;
    }
}
