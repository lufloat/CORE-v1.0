using CORE.Domain.Entities;
using CORE.Domain.Enums;
using CORE.DOMAIN.Enums;
using Xunit;

namespace CORE.Tests;

public class CivilizacaoTests
{
    [Fact]
    public void AvancarTurno_DeveIncrementarTurno()
    {
        var civ = new Civilizacao("Roma");
        var regioes = new List<Regiao>();

        civ.AvancarTurno(regioes);

        Assert.Equal(1, civ.Turno);
    }

    [Fact]
    public void AvancarTurno_ComRegiao_DeveProducirRecursos()
    {
        var civ = new Civilizacao("Roma");
        var regiao = new Regiao("Vale", TipoTerreno.Planicie, civ.Id);
        regiao.MarcarComoControlada();

        var comidaAntes = civ.Comida;
        civ.AvancarTurno(new List<Regiao> { regiao });

        Assert.True(civ.Comida != comidaAntes);
    }

    [Fact]
    public void AplicarDecisao_ProduzirComida_DeveAumentarComida()
    {
        var civ = new Civilizacao("Roma");
        var comidaAntes = civ.Comida;

        civ.AplicarDecisao(TipoDecisao.ProduzirComida);

        Assert.True(civ.Comida > comidaAntes);
    }

    [Fact]
    public void AplicarDecisao_MelhorarMoral_DeveAumentarMoral()
    {
        var civ = new Civilizacao("Roma");
        var moralAntes = civ.Moral;

        civ.AplicarDecisao(TipoDecisao.MelhorarMoral);

        Assert.True(civ.Moral > moralAntes);
    }

    [Fact]
    public void AplicarDecisao_TreinarExercito_DeveAumentarPoderMilitar()
    {
        var civ = new Civilizacao("Roma");
        var poderAntes = civ.PoderMilitar;

        civ.AplicarDecisao(TipoDecisao.TreinarExercito);

        Assert.True(civ.PoderMilitar > poderAntes);
    }

    [Fact]
    public void AplicarVitoria_DeveAumentarRecursosEMoral()
    {
        var civ = new Civilizacao("Roma");
        var comidaAntes = civ.Comida;
        var moralAntes = civ.Moral;

        civ.AplicarVitoria(10, 1);

        Assert.True(civ.Comida > comidaAntes);
        Assert.True(civ.Moral > moralAntes);
    }

    [Fact]
    public void AplicarDerrota_DeveReduzirRecursosEMoral()
    {
        var civ = new Civilizacao("Roma");
        var moralAntes = civ.Moral;
        var poderAntes = civ.PoderMilitar;

        civ.AplicarDerrota(5, 0);

        Assert.True(civ.Moral < moralAntes);
        Assert.True(civ.PoderMilitar < poderAntes);
    }

    [Fact]
    public void Civilizacao_NaoDeveIniciarComRecursosNegativos()
    {
        var civ = new Civilizacao("Roma");

        Assert.True(civ.Comida >= 0);
        Assert.True(civ.Moral >= 0);
        Assert.True(civ.PoderMilitar >= 0);
        Assert.True(civ.Populacao >= 0);
    }
}