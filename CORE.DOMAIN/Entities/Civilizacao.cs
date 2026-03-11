using CORE.Domain.Enums;
using CORE.DOMAIN.Entities;
using CORE.DOMAIN.Enums;

namespace CORE.Domain.Entities;

public class Civilizacao
{
    public Guid Id { get; private set; }

    public string Nome { get; private set; }

    public int Turno { get; private set; }

    public int Populacao { get; private set; }

    public int Comida { get; private set; }

    public int Madeira { get; private set; }

    public int Pedra { get; private set; }

    public int Moral { get; private set; }

    public int Tecnologia { get; private set; }

    public int PoderMilitar { get; private set; }

    public int Territorios { get; private set; }

    public EraCivilizacional Era { get; private set; }

    public Civilizacao(string nome)
    {
        Id = Guid.NewGuid();
        Nome = nome;

        Turno = 0;
        Populacao = 10;

        Comida = 20;
        Madeira = 10;
        Pedra = 10;

        Moral = 50;
        Tecnologia = 0;
        PoderMilitar = 5;

        Territorios = 1;
        Era = EraCivilizacional.Tribal;
    }

    public void AvancarTurno(IEnumerable<Regiao> regioesControladas)
    {
        Turno++;

        ProduzirRecursos(regioesControladas);
        ConsumirComida();
        AtualizarEra();
        AjustarLimites();
    }

    public void AplicarEvento(EventoCivilizacional evento)
    {
        Populacao += evento.ImpactoPopulacao;
        Comida += evento.ImpactoComida;
        Madeira += evento.ImpactoMadeira;
        Pedra += evento.ImpactoPedra;
        Moral += evento.ImpactoMoral;
        Tecnologia += evento.ImpactoTecnologia;
        PoderMilitar += evento.ImpactoPoderMilitar;

        AjustarLimites();
    }

    public void AdicionarTerritorio()
    {
        Territorios++;
    }

    private void ProduzirRecursos(IEnumerable<Regiao> regioesControladas)
    {
        foreach (var regiao in regioesControladas)
        {
            Comida += regiao.ProducaoComida;
            Madeira += regiao.ProducaoMadeira;
            Pedra += regiao.ProducaoPedra;
        }
    }

    private void ConsumirComida()
    {
        Comida -= Populacao;

        if (Comida < 0)
        {
            Moral -= 10;
            Populacao -= 2;
            Comida = 0;
        }
        else
        {
            Populacao += 2;
            Moral += 1;
        }
    }

    private void AtualizarEra()
    {
        if (Populacao >= 300 && Tecnologia >= 150 && Territorios >= 8)
        {
            Era = EraCivilizacional.Imperio;
            return;
        }

        if (Populacao >= 180 && Tecnologia >= 90 && Territorios >= 5)
        {
            Era = EraCivilizacional.Reino;
            return;
        }

        if (Populacao >= 100 && Tecnologia >= 50 && Territorios >= 3)
        {
            Era = EraCivilizacional.Cidade;
            return;
        }

        if (Populacao >= 40 && Tecnologia >= 15 && Territorios >= 2)
        {
            Era = EraCivilizacional.Aldeia;
            return;
        }

        Era = EraCivilizacional.Tribal;
    }

    private void AjustarLimites()
    {
        if (Populacao < 0) Populacao = 0;
        if (Comida < 0) Comida = 0;
        if (Madeira < 0) Madeira = 0;
        if (Pedra < 0) Pedra = 0;
        if (Moral < 0) Moral = 0;
        if (Moral > 100) Moral = 100;
        if (Tecnologia < 0) Tecnologia = 0;
        if (PoderMilitar < 0) PoderMilitar = 0;
        if (Territorios < 1) Territorios = 1;
    }
}