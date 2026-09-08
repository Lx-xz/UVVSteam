using System;
using System.Collections.Generic;
using System.Text;

namespace Classic_Snakes_Game_Tutorial___MOO_ICT
{
    enum Difficulty
    {
        /// <summary>Arena maior, paredes atravessaveis (wrap), meta de pontos baixa.</summary>
        Facil,

        /// <summary>Arena padrao, paredes solidas (batê-las mata), meta de pontos mais alta.</summary>
        Medio,

        /// <summary>Arena padrao, paredes solidas, sem meta — modo infinito de recorde.</summary>
        Pro
    }
}
