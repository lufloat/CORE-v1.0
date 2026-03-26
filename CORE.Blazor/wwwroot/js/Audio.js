// Variável global fora do objeto — garante persistência entre chamadas do Blazor Server
let _musicaAtual = null;

window.audioService = {

    tocarMusica(src, volume = 0.3) {
        // Para e destrói qualquer música que esteja tocando
        if (_musicaAtual) {
            _musicaAtual.pause();
            _musicaAtual.currentTime = 0;
            _musicaAtual = null;
        }
        const audio = new Audio(src);
        audio.loop = true;
        audio.volume = Math.max(0, Math.min(1, volume));
        audio.play().catch(() => { });
        _musicaAtual = audio;
    },

    pararMusica() {
        if (_musicaAtual) {
            _musicaAtual.pause();
            _musicaAtual.currentTime = 0;
            _musicaAtual = null;
        }
    },

    pausarMusica() {
        if (_musicaAtual) {
            _musicaAtual.pause();
        }
    },

    resumirMusica() {
        if (_musicaAtual) {
            _musicaAtual.play().catch(() => { });
        }
    },

    ajustarVolume(volume) {
        if (_musicaAtual) {
            _musicaAtual.volume = Math.max(0, Math.min(1, volume));
        }
    },

    tocarEfeito(src, volume = 0.6) {
        const audio = new Audio(src);
        audio.volume = Math.max(0, Math.min(1, volume));
        audio.play().catch(() => { });
    }
};