using CORE.Domain.Enums;

namespace CORE.DOMAIN.Entities
{
    public class EventoHistorico
    {
        public int Turno { get; private set; }
        public TipoEvento Tipo { get; private set; }
    public EventoHistorico(int turno, TipoEvento tipo)
        {
            Turno = turno;
            Tipo = tipo;
        }
    }
}

