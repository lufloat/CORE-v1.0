using CORE.Domain.Entities;

namespace CORE.Application.Interfaces;

public interface IRegiaoRepository
{
    Task<List<Regiao>> GetControladasAsync(Guid civilizacaoId);

    // ✅ NOVO: busca todas as regiões da civ (controladas ou não)
    Task<List<Regiao>> GetByCivilizacaoIdAsync(Guid civilizacaoId);

    Task AddAsync(Regiao regiao);
    Task UpdateAsync(Regiao regiao);
    Task<List<Regiao>> GetByPartidaAsync(Guid partidaId);
}