using CORE.DOMAIN.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.DOMAIN.Entities
{
    public class EventoCivilizacional
    {
        public Guid Id { get; private set; }
        public TipoEventoCivilizacional Tipo { get; private set;}
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public int ImpactoPopulacao { get; private set; }
        public int ImpactoComida { get; private set; }
        public int ImpactoMadeira { get; private set; }
        public int ImpactoTecnologia { get; private set;}
        public int ImpactoMoral { get; private set; }
        public int ImpactoPoderMilitar { get; private set; }
        public int ImpactoPedra { get; private set; }
        public EventoCivilizacional(
       TipoEventoCivilizacional tipo,
       string titulo,
       string descricao,
       int impactoPopulacao,
       int impactoComida,
       int impactoMadeira,
       int impactoPedra,
       int impactoMoral,
       int impactoTecnologia,
       int impactoPoderMilitar)
        {
            Id = Guid.NewGuid();
            Tipo = tipo;
            Titulo = titulo;
            Descricao = descricao;
            ImpactoPopulacao = impactoPopulacao;
            ImpactoComida = impactoComida;
            ImpactoMadeira = impactoMadeira;
            ImpactoPedra = impactoPedra;
            ImpactoMoral = impactoMoral;
            ImpactoTecnologia = impactoTecnologia;
            ImpactoPoderMilitar = impactoPoderMilitar;
        }


    }
}
