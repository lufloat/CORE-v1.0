using CORE.Domain.Entities;

namespace CORE.Application.Interfaces;

public interface IPartidaRepository
{
    Task AddAsync(Partida partida);
    Task<Partida?> GetByIdAsync(Guid id);
    Task<Partida?> GetByIdComCivilizacoesAsync(Guid id);
    Task UpdateAsync(Partida partida);
}