using CORE.Domain.Entities;

namespace CORE.Application.Interfaces;

public interface ICivilizacaoRepository
{
    Task AddAsync(Civilizacao civilizacao);
    Task<Civilizacao?> GetByIdAsync(Guid id);
    Task UpdateAsync(Civilizacao civilizacao);
}