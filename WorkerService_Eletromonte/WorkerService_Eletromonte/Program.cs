using WorkerService_Eletromonte;
using WorkerService_Eletromonte.Infraestructure.http;
using WorkerService_Eletromonte.Infraestructure.Persistence;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/worker.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = Host.CreateApplicationBuilder(args);

// Carrega a connection string do appsettings.json
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("Supabase");

// Registra os serviços
builder.Services.AddHttpClient<ApiService>();
builder.Services.AddSingleton(new SupabaseService(connectionString));
builder.Services.AddHostedService<Worker>();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var host = builder.Build();
host.Run();
