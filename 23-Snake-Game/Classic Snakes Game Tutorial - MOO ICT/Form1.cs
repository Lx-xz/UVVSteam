using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Imaging; // add this for the JPG compressor

namespace Classic_Snakes_Game_Tutorial___MOO_ICT
{
    public partial class Form1 : Form
    {

        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        // Estado atual (menu ou partida) e os elementos desenhados no canvas.
        private GameState state = GameState.Menu;
        private readonly MainMenu menu = new MainMenu();
        private readonly GameHeader header = new GameHeader();

        int maxWidth;
        int maxHeight;

        int score;
        int highScore;

        Random rand = new Random();

        bool goLeft, goRight, goDown, goUp;


        public Form1()
        {
            InitializeComponent();

            new Settings();

            menu.AddOption("Jogar", StartNewGame);
            menu.AddOption("Sair", Close);

            // Por padrao o WinForms so invalida a faixa recem-exposta ao
            // redimensionar, deixando pixels antigos no restante do canvas.
            // Isso passa despercebido durante a partida porque o gameTimer
            // ja redesenha tudo a cada tick, mas no menu e na pausa nada
            // força um repaint completo sem isso.
            picCanvas.SizeChanged += (s, e) => picCanvas.Invalidate();

            ShowMenu();
        }

        /// <summary>Exibe o menu principal e interrompe qualquer partida em andamento.</summary>
        private void ShowMenu()
        {
            state = GameState.Menu;
            gameTimer.Stop();
            menu.Reset();
            picCanvas.Invalidate();
        }

        /// <summary>Inicia uma nova partida a partir do menu.</summary>
        private void StartNewGame()
        {
            state = GameState.Playing;
            RestartGame();
        }

        /// <summary>
        /// Intercepta as setas e o Enter antes da navegacao padrao entre
        /// controles do formulario. Sem isso as setas moveriam o foco para o
        /// botao Snap e o Enter o acionaria, em vez de operar o menu.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (state == GameState.Menu)
            {
                switch (keyData)
                {
                    case Keys.Up:
                        menu.MoveUp();
                        picCanvas.Invalidate();
                        return true;
                    case Keys.Down:
                        menu.MoveDown();
                        picCanvas.Invalidate();
                        return true;
                    case Keys.Enter:
                        menu.ActivateSelected();
                        if (!picCanvas.IsDisposed)
                        {
                            picCanvas.Invalidate();
                        }
                        return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (state != GameState.Menu) return;

            if (menu.HandleMouseMove(e.Location))
            {
                picCanvas.Invalidate();
            }
        }

        private void CanvasMouseClick(object sender, MouseEventArgs e)
        {
            // O cabecalho (e o botao "Snap") respondem em qualquer estado.
            if (header.SnapClicked(e.Location))
            {
                TakeSnapShot();
                return;
            }

            if (header.PauseClicked(e.Location))
            {
                TogglePause();
                return;
            }

            if (state == GameState.Menu
                && menu.HandleMouseClick(e.Location)
                && !picCanvas.IsDisposed)
            {
                picCanvas.Invalidate();
            }
        }

        /// <summary>Alterna entre partida em andamento e partida pausada.</summary>
        private void TogglePause()
        {
            if (state == GameState.Playing)
            {
                state = GameState.Paused;
                gameTimer.Stop();
                picCanvas.Invalidate();
            }
            else if (state == GameState.Paused)
            {
                state = GameState.Playing;
                gameTimer.Start();
                picCanvas.Invalidate();
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            // No menu as teclas sao tratadas em ProcessCmdKey.
            if (state == GameState.Menu)
            {
                return;
            }

            if (e.KeyCode == Keys.P || e.KeyCode == Keys.Escape || e.KeyCode == Keys.Space)
            {
                TogglePause();
                return;
            }

            if (state == GameState.Paused)
            {
                return;
            }

            if (e.KeyCode == Keys.Left && Settings.directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void TakeSnapShot()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake Game SnapShot MOO ICT";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
            }

        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (state != GameState.Playing)
            {
                return;
            }

            // setting the directions

            if (goLeft)
            {
                Settings.directions = "left";
            }
            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }
            // end of directions

            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {

                    switch (Settings.directions)
                    {
                        case "left":
                            Snake[i].X--;
                            break;
                        case "right":
                            Snake[i].X++;
                            break;
                        case "down":
                            Snake[i].Y++;
                            break;
                        case "up":
                            Snake[i].Y--;
                            break;
                    }

                    if (Snake[i].X < 0)
                    {
                        Snake[i].X = maxWidth;
                    }
                    if (Snake[i].X > maxWidth)
                    {
                        Snake[i].X = 0;
                    }
                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = maxHeight;
                    }
                    if (Snake[i].Y > maxHeight)
                    {
                        Snake[i].Y = 0;
                    }


                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        EatFood();
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {

                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            GameOver();
                        }

                    }


                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }


            picCanvas.Invalidate();

        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (state == GameState.Menu)
            {
                menu.Draw(canvas, new Rectangle(
                    0, GameHeader.Height,
                    picCanvas.Width, picCanvas.Height - GameHeader.Height));
                header.Draw(canvas, picCanvas.Width, score, highScore, false, false);
                return;
            }

