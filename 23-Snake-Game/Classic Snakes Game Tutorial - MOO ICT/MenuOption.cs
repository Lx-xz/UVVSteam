using System;
using System.Drawing;

namespace Classic_Snakes_Game_Tutorial___MOO_ICT
{
    /// <summary>
    /// Uma opcao do <see cref="MainMenu"/>: um rotulo visivel e a acao
    /// executada quando o jogador a seleciona (por teclado ou mouse).
    /// </summary>
    class MenuOption
    {
        /// <summary>Texto exibido no menu.</summary>
        public string Label { get; }

        /// <summary>Acao executada ao ativar a opcao.</summary>
        public Action OnSelected { get; }

        /// <summary>
        /// Texto explicativo opcional, mostrado no rodape do menu quando
        /// esta opcao esta destacada (ex.: descrever uma dificuldade).
        /// Null para usar o texto padrao de instrucoes.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Area ocupada pela opcao no canvas. E recalculada a cada desenho
        /// e usada para detectar cliques e a passagem do mouse.
        /// </summary>
        public Rectangle Bounds { get; set; }

        public MenuOption(string label, Action onSelected, string? description = null)
        {
            Label = label;
            OnSelected = onSelected;
            Description = description;
        }
    }
}
