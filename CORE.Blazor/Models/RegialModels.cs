namespace CORE.Blazor.Models;

public class RegiaoModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Terreno { get; set; } = string.Empty;
    public bool Controlada { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public Guid CivilizacaoId { get; set; }
    public string CivilizacaoNome { get; set; } = string.Empty;
}