using CORE.Domain.Entities;

namespace CORE.Application.Interfaces;

public interface IRegiaoRepository
{
    Task<IEnumerable<Regiao>> GetControladasAsync();
    Task AddAsync(Regiao regiao);
    Task UpdateAsync(Regiao regiao);
}