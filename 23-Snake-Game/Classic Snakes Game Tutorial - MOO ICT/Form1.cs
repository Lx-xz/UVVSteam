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

        // Estado atual (menu, dificuldade, partida, pausa ou game over) e os
        // elementos desenhados no canvas. Os tres menus reaproveitam a mesma
        // classe MainMenu (label + acao + descricao opcional), so mudando o
        // titulo/subtitulo/opcoes passados para Draw().
        private GameState state = GameState.Menu;
        private readonly MainMenu menu = new MainMenu();
        private readonly MainMenu difficultyMenu = new MainMenu();
        private readonly MainMenu gameOverMenu = new MainMenu();
        private readonly GameHeader header = new GameHeader();

        int maxWidth;
        int maxHeight;

        int score;
        int highScore;

        /// <summary>Verdadeiro se a ultima partida terminou por vitoria (meta de pontos atingida), falso se foi derrota.</summary>
        bool lastGameWasVictory;

        Random rand = new Random();

        bool goLeft, goRight, goDown, goUp;


        public Form1()
        {
            InitializeComponent();

            new Settings();

            // Musica de fundo em loop durante toda a aplicacao. O volume cai
            // para metade fora da partida (ver UpdateMusicVolume).
            BackgroundMusic.Start();

            menu.AddOption("Jogar", ShowDifficultySelect);
            menu.AddOption("Sair", Close);

            difficultyMenu.AddOption("Facil", () => StartNewGame(Difficulty.Facil),
                "Arena maior, paredes atravessaveis, meta de 10 pontos para vencer");
            difficultyMenu.AddOption("Medio", () => StartNewGame(Difficulty.Medio),
                "Arena padrao, paredes solidas (matam), meta de 20 pontos para vencer");
            difficultyMenu.AddOption("PRO", () => StartNewGame(Difficulty.Pro),
                "Arena padrao, paredes solidas, sem meta: quebre seu recorde");
            difficultyMenu.AddOption("Voltar", ShowMenu);

            // As acoes de "Jogar novamente" e "Menu principal" sao montadas
            // dinamicamente (dependem da dificuldade da partida que acabou
            // de terminar), entao ficam mais faceis de ler direto no metodo
            // EndGame do que aqui no construtor.
            gameOverMenu.AddOption("Jogar novamente", () => StartNewGame(Settings.CurrentDifficulty));
            gameOverMenu.AddOption("Menu principal", ShowMenu);

            // Por padrao o WinForms so invalida a faixa recem-exposta ao
            // redimensionar, deixando pixels antigos no restante do canvas.
            // Isso passa despercebido durante a partida porque o gameTimer
            // ja redesenha tudo a cada tick, mas no menu e na pausa nada
            // força um repaint completo sem isso.
            picCanvas.SizeChanged += (s, e) => picCanvas.Invalidate();

            ShowMenu();
        }

        /// <summary>
        /// Devolve o menu que deve receber teclado/mouse no estado atual, ou
        /// null quando o estado atual nao usa um menu (Playing/Paused, que
        /// tem seu proprio tratamento de teclas).
        /// </summary>
        private MainMenu? ActiveMenu()
        {
            switch (state)
            {
                case GameState.Menu: return menu;
                case GameState.DifficultySelect: return difficultyMenu;
                case GameState.GameOver: return gameOverMenu;
                default: return null;
            }
        }

        /// <summary>Exibe o menu principal e interrompe qualquer partida em andamento.</summary>
        private void ShowMenu()
        {
            state = GameState.Menu;
            UpdateMusicVolume();
            gameTimer.Stop();
            menu.Reset();
            picCanvas.Invalidate();
        }

        /// <summary>Exibe a tela de escolha de dificuldade (a partir do menu principal).</summary>
        private void ShowDifficultySelect()
        {
            state = GameState.DifficultySelect;
            UpdateMusicVolume();
            difficultyMenu.Reset();
            picCanvas.Invalidate();
        }

        /// <summary>Aplica a dificuldade escolhida e inicia uma nova partida.</summary>
        private void StartNewGame(Difficulty difficulty)
        {
            Settings.ApplyDifficulty(difficulty);
            state = GameState.Playing;
            UpdateMusicVolume();
            RestartGame();
        }

        /// <summary>
        /// Deixa a musica de fundo no volume cheio durante a partida
        /// (<see cref="GameState.Playing"/>) e pela metade em qualquer outra
        /// tela — menu, escolha de dificuldade, pausa e game over. Deve ser
        /// chamado sempre que <see cref="state"/> mudar.
        /// </summary>
        private void UpdateMusicVolume()
        {
            if (state == GameState.Playing)
            {
                BackgroundMusic.SetGameplayVolume();
            }
            else
            {
                BackgroundMusic.SetMenuVolume();
            }
        }

        /// <summary>
        /// Intercepta as setas e o Enter antes da navegacao padrao entre
        /// controles do formulario. Sem isso as setas moveriam o foco para o
        /// botao Snap e o Enter o acionaria, em vez de operar o menu.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            MainMenu? activeMenu = ActiveMenu();

            if (activeMenu != null)
            {
                switch (keyData)
                {
                    case Keys.Up:
                        activeMenu.MoveUp();
                        picCanvas.Invalidate();
                        return true;
                    case Keys.Down:
                        activeMenu.MoveDown();
                        picCanvas.Invalidate();
                        return true;
                    case Keys.Enter:
                        activeMenu.ActivateSelected();
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
            MainMenu? activeMenu = ActiveMenu();
            if (activeMenu == null) return;

            if (activeMenu.HandleMouseMove(e.Location))
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

            MainMenu? activeMenu = ActiveMenu();
            if (activeMenu != null
                && activeMenu.HandleMouseClick(e.Location)
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
                UpdateMusicVolume();
                gameTimer.Stop();
                picCanvas.Invalidate();
            }
            else if (state == GameState.Paused)
            {
                state = GameState.Playing;
                UpdateMusicVolume();
                gameTimer.Start();
                picCanvas.Invalidate();
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            // Em qualquer tela de menu (principal, dificuldade ou game over)
            // as teclas sao tratadas em ProcessCmdKey, nao aqui.
            if (ActiveMenu() != null)
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

                    bool hitWall = Snake[i].X < 0 || Snake[i].X > maxWidth
                                || Snake[i].Y < 0 || Snake[i].Y > maxHeight;

                    if (hitWall)
                    {
                        if (Settings.WallsAreSolid)
                        {
                            // Facil: paredes vazam. Medio/PRO: paredes matam.
                            EndGame(victory: false);
                            return;
                        }

                        // Wrap: a cobra reaparece do lado oposto do tabuleiro.
                        if (Snake[i].X < 0) Snake[i].X = maxWidth;
                        if (Snake[i].X > maxWidth) Snake[i].X = 0;
                        if (Snake[i].Y < 0) Snake[i].Y = maxHeight;
                        if (Snake[i].Y > maxHeight) Snake[i].Y = 0;
                    }

                    if (Snake[i].X == food.X && Snake[i].Y == food.Y)
                    {
                        EatFood();

                        // EatFood() pode ter disparado a vitoria (meta de
                        // pontos atingida). Se o estado mudou, a partida
                        // acabou: nao ha mais nada a atualizar neste tick.
                        if (state != GameState.Playing)
                        {
                            return;
                        }
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {

                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            // Colidir com o proprio corpo mata em qualquer
                            // dificuldade, mesmo no Facil (onde a parede nao mata).
                            EndGame(victory: false);
                            return;
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

            var menuArea = new Rectangle(
                0, GameHeader.Height,
                picCanvas.Width, picCanvas.Height - GameHeader.Height);

            if (state == GameState.Menu)
            {
                menu.Draw(canvas, menuArea);
                header.Draw(canvas, picCanvas.Width, score, highScore, Settings.TracksHighScore, false, false);
                return;
            }

            if (state == GameState.DifficultySelect)
            {
                difficultyMenu.Draw(canvas, menuArea, title: "DIFICULDADE");
                header.Draw(canvas, picCanvas.Width, score, highScore, Settings.TracksHighScore, false, false);
                return;
            }

            if (state == GameState.GameOver)
            {
                string title = lastGameWasVictory ? "VOCE VENCEU!" : "GAME OVER";
                string subtitle = Settings.TracksHighScore
                    ? "Pontuacao: " + score + "   Recorde: " + highScore
                    : "Pontuacao: " + score;
                gameOverMenu.Draw(canvas, menuArea, title, subtitle);
                header.Draw(canvas, picCanvas.Width, score, highScore, Settings.TracksHighScore, false, false);
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

            header.Draw(canvas, picCanvas.Width, score, highScore, Settings.TracksHighScore, true, state == GameState.Paused);
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

            // No modo Pro a cobra acelera um pouco a cada maca (o intervalo
            // do timer diminui), ate o piso de Settings.MinSpeedMs. Nos
            // outros modos Settings.SnakeSpeedsUp e falso e a velocidade
            // fica fixa em Settings.SnakeSpeedMs.
            if (Settings.SnakeSpeedsUp && gameTimer.Interval > Settings.MinSpeedMs)
            {
                gameTimer.Interval = Math.Max(
                    Settings.MinSpeedMs,
                    gameTimer.Interval - Settings.SpeedUpStepMs);
            }

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };

            Snake.Add(body);

            food = new Circle { X = rand.Next(0, maxWidth + 1), Y = rand.Next(0, maxHeight + 1) };

            // Facil e Medio tem uma meta de pontos (Settings.TargetScore).
            // O Pro nao tem meta (TargetScore == null) e so termina quando a
            // cobra morre, entao esta checagem nunca dispara nele.
            if (Settings.TargetScore.HasValue && score >= Settings.TargetScore.Value)
            {
                EndGame(victory: true);
            }
        }

        /// <summary>
        /// Encerra a partida atual (por vitoria ou derrota), atualiza o
        /// recorde se necessario e mostra a tela de Game Over com o
        /// resultado.
        /// </summary>
        private void EndGame(bool victory)
        {
            gameTimer.Stop();

            // O recorde so e mantido no modo Pro (Settings.TracksHighScore);
            // Facil e Medio tem meta fixa de pontos, entao nao guardam recorde.
            if (Settings.TracksHighScore && score > highScore)
            {
                highScore = score;
            }

            lastGameWasVictory = victory;
            state = GameState.GameOver;
            UpdateMusicVolume();
            gameOverMenu.Reset();
            picCanvas.Invalidate();
        }


    }
}
