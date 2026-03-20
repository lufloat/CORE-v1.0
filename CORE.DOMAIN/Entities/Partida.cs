namespace CORE.Domain.Entities;

public class Partida
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public int TurnoAtual { get; private set; }
    public bool Encerrada { get; private set; }
    public List<Civilizacao> Civilizacoes { get; private set; } = new();

    public Partida(string nome)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        TurnoAtual = 0;
        Encerrada = false;
    }

    public void AdicionarCivilizacao(Civilizacao civilizacao)
    {
        Civilizacoes.Add(civilizacao);
    }

    public void AvancarTurno()
    {
        TurnoAtual++;
    }

    public void Encerrar()
    {
        Encerrada = true;
    }
}