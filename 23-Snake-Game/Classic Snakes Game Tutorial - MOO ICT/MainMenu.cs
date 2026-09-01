using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace Classic_Snakes_Game_Tutorial___MOO_ICT
{
    /// <summary>
    /// Menu principal do jogo, desenhado diretamente no canvas com GDI+.
    /// Suporta navegacao por teclado (setas + Enter) e por mouse (passar o
    /// cursor destaca a opcao, clicar a ativa). Novas opcoes sao adicionadas
    /// com <see cref="AddOption"/> sem alterar a logica de desenho ou navegacao.
    /// </summary>
    class MainMenu
    {
        private readonly List<MenuOption> options = new List<MenuOption>();

        private readonly Font titleFont = new Font("Microsoft Sans Serif", 40f, FontStyle.Bold);
        private readonly Font optionFont = new Font("Microsoft Sans Serif", 20f, FontStyle.Bold);
        private readonly Font hintFont = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular);

        private const int OptionWidth = 280;
        private const int OptionHeight = 54;
        private const int OptionSpacing = 18;

        /// <summary>Indice da opcao atualmente destacada.</summary>
        public int SelectedIndex { get; private set; }

        /// <summary>Adiciona uma opcao ao menu, na ordem em que sera exibida.</summary>
        public void AddOption(string label, Action onSelected)
        {
            options.Add(new MenuOption(label, onSelected));
        }

        /// <summary>Devolve o destaque para a primeira opcao.</summary>
        public void Reset()
        {
            SelectedIndex = 0;
        }

        /// <summary>Move o destaque para a opcao anterior (volta ao fim ao passar do inicio).</summary>
        public void MoveUp()
        {
            if (options.Count == 0) return;
            SelectedIndex = (SelectedIndex - 1 + options.Count) % options.Count;
        }

        /// <summary>Move o destaque para a proxima opcao (volta ao inicio ao passar do fim).</summary>
        public void MoveDown()
        {
            if (options.Count == 0) return;
            SelectedIndex = (SelectedIndex + 1) % options.Count;
        }

        /// <summary>Executa a acao da opcao destacada.</summary>
        public void ActivateSelected()
        {
            if (options.Count == 0) return;
            options[SelectedIndex].OnSelected();
        }

        /// <summary>
        /// Atualiza o destaque conforme a posicao do mouse sobre o canvas.
        /// Retorna true se o destaque mudou (o canvas precisa ser redesenhado).
        /// </summary>
        public bool HandleMouseMove(Point location)
        {
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].Bounds.Contains(location) && SelectedIndex != i)
                {
                    SelectedIndex = i;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Ativa a opcao sob o cursor, se houver. Retorna true se alguma
        /// opcao foi clicada.
        /// </summary>
        public bool HandleMouseClick(Point location)
        {
            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].Bounds.Contains(location))
                {
                    SelectedIndex = i;
                    options[i].OnSelected();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Desenha o menu dentro da area util informada (o restante do canvas
        /// abaixo do cabecalho fixo).
        /// </summary>
        public void Draw(Graphics canvas, Rectangle area)
        {
            canvas.Clear(Color.FromArgb(20, 20, 20));
            canvas.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            using (var centered = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                using (var titleBrush = new SolidBrush(Color.LimeGreen))
                {
                    var titleArea = new RectangleF(area.Left, area.Top + 24, area.Width, 120);
                    canvas.DrawString("SNAKE", titleFont, titleBrush, titleArea, centered);
                }

                int blockHeight = options.Count * OptionHeight
                                  + Math.Max(0, options.Count - 1) * OptionSpacing;
                int startY = area.Top + (area.Height - blockHeight) / 2 + 24;
                int x = area.Left + (area.Width - OptionWidth) / 2;

                for (int i = 0; i < options.Count; i++)
                {
                    int y = startY + i * (OptionHeight + OptionSpacing);
                    var bounds = new Rectangle(x, y, OptionWidth, OptionHeight);
                    options[i].Bounds = bounds;

                    bool selected = i == SelectedIndex;
                    Color boxColor = selected ? Color.LimeGreen : Color.FromArgb(45, 45, 45);
                    Color textColor = selected ? Color.Black : Color.Gainsboro;

                    using (var boxBrush = new SolidBrush(boxColor))
                    using (var textBrush = new SolidBrush(textColor))
                    using (var border = new Pen(Color.LimeGreen, 2))
                    {
                        canvas.FillRectangle(boxBrush, bounds);
                        canvas.DrawRectangle(border, bounds);
                        canvas.DrawString(options[i].Label, optionFont, textBrush, bounds, centered);
                    }
                }
            }

            using (var hintBrush = new SolidBrush(Color.Gray))
            using (var hintFormat = new StringFormat { Alignment = StringAlignment.Center })
            {
                var hintArea = new RectangleF(area.Left, area.Bottom - 34, area.Width, 24);
                canvas.DrawString("Setas + Enter ou clique do mouse", hintFont, hintBrush, hintArea, hintFormat);
            }
        }
    }
}
