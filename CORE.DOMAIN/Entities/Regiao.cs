using CORE.Domain.Enums;
using CORE.DOMAIN.Enums;

namespace CORE.Domain.Entities
{
    public class Regiao
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public TipoTerreno Terreno { get; private set; }
        public bool Controlada { get; private set; }
        public int ProducaoComida { get; private set; }
        public int ProducaoMadeira { get; private set; }
        public int ProducaoPedra { get; private set; }
        public int NivelDesenvolvimento { get; private set; }
        public Guid CivilizacaoId { get; private set; }

        public Regiao(string nome, TipoTerreno terreno, Guid civilizacaoId)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Terreno = terreno;
            Controlada = false;
            NivelDesenvolvimento = 0;
            CivilizacaoId = civilizacaoId; // ✅ agora o parâmetro existe
            DefinirProducaoBase();
        }

        private void DefinirProducaoBase()
        {
            switch (Terreno)
            {
                case TipoTerreno.Planicie:
                    ProducaoComida = 3; ProducaoMadeira = 1; ProducaoPedra = 1; break;
                case TipoTerreno.Floresta:
                    ProducaoComida = 1; ProducaoMadeira = 1; ProducaoPedra = 3; break;
                case TipoTerreno.Montanha:
                    ProducaoComida = 1; ProducaoMadeira = 3; ProducaoPedra = 1; break;
                case TipoTerreno.Rio:
                    ProducaoComida = 3; ProducaoMadeira = 1; ProducaoPedra = 1; break;
                case TipoTerreno.Deserto:
                    ProducaoComida = 0; ProducaoMadeira = 0; ProducaoPedra = 2; break;
            }
        }

        public void Conquistar() => Controlada = true;
        public void MarcarComoControlada() => Controlada = true;
        public void Controlar() => Controlada = true;
    }
}