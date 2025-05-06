using Serilog;
using Serilog.Events;

namespace dotnet_starter.Configurations;

public static class SerilogConfiguration
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
            .ReadFrom.Configuration(builder.Configuration) // Read from appsettings.json
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }

    public static WebApplication UseSerilogRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        });

        return app;
    }

    // Helper method to properly close and flush logs
    public static void FlushAndCloseLogger()
    {
        Log.CloseAndFlush();
    }
}