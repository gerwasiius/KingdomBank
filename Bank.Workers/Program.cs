using Bank.Workers;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

// 1) Konfiguriši Serilog (čita iz appsettings.*, Console sink, itd.)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

// 2) Uveži Serilog u Microsoft logging pipeline
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(dispose: true);

// 3) Tvoj worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

try
{
    Log.Information("Starting worker host");
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
