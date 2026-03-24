namespace CORE.Api.Responses;

public record RegiaoResponse(
    Guid Id,
    string Nome,
    string Terreno,
    bool Controlada,
    int X,
    int Y,
    Guid CivilizacaoId,
    string CivilizacaoNome
);