using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classic_Snakes_Game_Tutorial___MOO_ICT
{
    /// <summary>
    /// Parametros globais da partida. Os valores de tabuleiro e velocidade
    /// passarao a depender da dificuldade escolhida no menu.
    /// </summary>
    class Settings
    {
        /// <summary>Lado de cada celula do tabuleiro, em pixels.</summary>
        public static int CellSize { get; set; }

        /// <summary>Numero de colunas do tabuleiro.</summary>
        public static int Columns { get; set; }

        /// <summary>Numero de linhas do tabuleiro.</summary>
        public static int Rows { get; set; }

        /// <summary>Intervalo do timer do jogo em milissegundos (menor = cobra mais rapida).</summary>
        public static int SnakeSpeedMs { get; set; }

        /// <summary>
        /// Se verdadeiro, a cobra acelera a cada maca comida: o intervalo do
        /// timer diminui em <see cref="SpeedUpStepMs"/> ate o piso de
        /// <see cref="MinSpeedMs"/>. Ativado apenas no modo Pro.
        /// </summary>
        public static bool SnakeSpeedsUp { get; set; }

        /// <summary>Reducao no intervalo do timer (ms) por maca comida, quando <see cref="SnakeSpeedsUp"/> e verdadeiro.</summary>
        public const int SpeedUpStepMs = 4;

        /// <summary>Intervalo minimo do timer (ms): a cobra nao acelera alem disso.</summary>
        public const int MinSpeedMs = 60;

        /// <summary>
        /// Se verdadeiro, a partida mantem e exibe o recorde (maior pontuacao
        /// ja alcancada). Ativado apenas no modo Pro, que e o modo de quebrar
        /// recorde; Facil e Medio tem meta fixa de pontos, entao um recorde
        /// nao faz sentido neles.
        /// </summary>
        public static bool TracksHighScore { get; set; }

        /// <summary>
        /// Se verdadeiro, colidir com a borda do tabuleiro encerra a partida
        /// (Game Over). Se falso, a cobra atravessa a borda e reaparece do
        /// lado oposto (comportamento classico de "wrap").
        /// </summary>
        public static bool WallsAreSolid { get; set; }

        /// <summary>
        /// Pontuacao necessaria para vencer a partida. Null significa "sem
        /// meta": o modo continua indefinidamente ate a cobra morrer (usado
        /// no nivel Pro, que funciona como um modo de quebrar o recorde).
        /// </summary>
        public static int? TargetScore { get; set; }

        /// <summary>Ultima dificuldade aplicada (usada para "Jogar novamente" na tela de Game Over).</summary>
        public static Difficulty CurrentDifficulty { get; set; }

        /// <summary>Direcao atual da cobra: "left", "right", "up" ou "down".</summary>
        public static string directions = "left";

        public Settings()
        {
            // Valores validos antes mesmo do jogador escolher uma
            // dificuldade (ex.: enquanto o menu principal esta desenhando
            // o cabecalho). Medio funciona bem como padrao neutro.
            ApplyDifficulty(Difficulty.Medio);
            directions = "left";
        }

        /// <summary>
        /// Define o tamanho da arena, o comportamento das paredes e a meta
        /// de pontos de acordo com a dificuldade escolhida no menu.
        /// </summary>
        public static void ApplyDifficulty(Difficulty difficulty)
        {
            CurrentDifficulty = difficulty;

            switch (difficulty)
            {
                case Difficulty.Facil:
                    // Arena maior (mais celulas = mais espaco pra manobrar) e
                    // paredes atravessaveis: e o nivel mais tolerante a erro.
                    // Celula menor para o tabuleiro maior ainda caber no canvas.
                    Columns = 26;
                    Rows = 26;
                    CellSize = 16;
                    WallsAreSolid = false;
                    TargetScore = 10; // meta baixa, so pra dar uma vitoria alcancavel
                    SnakeSpeedsUp = false;
                    TracksHighScore = false;
                    break;

                case Difficulty.Medio:
                    // Arena no tamanho "classico" do jogo e paredes solidas:
                    // bater na borda mata, igual bater no proprio corpo.
                    Columns = 20;
                    Rows = 20;
                    CellSize = 20;
                    WallsAreSolid = true;
                    TargetScore = 20; // meta maior que a do Facil
                    SnakeSpeedsUp = false;
                    TracksHighScore = false;
                    break;

                case Difficulty.Pro:
                    // Mesmo tamanho de arena do Medio, mas sem meta: o jogo
                    // so acaba quando a cobra morre. Funciona como um modo
                    // "break ur highscore" — e o unico modo que guarda recorde
                    // e em que a cobra acelera conforme come.
                    Columns = 20;
                    Rows = 20;
                    CellSize = 20;
                    WallsAreSolid = true;
                    TargetScore = null;
                    SnakeSpeedsUp = true;
                    TracksHighScore = true;
                    break;
            }

            SnakeSpeedMs = 120;
        }
    }
}
