using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Domain.Enums;

namespace CORE.Application.UseCases;

public class AplicarDecisaoCivilizacao
{
    private readonly ICivilizacaoRepository repository;

    public AplicarDecisaoCivilizacao(ICivilizacaoRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Civilizacao> ExecutarAsync(Guid id, TipoDecisao decisao)
    {
        var civilizacao = await repository.GetByIdAsync(id);

        if (civilizacao is null)
            throw new Exception("Civilização não encontrada.");

        civilizacao.AplicarDecisao(decisao);

        await repository.UpdateAsync(civilizacao);

        return civilizacao;
    }
}