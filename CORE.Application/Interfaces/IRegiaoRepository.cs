using CORE.Domain.Entities;

namespace CORE.Application.Interfaces;

public interface IRegiaoRepository
{
    Task<IEnumerable<Regiao>> GetControladasAsync(Guid civilizacaoId);
    Task AddAsync(Regiao regiao);
    Task UpdateAsync(Regiao regiao);
    Task<IEnumerable<Regiao>> GetAllByPartidaAsync(Guid partidaId);
}