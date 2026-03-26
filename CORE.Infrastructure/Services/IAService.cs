using CORE.Application.Interfaces;
using CORE.Domain.Entities;
using CORE.Domain.Enums;
using System.Net.Http.Json;


namespace CORE.Infrastructure.Services;

public class IAService : IIAService  // ← adiciona isso

{
    private readonly HttpClient httpClient;

    public IAService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<TipoDecisao> ObterDecisaoAsync(Civilizacao civilizacao)
    {
        var estado = new
        {
            comida = civilizacao.Comida,
            moral = civilizacao.Moral,
            tecnologia = civilizacao.Tecnologia
        };

        var response = await httpClient.PostAsJsonAsync("/decidir", estado);

        var result = await response.Content.ReadFromJsonAsync<RespostaIA>();

        return Enum.Parse<TipoDecisao>(result.decisao);
    }

    private class RespostaIA
    {
        public string decisao { get; set; }
    }
}