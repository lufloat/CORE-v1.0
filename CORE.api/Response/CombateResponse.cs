namespace CORE.Api.Responses;

public record CombateResponse(
    string Descricao,
    string Vencedor,
    string Perdedor,
    int DiferencaPoderMilitar,
    int RecursosRoubados,
    int TerritoriosRoubados,
    CivilizacaoResponse EstadoAtacante,
    CivilizacaoResponse EstadoDefensor
);