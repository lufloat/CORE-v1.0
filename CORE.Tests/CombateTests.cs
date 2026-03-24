using CORE.Application.Interfaces;
using CORE.Application.UseCases;
using CORE.Domain.Entities;
using CORE.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CORE.Tests;

public class CombateTests
{
    private readonly Mock<IPartidaRepository> partidaRepo = new();
    private readonly Mock<ICivilizacaoRepository> civilizacaoRepo = new();
    private readonly Mock<ILogger<CombaterCivilizacoes>> logger = new();

    private CombaterCivilizacoes CriarUseCase() =>
        new(partidaRepo.Object, civilizacaoRepo.Object, logger.Object);

    [Fact]
    public async Task Combate_AtacanteVence_QuandoTemMaiorPoderMilitar()
    {
        var atacante = new Civilizacao("Roma");
        var defensor = new Civilizacao("Grécia");

        atacante.AplicarDecisao(TipoDecisao.TreinarExercito);
        atacante.AplicarDecisao(TipoDecisao.TreinarExercito);

        var partida = new Partida("Partida Teste");
        partida.AdicionarCivilizacao(atacante);
        partida.AdicionarCivilizacao(defensor);

        partidaRepo
            .Setup(r => r.GetByIdComCivilizacoesAsync(partida.Id))
            .ReturnsAsync(partida);

        civilizacaoRepo
            .Setup(r => r.UpdateAsync(It.IsAny<Civilizacao>()))
            .Returns(Task.CompletedTask);

        var useCase = CriarUseCase();
        var resultado = await useCase.ExecutarAsync(partida.Id, atacante.Id, defensor.Id);

        Assert.Equal(atacante.Nome, resultado.Vencedor.Nome);
        Assert.Equal(defensor.Nome, resultado.Perdedor.Nome);
    }

    [Fact]
    public async Task Combate_DeveLancarExcecao_QuandoAtacanteIgualDefensor()
    {
        var useCase = CriarUseCase();
        var id = Guid.NewGuid();

        await Assert.ThrowsAsync<Exception>(() =>
            useCase.ExecutarAsync(Guid.NewGuid(), id, id));
    }

    [Fact]
    public async Task Combate_DeveLancarExcecao_QuandoPartidaNaoEncontrada()
    {
        partidaRepo
            .Setup(r => r.GetByIdComCivilizacoesAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Partida?)null);

        var useCase = CriarUseCase();

        await Assert.ThrowsAsync<Exception>(() =>
            useCase.ExecutarAsync(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()));
    }

    [Fact]
    public async Task Combate_DeveLancarExcecao_QuandoPartidaEncerrada()
    {
        var partida = new Partida("Partida Teste");
        partida.Encerrar();

        partidaRepo
            .Setup(r => r.GetByIdComCivilizacoesAsync(partida.Id))
            .ReturnsAsync(partida);

        var useCase = CriarUseCase();

        await Assert.ThrowsAsync<Exception>(() =>
            useCase.ExecutarAsync(partida.Id, Guid.NewGuid(), Guid.NewGuid()));
    }
}
