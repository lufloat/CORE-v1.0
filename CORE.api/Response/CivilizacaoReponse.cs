namespace CORE.Api.Responses;

public record CivilizacaoResponse(
    Guid Id,
    string Nome,
    int Turno,
    int Populacao,
    int Comida,
    int Madeira,
    int Pedra,
    int Moral,
    int Tecnologia,
    int PoderMilitar,
    int Territorios,
    string Era);