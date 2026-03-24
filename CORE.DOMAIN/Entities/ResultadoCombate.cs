namespace CORE.Domain.Entities;

public class ResultadoCombate
{
    public Civilizacao Atacante { get; private set; }
    public Civilizacao Defensor { get; private set; }
    public Civilizacao Vencedor { get; private set; }
    public Civilizacao Perdedor { get; private set; }
    public int DiferencaPoderMilitar { get; private set; }
    public int RecursosRoubados { get; private set; }
    public int TerritoriosRoubados { get; private set; }
    public string Descricao { get; private set; }

    public ResultadoCombate(
        Civilizacao atacante,
        Civilizacao defensor,
        Civilizacao vencedor,
        Civilizacao perdedor,
        int diferencaPoderMilitar,
        int recursosRoubados,
        int territoriosRoubados,
        string descricao)
    {
        Atacante = atacante;
        Defensor = defensor;
        Vencedor = vencedor;
        Perdedor = perdedor;
        DiferencaPoderMilitar = diferencaPoderMilitar;
        RecursosRoubados = recursosRoubados;
        TerritoriosRoubados = territoriosRoubados;
        Descricao = descricao;
    }
}