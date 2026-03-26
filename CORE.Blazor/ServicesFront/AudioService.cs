using Microsoft.JSInterop;

namespace CORE.Blazor.Services;

public class AudioService
{
    private readonly IJSRuntime _js;

    public AudioService(IJSRuntime js)
    {
        _js = js;
    }

    /// <summary>Toca música de fundo em loop.</summary>
    public ValueTask TocarMusica(string src, double volume = 0.3)
        => _js.InvokeVoidAsync("audioService.tocarMusica", src, volume);

    /// <summary>Para e reseta a música de fundo.</summary>
    public ValueTask PararMusica()
        => _js.InvokeVoidAsync("audioService.pararMusica");

    /// <summary>Pausa a música de fundo.</summary>
    public ValueTask PausarMusica()
        => _js.InvokeVoidAsync("audioService.pausarMusica");

    /// <summary>Retoma a música de fundo pausada.</summary>
    public ValueTask ResumarMusica()
        => _js.InvokeVoidAsync("audioService.resumirMusica");

    /// <summary>Ajusta o volume da música (0.0 a 1.0).</summary>
    public ValueTask AjustarVolume(double volume)
        => _js.InvokeVoidAsync("audioService.ajustarVolume", volume);

    /// <summary>Toca um efeito sonoro pontual (não em loop).</summary>
    public ValueTask TocarEfeito(string src, double volume = 0.6)
        => _js.InvokeVoidAsync("audioService.tocarEfeito", src, volume);
}