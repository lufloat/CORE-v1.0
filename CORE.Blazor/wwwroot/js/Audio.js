window.audioService = {
    musica: null,

    tocarMusica(src, volume = 0.3) {
        if (this.musica) {
            this.musica.pause();
            this.musica.currentTime = 0;
        }
        this.musica = new Audio(src);
        this.musica.loop = true;
        this.musica.volume = volume;
        this.musica.play().catch(() => {
            // Autoplay bloqueado pelo browser — aguarda interação do usuário
        });
    },

    pararMusica() {
        if (this.musica) {
            this.musica.pause();
            this.musica.currentTime = 0;
        }
    },

    pausarMusica() {
        if (this.musica) {
            this.musica.pause();
        }
    },

    resumirMusica() {
        if (this.musica) {
            this.musica.play().catch(() => { });
        }
    },

    ajustarVolume(volume) {
        if (this.musica) {
            this.musica.volume = Math.max(0, Math.min(1, volume));
        }
    },

    tocarEfeito(src, volume = 0.6) {
        const audio = new Audio(src);
        audio.volume = Math.max(0, Math.min(1, volume));
        audio.play().catch(() => { });
    }
};