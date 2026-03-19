using CORE.Domain.Enums;

namespace CORE.Api.Helpers;

public static class EventoHelper
{
    public static string Traduzir(TipoEvento evento)
    {
        return evento switch
        {
            TipoEvento.Seca => "Seca atingiu a civilização",
            TipoEvento.ColheitaFarta => "Colheita farta!",
            TipoEvento.Rebeliao => "Rebelião interna!",
            TipoEvento.DescobertaTecnologica => "Avanço tecnológico!",
            _ => "Nada aconteceu"
        };
    }
}