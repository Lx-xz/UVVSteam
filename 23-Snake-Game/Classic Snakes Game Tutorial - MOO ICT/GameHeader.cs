using System;
using System.Drawing;

namespace Classic_Snakes_Game_Tutorial___MOO_ICT
{
    /// <summary>
    /// Faixa fixa no topo do canvas, visivel tanto no menu quanto durante a
    /// partida. Mostra a pontuacao e o recorde e oferece os botoes "Pausar"
    /// e "Snap" (captura de tela) desenhados com GDI+.
    /// </summary>
    class GameHeader
    {
        /// <summary>Altura da faixa em pixels. O tabuleiro comeca abaixo dela.</summary>
        public const int Height = 44;

        private readonly Font scoreFont = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold);
        private readonly Font snapFont = new Font("Microsoft Sans Serif", 11f, FontStyle.Bold);

        private Rectangle snapBounds;
        private Rectangle pauseBounds;
        private bool pauseButtonVisible;

        /// <summary>Desenha a faixa ocupando toda a largura do canvas.</summary>
        /// <param name="showPauseButton">Se o botao "Pausar" deve ser exibido (apenas durante uma partida).</param>
        /// <param name="isPaused">Se a partida esta pausada no momento (altera o rotulo do botao).</param>
        public void Draw(Graphics canvas, int canvasWidth, int score, int highScore, bool showPauseButton, bool isPaused)
        {
            var area = new Rectangle(0, 0, canvasWidth, Height);

            using (var background = new SolidBrush(Color.FromArgb(30, 30, 30)))
            using (var separator = new Pen(Color.LimeGreen, 2))
            {
                canvas.FillRectangle(background, area);
                canvas.DrawLine(separator, 0, Height - 1, canvasWidth, Height - 1);
            }

            pauseButtonVisible = showPauseButton;

            snapBounds = new Rectangle(canvasWidth - 84, 7, 72, Height - 14);
            pauseBounds = pauseButtonVisible
                ? new Rectangle(snapBounds.Left - 8 - 92, 7, 92, Height - 14)
                : Rectangle.Empty;

            int textRight = pauseButtonVisible ? pauseBounds.Left : snapBounds.Left;

            using (var textBrush = new SolidBrush(Color.Gainsboro))
            using (var vCenter = new StringFormat { LineAlignment = StringAlignment.Center })
            {
                var textArea = new RectangleF(12, 0, Math.Max(0, textRight - 22), Height);
                canvas.DrawString("Pontos: " + score + "    Recorde: " + highScore,
                    scoreFont, textBrush, textArea, vCenter);
            }

            if (pauseButtonVisible)
            {
                using (var pauseBrush = new SolidBrush(Color.FromArgb(60, 60, 60)))
                using (var pauseBorder = new Pen(Color.LimeGreen, 1))
                using (var pauseText = new SolidBrush(Color.LimeGreen))
                using (var centered = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    canvas.FillRectangle(pauseBrush, pauseBounds);
                    canvas.DrawRectangle(pauseBorder, pauseBounds);
                    canvas.DrawString(isPaused ? "Retomar" : "Pausar", snapFont, pauseText, pauseBounds, centered);
                }
            }

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

        /// <summary>
        /// Indica se um ponto (em coordenadas do canvas) caiu sobre o botao "Pausar",
        /// quando este esta visivel.
        /// </summary>
        public bool PauseClicked(Point location)
        {
            return pauseButtonVisible && pauseBounds.Contains(location);
        }
    }
}
