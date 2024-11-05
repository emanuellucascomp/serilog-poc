using System.Reflection;
using Serilog;
using Serilog.Formatting.Json;
using WeatherApi.Formatters;

namespace WeatherApi.Services;

public static class SerilogExtensions
{

    public static void AddCustomSerilog(this IServiceCollection serviceCollection, IHostBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithProperty("applicationName", Assembly.GetEntryAssembly()?.GetName().Name ?? "DefaultApplicationName") 
            .WriteTo.Async(a => a.Sink(new JsonMaskingSink(new JsonFormatter(), Console.Write))).CreateLogger();
        
        builder.UseSerilog();
    }
}