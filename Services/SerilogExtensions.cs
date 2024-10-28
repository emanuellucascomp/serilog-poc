using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using WeatherApi.Utils;

public static class SerilogExtensions
{
    /// <summary>
    /// Configures Serilog with async sinks and enrichments.
    /// </summary>
    public static void AddCustomSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)  // Avoid noise from framework logs
                .Enrich.FromLogContext()
                .Enrich.WithProperty("applicationName", context.HostingEnvironment.ApplicationName)
                .WriteTo.Sink(new CustomFieldRenamingSink(Console.Out))
                // .Enrich.With<MaskEmailEnricher>()
                // .WriteTo.Async(a => a.Console(new CustomJsonTextFormatter()))  // Async console sink
                .ReadFrom.Configuration(context.Configuration);  // Optional: Load from appsettings.json
        });
    }
}