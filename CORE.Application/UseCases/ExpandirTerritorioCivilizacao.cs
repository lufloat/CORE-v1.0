using CORE.Application.Interfaces;
using CORE.Domain.Entities;

namespace CORE.Application.UseCases;

public class ExpandirTerritorioCivilizacao
{
    private readonly ICivilizacaoRepository civilizacaoRepository;

    public ExpandirTerritorioCivilizacao(ICivilizacaoRepository civilizacaoRepository)
    {
        this.civilizacaoRepository = civilizacaoRepository;
    }

    public async Task<Civilizacao> ExecutarAsync(Guid civilizacaoId)
    {
        var civilizacao = await civilizacaoRepository.GetByIdAsync(civilizacaoId);

        if (civilizacao is null)
            throw new Exception("Civilização não encontrada.");

        // regra de negócio
        civilizacao.AdicionarTerritorio();

        // salva no banco
        await civilizacaoRepository.UpdateAsync(civilizacao);

        return civilizacao;
    }
}