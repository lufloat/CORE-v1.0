using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Domain.Enums;
using System.Text;
using System.Text.Json;

namespace CORE.Infrastructure.Services;

public class IAService : IIAService
{
    private readonly HttpClient _httpClient;

    public IAService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TipoDecisao> ObterDecisaoAsync(Civilizacao civilizacao)
    {
        var payload = new
        {
            nome = civilizacao.Nome,
            turno = civilizacao.Turno,
            populacao = civilizacao.Populacao,
            comida = civilizacao.Comida,
            madeira = civilizacao.Madeira,
            pedra = civilizacao.Pedra,
            moral = civilizacao.Moral,
            tecnologia = civilizacao.Tecnologia,
            poderMilitar = civilizacao.PoderMilitar,
            territorios = civilizacao.Territorios,
            era = civilizacao.Era.ToString(),
            ultimoEvento = civilizacao.UltimoEvento.ToString()
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("decidir", content);
        response.EnsureSuccessStatusCode();

        var resultado = await response.Content.ReadAsStringAsync();

        if (Enum.TryParse<TipoDecisao>(resultado.Trim('"'), ignoreCase: true, out var decisao))
            return decisao;

        throw new Exception($"IA retornou valor inválido: {resultado}");
    }
}