using System;
using System.Drawing;

namespace Classic_Snakes_Game_Tutorial___MOO_ICT
{
    /// <summary>
    /// Faixa fixa no topo do canvas, visivel tanto no menu quanto durante a
    /// partida. Mostra a pontuacao e o recorde e oferece um botao "Snap"
    /// (captura de tela) desenhado com GDI+.
    /// </summary>
    class GameHeader
    {
        /// <summary>Altura da faixa em pixels. O tabuleiro comeca abaixo dela.</summary>
        public const int Height = 44;

        private readonly Font scoreFont = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold);
        private readonly Font snapFont = new Font("Microsoft Sans Serif", 11f, FontStyle.Bold);

        private Rectangle snapBounds;

        /// <summary>Desenha a faixa ocupando toda a largura do canvas.</summary>
        public void Draw(Graphics canvas, int canvasWidth, int score, int highScore)
        {
            var area = new Rectangle(0, 0, canvasWidth, Height);

            using (var background = new SolidBrush(Color.FromArgb(30, 30, 30)))
            using (var separator = new Pen(Color.LimeGreen, 2))
            {
                canvas.FillRectangle(background, area);
                canvas.DrawLine(separator, 0, Height - 1, canvasWidth, Height - 1);
            }

            using (var textBrush = new SolidBrush(Color.Gainsboro))
            using (var vCenter = new StringFormat { LineAlignment = StringAlignment.Center })
            {
                var textArea = new RectangleF(12, 0, canvasWidth - 110, Height);
                canvas.DrawString("Pontos: " + score + "    Recorde: " + highScore,
                    scoreFont, textBrush, textArea, vCenter);
            }

            snapBounds = new Rectangle(canvasWidth - 84, 7, 72, Height - 14);
            using (var snapBrush = new SolidBrush(Color.FromArgb(60, 60, 60)))
            using (var snapBorder = new Pen(Color.LimeGreen, 1))
            using (var snapText = new SolidBrush(Color.LimeGreen))
            using (var centered = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                canvas.FillRectangle(snapBrush, snapBounds);
                canvas.DrawRectangle(snapBorder, snapBounds);
                canvas.DrawString("Snap", snapFont, snapText, snapBounds, centered);
            }
        }

        /// <summary>
        /// Indica se um ponto (em coordenadas do canvas) caiu sobre o botao "Snap".
        /// </summary>
        public bool SnapClicked(Point location)
        {
            return snapBounds.Contains(location);
        }
    }
}
