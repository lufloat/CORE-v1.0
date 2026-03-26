using CORE.Domain.Entities;
using CORE.Domain.Enums;
using CORE.DOMAIN.Enums;

namespace CORE.Domain.Services;

public static class MapaHelper
{
    private static readonly Random _random = new();

    private static readonly TipoTerreno[] Terrenos = new[]
    {
        TipoTerreno.Planicie,
        TipoTerreno.Floresta,
        TipoTerreno.Montanha,
        TipoTerreno.Rio,
        TipoTerreno.Deserto
    };

    public static (int x, int y)? EncontrarCelulaAdjacente(
        IEnumerable<Regiao> regioesOcupadas,
        Guid civilizacaoId,
        int tamanhoX = 10,
        int tamanhoY = 10)
    {
        var ocupadas = regioesOcupadas
            .Select(r => (r.X, r.Y))
            .ToHashSet();

        var regioesDaCiv = regioesOcupadas
            .Where(r => r.CivilizacaoId == civilizacaoId)
            .ToList();

        // tenta adjacentes às regiões da civilização
        var candidatas = new List<(int x, int y)>();

        foreach (var regiao in regioesDaCiv)
        {
            var adjacentes = new[]
            {
                (regiao.X + 1, regiao.Y),
                (regiao.X - 1, regiao.Y),
                (regiao.X, regiao.Y + 1),
                (regiao.X, regiao.Y - 1),
            };

            foreach (var adj in adjacentes)
            {
                if (adj.Item1 >= 0 && adj.Item1 < tamanhoX &&
                    adj.Item2 >= 0 && adj.Item2 < tamanhoY &&
                    !ocupadas.Contains(adj))
                {
                    candidatas.Add(adj);
                }
            }
        }

        if (candidatas.Count == 0) return null;

        return candidatas[_random.Next(candidatas.Count)];
    }

    public static TipoTerreno TerremoAleatorio()
        => Terrenos[_random.Next(Terrenos.Length)];
}