namespace CORE.Blazor.Models;

public class CivilizacaoModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Turno { get; set; }
    public int Populacao { get; set; }
    public int Comida { get; set; }
    public int Madeira { get; set; }
    public int Pedra { get; set; }
    public int Moral { get; set; }
    public int Tecnologia { get; set; }
    public int PoderMilitar { get; set; }
    public int Territorios { get; set; }
    public string Era { get; set; } = string.Empty;
    public string? UltimoEvento { get; set; }
}

public class PartidaModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TurnoAtual { get; set; }
    public bool Encerrada { get; set; }
    public List<CivilizacaoModel> Civilizacoes { get; set; } = new();
}

public class CombateModel
{
    public string Descricao { get; set; } = string.Empty;
    public string Vencedor { get; set; } = string.Empty;
    public string Perdedor { get; set; } = string.Empty;
    public int DiferencaPoderMilitar { get; set; }
    public int RecursosRoubados { get; set; }
    public int TerritoriosRoubados { get; set; }
    public CivilizacaoModel EstadoAtacante { get; set; } = new();
    public CivilizacaoModel EstadoDefensor { get; set; } = new();
}