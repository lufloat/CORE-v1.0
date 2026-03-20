namespace CORE.Api.Responses;

public record SimulacaoTurnoResponse(
    int Turno,
    int Populacao,
    int Comida,
    int Madeira,
    int Pedra,
    int Moral,
    int Tecnologia,
    int PoderMilitar,
    int Territorios,
    string Era,
    string Decisao,
    string Evento
);

public record SimulacaoResponse(
    int TurnosSimulados,
    List<SimulacaoTurnoResponse> HistoricoTurnos,
    CivilizacaoResponse EstadoFinal
);