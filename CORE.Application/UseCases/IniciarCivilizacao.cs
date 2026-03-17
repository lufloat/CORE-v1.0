using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Domain.Enums;

namespace CORE.Application.UseCases;

public class IniciarCivilizacao
{
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly IRegiaoRepository regiaoRepository;

    public IniciarCivilizacao(
        ICivilizacaoRepository civilizacaoRepository,
        IRegiaoRepository regiaoRepository)
    {
        this.civilizacaoRepository = civilizacaoRepository;
        this.regiaoRepository = regiaoRepository;
    }

    public async Task<Civilizacao> ExecutarAsync(string nome)
    {
        var civilizacao = new Civilizacao(nome);

        var regiaoInicial = new Regiao("Vale Inicial", TipoTerreno.Planicie, civilizacao.Id);
        regiaoInicial.MarcarComoControlada();

        await civilizacaoRepository.AddAsync(civilizacao);
        await regiaoRepository.AddAsync(regiaoInicial);

        return civilizacao;
    }
}