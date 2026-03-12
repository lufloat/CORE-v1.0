using CORE.Application.Interfaces;
using CORE.Domain.Entities;

namespace CORE.Application.UseCases;

public class IniciarCivilizacao
{
    private readonly ICivilizacaoRepository repository;

    public IniciarCivilizacao(ICivilizacaoRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Civilizacao> ExecutarAsync(string nome)
    {
        var civilizacao = new Civilizacao(nome);

        await repository.AddAsync(civilizacao);

        return civilizacao;
    }
}