using CORE.Application.Interfaces;
using CORE.Domain.Entities;

namespace CORE.Application.UseCases;

public class CombaterCivilizacoes
{
    private readonly IPartidaRepository partidaRepository;
    private readonly ICivilizacaoRepository civilizacaoRepository;
    private readonly IRegiaoRepository regiaoRepository;

    public CombaterCivilizacoes(
        IPartidaRepository partidaRepository,
        ICivilizacaoRepository civilizacaoRepository,
        IRegiaoRepository regiaoRepository)
    {
        this.partidaRepository = partidaRepository;
        this.civilizacaoRepository = civilizacaoRepository;
        this.regiaoRepository = regiaoRepository;
    }

    public async Task<ResultadoCombate> ExecutarAsync(Guid partidaId, Guid atacanteId, Guid defensorId)
    {
        var partida = await partidaRepository.GetByIdComCivilizacoesAsync(partidaId)
            ?? throw new Exception("Partida não encontrada.");

        var atacante = partida.Civilizacoes.FirstOrDefault(c => c.Id == atacanteId)
            ?? throw new Exception("Civilização atacante não encontrada.");

        var defensor = partida.Civilizacoes.FirstOrDefault(c => c.Id == defensorId)
            ?? throw new Exception("Civilização defensora não encontrada.");

        if (atacante.Id == defensor.Id)
            throw new Exception("Uma civilização não pode atacar a si mesma.");

        // ── Calcula vencedor ──────────────────────────────────────────────
        var forcaAtacante = atacante.PoderMilitar + new Random().Next(1, 20);
        var forcaDefensor = defensor.PoderMilitar + new Random().Next(1, 20);

        var atacanteVenceu = forcaAtacante >= forcaDefensor;
        var vencedor = atacanteVenceu ? atacante : defensor;
        var perdedor = atacanteVenceu ? defensor : atacante;
        var diferenca = Math.Abs(forcaAtacante - forcaDefensor);

        // ── Recursos roubados ─────────────────────────────────────────────
        var recursosRoubados = 0;
        if (atacanteVenceu && perdedor.Comida > 0)
        {
            recursosRoubados = Math.Min(perdedor.Comida, Math.Max(1, diferenca / 2));
            perdedor.RemoverComida(recursosRoubados);
            vencedor.AdicionarComida(recursosRoubados);
        }

        // ── Transferência de territórios ──────────────────────────────────
        // ✅ CORRIGIDO: antes só atualizava o contador int Territorios,
        //    agora busca as entidades Regiao do banco e transfere de verdade
        var territoriosRoubados = 0;

        if (atacanteVenceu && perdedor.Territorios > 1)
        {
            // Quantas regiões transferir (pelo menos 1 se o perdedor tiver mais de 1)
            var qtd = Math.Max(1, diferenca / 10);
            qtd = Math.Min(qtd, perdedor.Territorios - 1); // deixa ao menos 1 para o perdedor

            var regioesDoPerdedor = await regiaoRepository.GetByCivilizacaoIdAsync(perdedor.Id);

            // Pega as primeiras N regiões para transferir
            var regioesSelecionadas = regioesDoPerdedor.Take(qtd).ToList();

            foreach (var regiao in regioesSelecionadas)
            {
                // Transfere a região no domínio
                regiao.TransferirPara(vencedor.Id);

                // Persiste a mudança no banco
                await regiaoRepository.UpdateAsync(regiao);
            }

            territoriosRoubados = regioesSelecionadas.Count;

            // Atualiza os contadores inteiros nas civilizações
            perdedor.RemoverTerritorios(territoriosRoubados);
            vencedor.AdicionarTerritorios(territoriosRoubados);
        }

        // Penalidade de moral para o perdedor
        perdedor.ReduzirMoral(10);

        // Persiste as civilizações atualizadas
        await civilizacaoRepository.UpdateAsync(vencedor);
        await civilizacaoRepository.UpdateAsync(perdedor);

        var descricao = atacanteVenceu
            ? $"{atacante.Nome} venceu o combate contra {defensor.Nome} e roubou {recursosRoubados} de comida e {territoriosRoubados} território(s)!"
            : $"{defensor.Nome} repeliu o ataque de {atacante.Nome}!";

        return new ResultadoCombate(
            Descricao: descricao,
            Vencedor: vencedor,
            Perdedor: perdedor,
            Atacante: atacante,
            Defensor: defensor,
            DiferencaPoderMilitar: diferenca,
            RecursosRoubados: recursosRoubados,
            TerritoriosRoubados: territoriosRoubados
        );
    }
}

public record ResultadoCombate(
    string Descricao,
    CORE.Domain.Entities.Civilizacao Vencedor,
    CORE.Domain.Entities.Civilizacao Perdedor,
    CORE.Domain.Entities.Civilizacao Atacante,
    CORE.Domain.Entities.Civilizacao Defensor,
    int DiferencaPoderMilitar,
    int RecursosRoubados,
    int TerritoriosRoubados
);