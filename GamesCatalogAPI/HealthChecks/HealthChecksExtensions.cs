using HealthChecks.UI.Client;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection;

public static class ExceptionHandlingExtensions
{
    public static void AddHealthChecks(this WebApplicationBuilder builder, string connectionString)
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddSqlServer(connectionString, tags: new[] { "services", "db" })
            .AddCheck("services", () => HealthCheckResult.Healthy(), tags: new[] { "services" });
    }

    public static void UseHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health/liveness",
            new() { Predicate = x => x.Tags is null, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
        app.MapHealthChecks("/health/readiness",
            new() { Predicate = x => x.Tags is not null, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
    }
}

