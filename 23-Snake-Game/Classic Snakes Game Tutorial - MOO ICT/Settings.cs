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

        /// <summary>Direcao atual da cobra: "left", "right", "up" ou "down".</summary>
        public static string directions = "left";

        public Settings()
        {
            // Tabuleiro pequeno e cobra lenta.
            CellSize = 20;
            Columns = 20;
            Rows = 20;
            SnakeSpeedMs = 120;
            directions = "left";
        }
    }
}
