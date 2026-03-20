using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Infrastructure.Services;

namespace CORE.Application.UseCases;

public class AvancarTurnoCivilizacao
{
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly IRegiaoRepository regiaoRepository;
    private readonly IAService iaservice;

    public AvancarTurnoCivilizacao(
        ICivilizacaoRepository civilizacaoRepository,
        IRegiaoRepository regiaoRepository,
        IAService iaservice)  // ← ADICIONA AQUI
    {
        this.civilizacaoRepository = civilizacaoRepository;
        this.regiaoRepository = regiaoRepository;
        this.iaservice = iaservice;  // ← E AQUI
    }

    public async Task<Civilizacao> ExecutarAsync(Guid civilizacaoId)
    {
        var civilizacao = await civilizacaoRepository.GetByIdAsync(civilizacaoId);
        if (civilizacao is null)
            throw new Exception("Civilização não encontrada.");

        var regioesControladas = await regiaoRepository.GetControladasAsync(civilizacaoId);
        civilizacao.AvancarTurno(regioesControladas);
        await civilizacaoRepository.UpdateAsync(civilizacao);
        return civilizacao;
    }
}