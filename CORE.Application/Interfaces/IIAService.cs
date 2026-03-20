using CORE.Domain.Entities;
using CORE.Domain.Enums;

namespace CORE.Application.Interfaces;

public interface IIAService
{
    Task<TipoDecisao> ObterDecisaoAsync(Civilizacao civilizacao);
}