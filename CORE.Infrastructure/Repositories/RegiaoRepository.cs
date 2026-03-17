using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CORE.Infrastructure.Repositories;

public class RegiaoRepository : IRegiaoRepository
{
    private readonly AppDbContext context;

    public RegiaoRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<Regiao>> GetControladasAsync(Guid civilizacaoId)
    {
        return await context.Regioes
            .Where(r => r.Controlada && r.CivilizacaoId == civilizacaoId)
            .ToListAsync();
    }

    public async Task AddAsync(Regiao regiao)
    {
        await context.Regioes.AddAsync(regiao);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Regiao regiao)
    {
        context.Regioes.Update(regiao);
        await context.SaveChangesAsync();
    }
}