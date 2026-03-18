using CORE.Application.Interfaces;
using CORE.Application.UseCases;
using CORE.Infrastructure.Persistence;
using CORE.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CoreDb")));

// Repositories
builder.Services.AddScoped<ICivilizacaoRepository, CivilizacaoRepository>();
builder.Services.AddScoped<IRegiaoRepository, RegiaoRepository>();

// Use Cases
builder.Services.AddScoped<IniciarCivilizacao>();
builder.Services.AddScoped<ExpandirTerritorioCivilizacao>();
builder.Services.AddScoped<AvancarTurnoCivilizacao>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();