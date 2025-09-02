using Bank.App.Security;
using Bank.App.Security.Interfaces;
using Bank.Infrastructure;
using Bank.Infrastructure.Security;
using Bank.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(dispose: true);

// EF Core (SQL Server) – koristi ConnectionStrings:Sql
builder.Services.AddDbContext<BankDbContext>(opt =>
    opt.UseSqlServer(
        builder.Configuration.GetConnectionString("Sql")
        ?? builder.Configuration["ConnectionStrings:Sql"] // fallback na env var
    )
);

// Generator ključeva
builder.Services.AddSingleton<IKeyGenerator, RsaKeyGenerator>();
builder.Services.AddSingleton<IKeyGenerator, EcdsaKeyGenerator>();

// Vault (FileKeyVault radi s /keys; vidi compose volumen)
builder.Services.AddSingleton<FileKeyVault>();
builder.Services.AddSingleton<IKeyVault>(sp => sp.GetRequiredService<FileKeyVault>());

// Repo (EF)
builder.Services.AddScoped<IKeyRotationRepository, KeyRotationEfRepository>();

// Hosted service (singleton) – NE injektuje repo direktno
builder.Services.AddHostedService<KeyRotationWorker>();

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
