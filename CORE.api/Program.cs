using CORE.Application.Interfaces;
using CORE.Application.UseCases;
using CORE.Infrastructure.Persistence;
using CORE.Infrastructure.Repositories;
using CORE.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// HttpClient para IA Python
builder.Services.AddHttpClient<IIAService, IAService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["PythonAI:BaseUrl"] ?? "http://localhost:8000/");
});

// DbContext - lê direto da variável de ambiente do Railway
var connectionString =
    Environment.GetEnvironmentVariable("ConnectionStrings__CoreDb")
    ?? builder.Configuration.GetConnectionString("CoreDb");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repositories
builder.Services.AddScoped<ICivilizacaoRepository, CivilizacaoRepository>();
builder.Services.AddScoped<IRegiaoRepository, RegiaoRepository>();
builder.Services.AddScoped<IPartidaRepository, PartidaRepository>();

// Use Cases
builder.Services.AddScoped<IniciarCivilizacao>();
builder.Services.AddScoped<ExpandirTerritorioCivilizacao>();
builder.Services.AddScoped<AvancarTurnoCivilizacao>();
builder.Services.AddScoped<AplicarDecisaoCivilizacao>();
builder.Services.AddScoped<SimularCivilizacao>();
builder.Services.AddScoped<CriarPartida>();
builder.Services.AddScoped<AvancarTurnoPartida>();
builder.Services.AddScoped<CombaterCivilizacoes>();

var app = builder.Build();

// Migrations automáticas com retry
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var retries = 5;
    while (retries > 0)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch (Exception ex)
        {
            retries--;
            Console.WriteLine($"Falha ao conectar no banco. Tentativas restantes: {retries}. Erro: {ex.Message}");
            if (retries == 0) throw;
            Thread.Sleep(3000);
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();