            // Area disponivel abaixo do cabecalho. O tabuleiro fica preso ao
            // topo dessa area e centralizado horizontalmente quando a janela
            // e maior do que ele, com uma borda para deixar seu limite visivel.
            var playArea = new Rectangle(0, GameHeader.Height, picCanvas.Width, picCanvas.Height - GameHeader.Height);
            int boardWidth = Settings.Columns * Settings.CellSize;
            int boardHeight = Settings.Rows * Settings.CellSize;
            int boardLeft = playArea.Left + Math.Max(0, (playArea.Width - boardWidth) / 2);
            int boardTop = playArea.Top;
            var boardArea = new Rectangle(boardLeft, boardTop, boardWidth, boardHeight);

            using (var outsideBrush = new SolidBrush(Color.FromArgb(20, 20, 20)))
            {
                canvas.FillRectangle(outsideBrush, playArea);
            }
            using (var boardBrush = new SolidBrush(Color.Silver))
            {
                canvas.FillRectangle(boardBrush, boardArea);
            }

            Brush snakeColour;

            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColour = Brushes.Black;
                }
                else
                {
                    snakeColour = Brushes.DarkGreen;
                }

                canvas.FillEllipse(snakeColour, new Rectangle
                    (
                    boardLeft + Snake[i].X * Settings.CellSize,
                    boardTop + Snake[i].Y * Settings.CellSize,
                    Settings.CellSize, Settings.CellSize
                    ));
            }


            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
            (
            boardLeft + food.X * Settings.CellSize,
            boardTop + food.Y * Settings.CellSize,
            Settings.CellSize, Settings.CellSize
            ));

            using (var boardBorder = new Pen(Color.LimeGreen, 2))
            {
                canvas.DrawRectangle(boardBorder, boardArea.X, boardArea.Y, boardArea.Width - 1, boardArea.Height - 1);
            }

            if (state == GameState.Paused)
            {
                DrawPauseOverlay(canvas, boardArea);
            }

            header.Draw(canvas, picCanvas.Width, score, highScore, true, state == GameState.Paused);
        }

        /// <summary>Desenha o aviso de "PAUSADO" sobre o tabuleiro.</summary>
        private void DrawPauseOverlay(Graphics canvas, Rectangle boardArea)
        {
            using (var overlay = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
            {
                canvas.FillRectangle(overlay, boardArea);
            }

            using (var titleFont = new Font("Microsoft Sans Serif", 22f, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(Color.LimeGreen))
            using (var hintFont = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular))
            using (var hintBrush = new SolidBrush(Color.Gainsboro))
            using (var centered = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                var titleArea = new RectangleF(boardArea.X, boardArea.Y + boardArea.Height / 2 - 40, boardArea.Width, 50);
                canvas.DrawString("PAUSADO", titleFont, titleBrush, titleArea, centered);

                var hintArea = new RectangleF(boardArea.X, boardArea.Y + boardArea.Height / 2 + 10, boardArea.Width, 30);
                canvas.DrawString("Pressione Esc, Espaco ou P, ou clique em Retomar, para continuar", hintFont, hintBrush, hintArea, centered);
            }
        }

        private void RestartGame()
        {
            maxWidth = Settings.Columns - 1;
            maxHeight = Settings.Rows - 1;

            Snake.Clear();

            // Zera a direcao e as teclas retidas para que uma nova partida
            // nao herde o movimento da anterior. A cabeca nasce a direita do
            // corpo, entao a direcao inicial precisa ser "right": partindo
            // para "left" a cabeca andaria para cima do proprio corpo e
            // colidiria consigo mesma no primeiro passo.
            Settings.directions = "right";
            goLeft = goRight = goUp = goDown = false;

            gameTimer.Interval = Settings.SnakeSpeedMs;
            score = 0;

            // Comeca com 3 segmentos ja alinhados no meio do tabuleiro
            // (cabeca a frente, rabo atras), em vez de nascer com varios
            // segmentos empilhados em (0,0) esperando a cabeca se afastar.
            int midX = Settings.Columns / 2;
            int midY = Settings.Rows / 2;

            Snake.Add(new Circle { X = midX + 1, Y = midY }); // cabeca
            Snake.Add(new Circle { X = midX, Y = midY });
            Snake.Add(new Circle { X = midX - 1, Y = midY }); // rabo

            food = new Circle { X = rand.Next(0, maxWidth + 1), Y = rand.Next(0, maxHeight + 1) };

            gameTimer.Start();

        }

        private void EatFood()
        {
            score += 1;

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };

            Snake.Add(body);

            food = new Circle { X = rand.Next(0, maxWidth + 1), Y = rand.Next(0, maxHeight + 1) };


        }

        private void GameOver()
        {
            gameTimer.Stop();

            if (score > highScore)
            {
                highScore = score;
            }

            // Volta ao menu principal apos a derrota.
            ShowMenu();
        }


    }
}
