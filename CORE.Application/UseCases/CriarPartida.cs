using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Domain.Enums;
using CORE.DOMAIN.Enums;
using Microsoft.Extensions.Logging;

namespace CORE.Application.UseCases;

public class CriarPartida
{
    private readonly IPartidaRepository partidaRepository;
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly IRegiaoRepository regiaoRepository;
    private readonly ILogger<CriarPartida> logger;

    public CriarPartida(
        IPartidaRepository partidaRepository,
        ICivilizacaoRepository civilizacaoRepository,
        IRegiaoRepository regiaoRepository,
        ILogger<CriarPartida> logger)
    {
        this.partidaRepository = partidaRepository;
        this.civilizacaoRepository = civilizacaoRepository;
        this.regiaoRepository = regiaoRepository;
        this.logger = logger;
    }

    public async Task<Partida> ExecutarAsync(List<string> nomesCivilizacoes)
    {
        if (nomesCivilizacoes.Count < 2)
        {
            logger.LogWarning("Tentativa de criar partida com menos de 2 civilizações.");
            throw new Exception("Uma partida precisa de pelo menos 2 civilizações.");
        }

        var partida = new Partida($"Partida {DateTime.UtcNow:dd/MM/yyyy HH:mm}");
        logger.LogInformation("Criando partida: {Nome}", partida.Nome);

        var posicoes = new List<(int x, int y)>
        {
            (0, 0), (4, 0), (0, 4), (4, 4), (2, 2), (4, 2)
        };

        int indice = 0;
        foreach (var nome in nomesCivilizacoes)
        {
            var civilizacao = new Civilizacao(nome);

            var (x, y) = posicoes[indice % posicoes.Count];
            var regiaoInicial = new Regiao("Vale Inicial", TipoTerreno.Planicie, civilizacao.Id, x, y);
            regiaoInicial.MarcarComoControlada();

            await civilizacaoRepository.AddAsync(civilizacao);
            await regiaoRepository.AddAsync(regiaoInicial);

            partida.AdicionarCivilizacao(civilizacao);
            logger.LogInformation("Civilização criada: {Nome} na posição ({X}, {Y})",
                civilizacao.Nome, x, y);

            indice++;
        }

        await partidaRepository.AddAsync(partida);
        logger.LogInformation("Partida {Nome} salva com {Total} civilizações.",
            partida.Nome, partida.Civilizacoes.Count);

        return partida;
    }
}