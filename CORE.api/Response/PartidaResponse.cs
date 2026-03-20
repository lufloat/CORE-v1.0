namespace CORE.Api.Responses;

public record PartidaResponse(
    Guid Id,
    string Nome,
    int TurnoAtual,
    bool Encerrada,
    List<CivilizacaoResponse> Civilizacoes
);