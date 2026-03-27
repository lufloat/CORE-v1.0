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

    // Regiões onde Controlada = true (mantido para compatibilidade)
    public async Task<List<Regiao>> GetControladasAsync(Guid civilizacaoId)
        => await context.Regioes
            .Where(r => r.CivilizacaoId == civilizacaoId && r.Controlada)
            .ToListAsync();

    // ✅ NOVO: todas as regiões da civilização, controladas ou não
    public async Task<List<Regiao>> GetByCivilizacaoIdAsync(Guid civilizacaoId)
        => await context.Regioes
            .Where(r => r.CivilizacaoId == civilizacaoId)
            .ToListAsync();

    public async Task AddAsync(Regiao regiao)
    {
        await context.Regioes.AddAsync(regiao);
        await context.SaveChangesAsync();
    }

    // ✅ ESSENCIAL: persiste a transferência de CivilizacaoId no banco
    public async Task UpdateAsync(Regiao regiao)
    {
        context.Regioes.Update(regiao);
        await context.SaveChangesAsync();
    }

    // ✅ CORRIGIDO: usa join em vez de navegação .Civilizacao que não existe na entidade
    public async Task<List<Regiao>> GetByPartidaAsync(Guid partidaId)
    {
        var civIds = await context.Civilizacoes
            .Where(c => c.PartidaId == partidaId)
            .Select(c => c.Id)
            .ToListAsync();

        return await context.Regioes
            .Where(r => civIds.Contains(r.CivilizacaoId))
            .ToListAsync();
    }
}