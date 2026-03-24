using CORE.Blazor.Models;
using System.Net.Http.Json;

namespace CORE.Blazor.Services;

public class ApiService
{
    private readonly HttpClient httpClient;

    public ApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<PartidaModel?> GetPartidaAsync(Guid partidaId)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<PartidaModel>($"api/partida/{partidaId}");
        }
        catch { return null; }
    }

    public async Task<PartidaModel?> CriarPartidaAsync(List<string> nomes)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/partida/criar", nomes);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<PartidaModel>();
        }
        catch { return null; }
    }

    public async Task<PartidaModel?> AvancarTurnoAsync(Guid partidaId)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(
                $"api/partida/{partidaId}/avancar-turno", new { });
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<PartidaModel>();
        }
        catch { return null; }
    }

    public async Task<CombateModel?> CombaterAsync(Guid partidaId, Guid atacanteId, Guid defensorId)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(
                $"api/partida/{partidaId}/combate?atacanteId={atacanteId}&defensorId={defensorId}", new { });
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<CombateModel>();
        }
        catch { return null; }
    }
}