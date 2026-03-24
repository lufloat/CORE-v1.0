using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CORE.Application.UseCases;

public class CombaterCivilizacoes
{
    private readonly IPartidaRepository partidaRepository;
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly ILogger<CombaterCivilizacoes> logger;

    public CombaterCivilizacoes(
        IPartidaRepository partidaRepository,
        ICivilizacaoRepository civilizacaoRepository,
        ILogger<CombaterCivilizacoes> logger)
    {
        this.partidaRepository = partidaRepository;
        this.civilizacaoRepository = civilizacaoRepository;
        this.logger = logger;
    }

    public async Task<ResultadoCombate> ExecutarAsync(Guid partidaId, Guid atacanteId, Guid defensorId)
    {
        if (atacanteId == defensorId)
            throw new Exception("Uma civilização não pode atacar a si mesma.");

        var partida = await partidaRepository.GetByIdComCivilizacoesAsync(partidaId);
        if (partida is null)
            throw new Exception("Partida não encontrada.");

        if (partida.Encerrada)
            throw new Exception("Esta partida já foi encerrada.");

        var atacante = partida.Civilizacoes.FirstOrDefault(c => c.Id == atacanteId);
        var defensor = partida.Civilizacoes.FirstOrDefault(c => c.Id == defensorId);

        if (atacante is null)
            throw new Exception("Civilização atacante não encontrada na partida.");

        if (defensor is null)
            throw new Exception("Civilização defensora não encontrada na partida.");

        logger.LogInformation("Combate iniciado: {Atacante} vs {Defensor}", atacante.Nome, defensor.Nome);
        logger.LogInformation("Poder Militar — {Atacante}: {PMA} | {Defensor}: {PMD}",
            atacante.Nome, atacante.PoderMilitar, defensor.Nome, defensor.PoderMilitar);

        var diferenca = Math.Abs(atacante.PoderMilitar - defensor.PoderMilitar);
        Civilizacao vencedor;
        Civilizacao perdedor;

        if (atacante.PoderMilitar >= defensor.PoderMilitar)
        {
            vencedor = atacante;
            perdedor = defensor;
        }
        else
        {
            vencedor = defensor;
            perdedor = atacante;
        }

        // aplica consequências do combate
        var recursosRoubados = Math.Min(15, perdedor.Comida);
        var territoriosRoubados = perdedor.Territorios > 1 ? 1 : 0;

        vencedor.AplicarVitoria(recursosRoubados, territoriosRoubados);
        perdedor.AplicarDerrota(recursosRoubados, territoriosRoubados);

        await civilizacaoRepository.UpdateAsync(atacante);
        await civilizacaoRepository.UpdateAsync(defensor);

        var descricao = $"{vencedor.Nome} venceu o combate contra {perdedor.Nome} " +
                        $"e roubou {recursosRoubados} de comida e {territoriosRoubados} território(s)!";

        logger.LogInformation(descricao);

        if (perdedor.PoderMilitar <= 0)
            logger.LogWarning("{Perdedor} ficou sem poder militar após o combate!", perdedor.Nome);

        return new ResultadoCombate(
            atacante, defensor, vencedor, perdedor,
            diferenca, recursosRoubados, territoriosRoubados, descricao);
    }
}