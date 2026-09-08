using System;
using System.IO;
using System.Windows.Media;

namespace Classic_Snakes_Game_Tutorial___MOO_ICT
{
    /// <summary>
    /// Musica de fundo do jogo. Toca o arquivo track.mp3 (que fica ao lado
    /// do executavel) em loop continuo, do inicio ao fim da aplicacao.
    ///
    /// O volume tem dois patamares: cheio durante a partida
    /// (<see cref="GameState.Playing"/>) e pela metade em qualquer outra
    /// tela — menu principal, escolha de dificuldade, pausa e game over.
    /// A troca e feita por <see cref="Form1.UpdateMusicVolume"/> a cada
    /// mudanca de estado.
    ///
    /// Usa o <see cref="MediaPlayer"/> do WPF, que ja acompanha o .NET e
    /// decodifica MP3 sem pacote extra. Se o arquivo nao existir ou o audio
    /// falhar por qualquer motivo, o jogo continua normalmente, sem som.
    /// </summary>
    static class BackgroundMusic
    {
        /// <summary>Volume (0.0 a 1.0) durante a partida.</summary>
        private const double GameplayVolume = 1.0;

        /// <summary>Volume (0.0 a 1.0) nas telas de menu, pausa e game over.</summary>
        private const double MenuVolume = 0.5;

        private static MediaPlayer? player;

        /// <summary>
        /// Carrega track.mp3 e comeca a tocar em loop. Chamado uma unica vez,
        /// na inicializacao do formulario. Chamadas seguintes sao ignoradas.
        /// </summary>
        public static void Start()
        {
            if (player != null)
            {
                return;
            }

            string path = Path.Combine(AppContext.BaseDirectory, "track.mp3");
            if (!File.Exists(path))
            {
                return;
            }

            try
            {
                player = new MediaPlayer { Volume = GameplayVolume };

                // O MediaPlayer nao tem loop nativo: quando a faixa termina,
                // voltamos para o inicio e tocamos de novo.
                player.MediaEnded += (s, e) =>
                {
                    player.Position = TimeSpan.Zero;
                    player.Play();
                };

                player.Open(new Uri(path));
                player.Play();
            }
            catch
            {
                // Sem suporte de midia disponivel: o jogo segue sem musica.
                player = null;
            }
        }

        /// <summary>Volume cheio: usado enquanto a partida esta em andamento.</summary>
        public static void SetGameplayVolume() => SetVolume(GameplayVolume);

        /// <summary>Metade do volume: usado nas telas de menu, pausa e game over.</summary>
        public static void SetMenuVolume() => SetVolume(MenuVolume);

        private static void SetVolume(double volume)
        {
            if (player != null)
            {
                player.Volume = volume;
            }
        }
    }
}
