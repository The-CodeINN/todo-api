using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Exceptions.HandlerMiddleware;
using TodoApi.Extensions;
using TodoApi.Middlewares;
using TodoApi.Repositories;
using TodoApi.RequestHelpers;
using TodoApi.Services;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

var vaultUri = Environment.GetEnvironmentVariable("VAULT_ADDR");
var vaultToken = Environment.GetEnvironmentVariable("VAULT_TOKEN");

var vaultSecretsProvider = new VaultSecretProvider(vaultUri, vaultToken);

// Add services to the container.
builder.Configuration
.AddEnvironmentVariables()
    .AddVaultSecrets(vaultSecretsProvider, "todo", "secret");

var defaultConnection = builder.Configuration["Database"];
Console.WriteLine($"Database connection: {defaultConnection}");

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseNpgsql(defaultConnection));

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddExceptionHandler<CustomExceptionHandlerMiddleware>();

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ValidationMiddleware>();
app.UseExceptionHandler(opt => { });

app.MapControllers();

try
{
    DbInitializer.InitDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();
