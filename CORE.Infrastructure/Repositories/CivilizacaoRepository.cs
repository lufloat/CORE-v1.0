using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CORE.Infrastructure.Repositories;

public class CivilizacaoRepository : ICivilizacaoRepository
{
    private readonly AppDbContext context;

    public CivilizacaoRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Civilizacao civilizacao)
    {
        await context.Civilizacoes.AddAsync(civilizacao);
        await context.SaveChangesAsync();
    }

    public async Task<Civilizacao?> GetByIdAsync(Guid id)
    {
        return await context.Civilizacoes.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(Civilizacao civilizacao)
    {
        context.Civilizacoes.Update(civilizacao);
        await context.SaveChangesAsync();
    }
}