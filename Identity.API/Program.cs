using Identity.API.Data;
using Identity.API.Interfaces;
using Identity.API.Models;
using Identity.API.Repositories;
using Identity.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Prometheus;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core
builder.Services.AddDbContext<BankDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Sql")));

builder.Services.AddSingleton<FileKeyVault>();
builder.Services.AddSingleton<IKeyVault>(sp => sp.GetRequiredService<FileKeyVault>());
builder.Services.AddScoped<IKeyRotationRepository, KeyRotationEfRepository>();
//builder.Services.AddScoped<ITokenService, TokenService>();
//builder.Services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
//builder.Services.AddSingleton<ISecureRandom, SecureRandom>();

builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("TokenOptions"));


// HealthChecks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddSqlServer(builder.Configuration.GetConnectionString("Sql")!);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHttpMetrics();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseReDoc(c =>
    {
        c.RoutePrefix = "docs";
        c.SpecUrl("/swagger/v1/swagger.json");
    });
}

app.MapControllers();
app.MapHealthChecks("/health");
app.MapMetrics();
app.MapGet("/", () => Results.Ok(new { message = "Hello from Bank.Api" }));

app.Run();
