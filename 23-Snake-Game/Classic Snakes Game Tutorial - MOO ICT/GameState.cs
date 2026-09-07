namespace Classic_Snakes_Game_Tutorial___MOO_ICT
{
    /// <summary>
    /// Telas possiveis do jogo. Define o que e desenhado no canvas e como
    /// as teclas e o mouse sao interpretados a cada momento.
    /// </summary>
    enum GameState
    {
        /// <summary>Menu principal visivel; nenhuma partida em andamento.</summary>
        Menu,

        /// <summary>Tela de escolha de dificuldade, antes de iniciar a partida.</summary>
        DifficultySelect,

        /// <summary>Partida em andamento.</summary>
        Playing,

        /// <summary>Partida em andamento, porem pausada.</summary>
        Paused,

        /// <summary>Partida encerrada (derrota ou vitoria); tela de resultado visivel.</summary>
        GameOver
    }
}
