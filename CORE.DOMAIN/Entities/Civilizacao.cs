using CORE.DOMAIN.Enums;

namespace CORE.DOMAIN.Entities
{
    public class Civilizacao
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public int Turno { get; private set; }

        public int Populacao { get; private set; }

        public int Comida { get; private set; }

        public int Madeira { get; private set; }

        public int Pedra { get; private set; }

        public int Moral { get; private set; }

        public int PoderMilitar { get; private set; }

        public int Territorio { get; private set; }

        public EraCivilizacional Era { get; private set; }

        public Civilizacao(string nome)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Turno = 0;
            Populacao = 10;
            Comida = 20;
            Madeira = 10;
            Pedra = 10;
            Moral = 50; // Moral começa em 100
            PoderMilitar = 5;
            Territorio = 1;
            Era = EraCivilizacional.Tribal;
        }
    }
}
