using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Domain.Enums;
using CORE.DOMAIN.Enums;

namespace CORE.Application.UseCases;

public class CriarPartida
{
    private readonly IPartidaRepository partidaRepository;
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly IRegiaoRepository regiaoRepository;

    public CriarPartida(
        IPartidaRepository partidaRepository,
        ICivilizacaoRepository civilizacaoRepository,
        IRegiaoRepository regiaoRepository)
    {
        this.partidaRepository = partidaRepository;
        this.civilizacaoRepository = civilizacaoRepository;
        this.regiaoRepository = regiaoRepository;
    }

    public async Task<Partida> ExecutarAsync(List<string> nomesCivilizacoes)
    {
        if (nomesCivilizacoes.Count < 2)
            throw new Exception("Uma partida precisa de pelo menos 2 civilizações.");

        var partida = new Partida($"Partida {DateTime.UtcNow:dd/MM/yyyy HH:mm}");

        foreach (var nome in nomesCivilizacoes)
        {
            var civilizacao = new Civilizacao(nome);
            var regiaoInicial = new Regiao("Vale Inicial", TipoTerreno.Planicie, civilizacao.Id);
            regiaoInicial.MarcarComoControlada();

            await civilizacaoRepository.AddAsync(civilizacao);
            await regiaoRepository.AddAsync(regiaoInicial);

            partida.AdicionarCivilizacao(civilizacao);
        }

        await partidaRepository.AddAsync(partida);
        return partida;
    }
}