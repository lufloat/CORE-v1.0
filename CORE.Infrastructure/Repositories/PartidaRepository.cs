using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CORE.Infrastructure.Repositories;

public class PartidaRepository : IPartidaRepository
{
    private readonly AppDbContext context;

    public PartidaRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Partida partida)
    {
        await context.Partidas.AddAsync(partida);
        await context.SaveChangesAsync();
    }

    public async Task<Partida?> GetByIdAsync(Guid id)
    {
        return await context.Partidas
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Partida?> GetByIdComCivilizacoesAsync(Guid id)
    {
        return await context.Partidas
            .Include(p => p.Civilizacoes)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(Partida partida)
    {
        context.Partidas.Update(partida);
        await context.SaveChangesAsync();
    }
}