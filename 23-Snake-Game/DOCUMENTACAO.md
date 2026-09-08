# UVV Steam — Documentação do Jogo 23

> Documento vivo. Deve ser atualizado a cada alteração feita no projeto.
> Última atualização: 2026-09-08 (2ª revisão do dia)

---

## 1. Descritivo do projeto

### 1.1. Jogo base

O **Snake Game** deste catálogo é o "Classic Snakes Game Tutorial — MOO ICT", uma
aplicação **Windows Forms** em **C#** (`net10.0-windows7.0`, `Nullable` habilitado).

Estrutura original (antes das contribuições do grupo):

| Arquivo | Responsabilidade |
|---|---|
| `Program.cs` | Ponto de entrada; cria a `Form1`. |
| `Form1.cs` | Toda a lógica: laço do jogo (`gameTimer`), entrada de teclado, colisão, comida, pontuação, desenho e captura de tela. |
| `Form1.Designer.cs` | Layout: `picCanvas` (área de jogo), botões `Start` e `Snap`, rótulos de pontuação, `gameTimer`. |
| `Circle.cs` | Estrutura de posição `X`/`Y` de cada segmento da cobra e da comida. |
| `Settings.cs` | Constantes estáticas: tamanho da célula (`Width`/`Height`) e direção atual (`directions`). |

Funcionamento original: ao abrir, o jogador clica em **Start**; a cobra se move em
intervalos fixos de `gameTimer` (10 ms); comer a comida aumenta o placar e o corpo;
colidir com o próprio corpo encerra a partida, reabilitando os botões e atualizando
o *high score*. As bordas do canvas fazem *wrap* (a cobra reaparece do lado oposto).

### 1.2. Requisitos obrigatórios da "Atividade 01" e onde são aplicados

| Requisito | Aplicação no projeto |
|---|---|
| Escolher 2 jogos e compreendê-los | Snake Game + _(2º jogo)_. Compreensão registrada na seção 1.1. |
| Inserir **novas funcionalidades** | Seção 4 (lista) e seção 5 (status antes/depois). |
| Documentar contribuições com status antes/depois | Seção 5. |
| Diagrama de **casos de uso** (UML) | Seção 6.1. |
| **Modelagem de classes** (UML) | Seção 6.2. |
| Paradigma orientado a objetos | Novas classes `MainMenu`, `MenuOption`, `GameHeader`, enums `GameState` e `Difficulty` (seção 5.2). |
| Coleções, associação, navegabilidade | `MainMenu` contém uma coleção de `MenuOption` (associação 1..*); `Form1` conhece três instâncias de `MainMenu` (principal, dificuldade e game over). |
| GUI | Windows Forms + desenho GDI+ no canvas. |
| Gestão de tarefas | Seção 3. |
| Uso e documentação de IA | Seção 7. |

---

## 2. Histórico do planejamento

| Data | Sessão | Atividade planejada | Status | Responsável |
|---|---|---|---|---|
| 2026-09-01 | 1 | Levantar funcionalidades a adicionar (pausa, menu, dificuldade, som, game over visível) | Concluído | Luiz |
| 2026-09-01 | 1 | Implementar **menu principal** | Concluído | Luiz |
| 2026-09-01 | 1 | Corrigir navegação do menu por teclado (setas + Enter) | Concluído | Luiz |
| 2026-09-01 | 2 | Cabeçalho fixo (HUD) com pontuação, recorde e botão Snap | Concluído | Luiz |
| 2026-09-01 | 2 | Reduzir o tabuleiro (20×20) e deixar a cobra mais lenta (120 ms) | Concluído | Luiz |
| 2026-09-04 | 3 | Implementar **pausa** (botão no cabeçalho, teclas Esc/Espaço/P) | Concluído | Luiz |
| 2026-09-04 | 3 | Corrigir tabuleiro sem limite visível e sem centralizar ao redimensionar a janela | Concluído | Luiz |
| 2026-09-04 | 3 | Corrigir surgimento da cobra (segmentos nascendo empilhados em 0,0) | Concluído | Luiz |
| 2026-09-08 | 4 | **Dificuldade**: tela de seleção (Fácil / Médio / PRO), enum `Difficulty`, `Settings.ApplyDifficulty` (tamanho da arena, paredes sólidas x wrap, meta de pontos) | Concluído | Gabriel |
| 2026-09-08 | 4 | Implementar tela de **"Game Over"** e tela de **"Vitória"** (estado `GameOver`, `gameOverMenu`, `EndGame(victory)`) | Concluído | Gabriel |
| 2026-09-08 | 4 | **Pontuação / meta**: condição de vitória por pontos (`Settings.TargetScore`); tela de resultado mostra pontuação e recorde | Concluído | Gabriel |
| 2026-09-08 | 4 | Restringir o **recorde** ao modo PRO e fazer a **cobra acelerar a cada maçã**, também só no PRO (`Settings.TracksHighScore`, `Settings.SnakeSpeedsUp`) | Concluído | Luiz e Gabriel |

> Manter esta tabela sincronizada com a ferramenta de gestão (prints como evidência).

---

## 3. Funcionalidades que o grupo irá adicionar

1. **Menu principal** — ✅ implementado (sessão 1).
2. **Cabeçalho fixo (HUD)** com pontuação, recorde e botão Snap — ✅ implementado (sessão 2).
3. **Pausa** — ✅ implementado (sessão 3).
4. **Dificuldade** (Fácil / Médio / PRO): tela de seleção que define o tamanho da
   arena, o comportamento das paredes e a meta de pontos — ✅ implementado (sessão 4, PR).
5. **Tela de "Game Over" e de "Vitória"** no canvas, com pontuação/recorde e
   opções "Jogar novamente" / "Menu principal" — ✅ implementado (sessão 4, PR).
6. **Pontuação com meta de vitória** por dificuldade (`Settings.TargetScore`) —
   ✅ implementado (sessão 4, PR).
7. **Velocidade dinâmica** da cobra (acelera a cada maçã) — ✅ implementado
   **apenas no modo PRO** (sessão 4); os modos Fácil e Médio continuam com
   velocidade fixa.
8. **Recorde (*high score*) exclusivo do modo PRO** — ✅ implementado (sessão 4).
9. **Som** (efeitos e/ou música, com liga/desliga) — pendente.

> A redução do tabuleiro para 20×20 e a lentidão inicial da cobra (120 ms) na
> sessão 2 foram a base para o item 4: os valores foram movidos para
> `Settings` (`Columns`, `Rows`, `CellSize`, `SnakeSpeedMs`) e agora variam com a
> dificuldade via `Settings.ApplyDifficulty`. A aceleração do item 7 e o recorde
> do item 8 foram deliberadamente restritos ao PRO, o único modo sem meta de
> pontos — ver seção 4.7.

---

## 4. Contribuições — status ANTES x DEPOIS

### 4.1. Menu principal

**Descrição:** tela inicial desenhada diretamente no canvas (GDI+), com as opções
**Jogar** e **Sair**. Navegação por teclado (setas + Enter) e por mouse (o cursor
destaca a opção, o clique a ativa). O menu aparece ao abrir o jogo e sempre que a
partida termina.

| Aspecto | ANTES | DEPOIS |
|---|---|---|
| Início da partida | Botão `Start` (controle solto ao lado do canvas) | Opção **Jogar** no menu, dentro do canvas |
| Encerrar o jogo | Fechar a janela (X) | Opção **Sair** no menu, além do X |
| Estado do jogo | Inexistente (sempre "jogando") | Enum `GameState` (`Menu` / `Playing`), campo `state` em `Form1` |
| Fim de partida | Reabilitava os botões `Start`/`Snap` | Retorna ao menu principal (`ShowMenu()`) |
| Botão `Start` | Presente | **Removido** (`Form1.cs` e `Form1.Designer.cs`); `Snap` reposicionado |
| Navegação por teclado | Só a direção da cobra | `ProcessCmdKey` intercepta setas/Enter no menu antes da navegação entre controles |
| Entrada de mouse no canvas | Nenhuma | Eventos `MouseMove` (destaque) e `MouseClick` (ativação) |
| Orientação a objetos | Toda a lógica em `Form1` | Novas classes `MainMenu` e `MenuOption` isolam o menu |

**Arquivos novos:** `GameState.cs`, `MenuOption.cs`, `MainMenu.cs`.
**Arquivos alterados:** `Form1.cs`, `Form1.Designer.cs`.

**Ajustes de robustez feitos junto:**

- `RestartGame()` passou a zerar `Settings.directions` e as flags `goLeft/goRight/goUp/goDown`.
  Sem isso, uma segunda partida iniciada pelo menu herdava o movimento da anterior.
- `GameTimerEvent` retorna imediatamente se `state != Playing`, evitando processar
  um *tick* já enfileirado após o fim da partida.
- Chamadas a `picCanvas.Invalidate()` no caminho de "Sair" são protegidas por
  `!picCanvas.IsDisposed`.

**Correção posterior (mesma sessão):** a navegação por setas + Enter não funcionava
— as setas moviam o foco para o botão `Snap` e o Enter o acionava. Causa: a
navegação entre controles do WinForms ocorre antes do evento `KeyDown`. Solução:
sobrescrever `ProcessCmdKey` na `Form1` para tratar as setas e o Enter enquanto o
menu está visível. O tratamento no `KeyDown` (`KeyIsDown`) foi removido para o
estado de menu.

### 4.2. Cabeçalho fixo (HUD)

**Descrição:** faixa desenhada no topo do canvas (GDI+), visível tanto no menu
quanto durante a partida. Mostra `Pontos` e `Recorde` e traz o botão **Snap**
(captura de tela) desenhado, não mais como controle solto.

| Aspecto | ANTES | DEPOIS |
|---|---|---|
| Pontuação | `Label txtScore` à direita do canvas | Texto desenhado no cabeçalho |
| Recorde | `Label txtHighScore` à direita do canvas | Texto desenhado no cabeçalho |
| Captura de tela | Botão `snapButton` (controle solto) | Área "Snap" desenhada no cabeçalho, clicável em qualquer estado |
| Captura de tela (legenda) | `TakeSnapShot` sobrepunha um `Label` com "I scored: X and my Highscore is Y..." antes de salvar | Legenda removida; a captura salva o canvas tal como está na tela |
| Pausar | Inexistente | Área "Pausar"/"Retomar" desenhada no cabeçalho, ao lado do Snap, visível só durante a partida |
| Persistência na tela | Rótulos sempre visíveis fora da área de jogo | Cabeçalho fixo dentro do canvas, no menu e no jogo |
| Área de jogo | Ocupava todo o `picCanvas` | Começa abaixo do cabeçalho (`GameHeader.Height`) |
| Orientação a objetos | — | Nova classe `GameHeader` (desenho + teste de clique no Snap) |

**Arquivos novos:** `GameHeader.cs`.
**Arquivos alterados:** `Form1.cs`, `Form1.Designer.cs`, `MainMenu.cs` (o método
`Draw` passou a receber um `Rectangle` com a área útil abaixo do cabeçalho, em vez
de `Size`).

**Detalhes:**

- `Form1` desenha o cabeçalho por último em `UpdatePictureBoxGraphics`, sobre o
  menu ou sobre a partida.
- `CanvasMouseClick` testa primeiro `header.SnapClicked(...)`; o clique no Snap
  funciona no menu e durante o jogo.
- Removidos os controles `snapButton`, `txtScore` e `txtHighScore` do
  `Form1.Designer.cs`; `TakeSnapShot` deixou de ser *event handler* e virou um
  método comum.

### 4.3. Tabuleiro pequeno e cobra mais lenta

**Descrição:** o tabuleiro passou a ter dimensão fixa em número de células
(20×20) e a cobra ficou mais lenta. Os parâmetros foram centralizados em
`Settings` para depois dependerem da dificuldade.

| Aspecto | ANTES | DEPOIS |
|---|---|---|
| Dimensão do tabuleiro | Derivada do tamanho em pixels do `picCanvas` (≈ 36×42 células) | `Settings.Columns` × `Settings.Rows` = 20 × 20 |
| Célula | `Settings.Width` / `Settings.Height` = 16 px | `Settings.CellSize` = 20 px |
| Velocidade | `gameTimer.Interval` fixo em 10 ms (muito rápida) | `Settings.SnakeSpeedMs` = 120 ms, aplicado em `RestartGame` |
| `picCanvas` | 580 × 680 | 400 × 444 (400 de jogo + 44 de cabeçalho) |
| Limites do jogo | `maxWidth`/`maxHeight` calculados por pixel | `Settings.Columns - 1` / `Settings.Rows - 1` |
| Sorteio da comida | `rand.Next(2, max)` (excluía bordas) | `rand.Next(0, max + 1)` (tabuleiro inteiro) |

**Arquivos alterados:** `Settings.cs`, `Form1.cs`, `Form1.Designer.cs`.

**Limpeza junto:** `Settings.directions` passou a ser inicializado na declaração,
eliminando o *warning* `CS8618` que existia antes. O *build* agora conclui com
**0 warnings**.

### 4.4. Pausa

**Descrição:** durante a partida, o jogador pode pausar e retomar o jogo a
qualquer momento — pelo botão **Pausar/Retomar** no cabeçalho, ou pelas teclas
**Esc**, **Espaço** ou **P**. Enquanto pausado, o `gameTimer` para (a cobra
congela no lugar), um aviso **"PAUSADO"** é desenhado sobre o tabuleiro e o
botão do cabeçalho passa a exibir "Retomar".

| Aspecto | ANTES | DEPOIS |
|---|---|---|
| Pausar a partida | Não existia; só era possível fechar a janela ou perder | Botão "Pausar" no cabeçalho e teclas Esc/Espaço/P alternam entre jogando e pausado |
| Estado do jogo | `GameState` com `Menu` / `Playing` | Novo valor `Paused` no enum `GameState` |
| Laço do jogo durante a pausa | — | `gameTimer.Stop()` ao pausar, `gameTimer.Start()` ao retomar (`TogglePause()`) |
| Feedback visual da pausa | — | Overlay escurecido com o texto "PAUSADO" e a dica de teclas, desenhado sobre o tabuleiro |
| Cabeçalho durante a pausa | — | Botão alterna o rótulo entre "Pausar" e "Retomar" conforme o estado |

**Arquivos alterados:** `GameState.cs`, `GameHeader.cs`, `Form1.cs`.

**Detalhes:**

- `GameHeader.Draw` ganhou os parâmetros `showPauseButton` e `isPaused`: o botão
  só é desenhado durante a partida (não aparece no menu) e troca de rótulo
  conforme o estado.
- `CanvasMouseClick` testa `header.PauseClicked(...)` depois do Snap, chamando
  `TogglePause()`.
- `KeyIsDown` trata `P`, `Escape` e `Espaço` como atalhos de pausa, antes de
  qualquer verificação de movimento (evita mover a cobra no mesmo toque que
  pausa/retoma).

**Correções feitas junto (mesma sessão):**

- **Tabuleiro sem limite visível:** o `picCanvas` era redimensionado com a
  janela, mas o tabuleiro (área jogável) tinha tamanho fixo em pixels, sem
  nenhuma borda — ficava difícil enxergar onde ele realmente terminava dentro
  da janela maior. Agora o tabuleiro é desenhado com uma borda verde nítida,
  fixo no topo e centralizado horizontalmente quando a janela é maior que ele.
- **Repaint incompleto ao redimensionar:** no menu e na pausa, redimensionar a
  janela deixava pixels antigos ("fantasmas") no canvas, porque o WinForms só
  invalida por padrão a faixa recém-exposta, não o controle inteiro. Durante a
  partida isso passava despercebido porque o `gameTimer` já forçava um repaint
  completo a cada *tick*. Corrigido com
  `picCanvas.SizeChanged += (s, e) => picCanvas.Invalidate();`.
- **Borda entre a janela e o canvas:** `picCanvas` tinha uma margem fixa
  (9px/12px) em relação à borda do formulário. Trocado `Anchor` por
  `Dock = DockStyle.Fill`, eliminando a folga.
- **Cobra nascendo "quebrada":** a cobra iniciava com 1 cabeça posicionada e
  10 segmentos de corpo todos empilhados em `(0, 0)`, que só se separavam da
  pilha depois de vários *ticks* — visualmente estranho. Agora nasce com
  apenas 3 segmentos já alinhados no centro do tabuleiro (cabeça em
  `meio + 1`, corpo em `meio`, rabo em `meio - 1`). Isso exigiu também trocar
  a direção inicial de `"left"` para `"right"` em `RestartGame()`: com a
  cabeça posicionada à direita do corpo, mantê-la indo para a esquerda faria
  o primeiro movimento colidir com o próprio corpo (Game Over imediato).

### 4.5. Dificuldade (Fácil / Médio / PRO)

**Descrição:** ao escolher **Jogar** no menu principal, o jogo passa antes por uma
**tela de seleção de dificuldade** (novo estado `GameState.DifficultySelect`),
desenhada no canvas reaproveitando a classe `MainMenu` com o título "DIFICULDADE".
Cada opção traz uma descrição no rodapé explicando o que muda. A dificuldade
escolhida é aplicada por `Settings.ApplyDifficulty(...)` antes de a partida
começar.

| Nível | Arena (colunas × linhas) | Célula | Paredes | Meta de pontos | Velocidade | Recorde |
|---|---|---|---|---|---|---|
| **Fácil** | 26 × 26 | 16 px | Atravessáveis (*wrap*) | 10 (vitória) | Fixa (120 ms) | Não |
| **Médio** | 20 × 20 | 20 px | Sólidas (bater mata) | 20 (vitória) | Fixa (120 ms) | Não |
| **PRO** | 20 × 20 | 20 px | Sólidas (bater mata) | Sem meta — modo recorde infinito | Acelera a cada maçã | Sim |

| Aspecto | ANTES | DEPOIS |
|---|---|---|
| Escolha de dificuldade | Inexistente; parâmetros fixos em `Settings` | Tela dedicada (`DifficultySelect`) com Fácil / Médio / PRO / Voltar |
| Tamanho da arena | Fixo (`Columns`/`Rows`/`CellSize` constantes) | Definido por `Settings.ApplyDifficulty` conforme o nível |
| Bordas do tabuleiro | Sempre *wrap* (a cobra reaparece do lado oposto) | `Settings.WallsAreSolid`: *wrap* no Fácil, colisão fatal no Médio/PRO |
| Fim de jogo por pontos | Não existia | `Settings.TargetScore`: atingir a meta encerra a partida como **vitória** (Fácil/Médio); PRO não tem meta |
| Fluxo do menu | "Jogar" iniciava a partida direto | "Jogar" → tela de dificuldade → partida |
| Reinício | `RestartGame()` com valores fixos | `StartNewGame(Difficulty)` aplica a dificuldade e chama `RestartGame()`; `Settings.CurrentDifficulty` guarda o nível para o "Jogar novamente" |

**Arquivos novos:** `Difficulty.cs` (enum `Facil` / `Medio` / `Pro`).
**Arquivos alterados:** `Settings.cs` (novos campos `WallsAreSolid`, `TargetScore`,
`CurrentDifficulty` e método `ApplyDifficulty`), `Form1.cs` (`difficultyMenu`,
`ShowDifficultySelect`, `StartNewGame(Difficulty)`, tratamento de parede no
`GameTimerEvent`), `GameState.cs` (valor `DifficultySelect`), `MenuOption.cs` e
`MainMenu.cs` (opção passa a ter `Description` opcional, exibida no rodapé).

**Detalhes:**

- `GameTimerEvent` verifica `Settings.WallsAreSolid` ao bater na borda: se sólida,
  chama `EndGame(victory: false)`; senão, faz o *wrap* clássico.
- Colidir com o próprio corpo mata em **qualquer** nível, mesmo no Fácil.
- `Settings` usa `Difficulty.Medio` como padrão neutro no construtor, para haver
  valores válidos enquanto só o menu está na tela (o cabeçalho já é desenhado).

### 4.6. Telas de Game Over e de Vitória

**Descrição:** quando a partida termina — por derrota (bater na parede sólida ou
no próprio corpo) ou por vitória (atingir `Settings.TargetScore`) — o jogo entra
no estado `GameState.GameOver` e mostra uma tela de resultado no canvas, também
construída com `MainMenu`. O título é **"GAME OVER"** ou **"VOCE VENCEU!"**
conforme o resultado, o subtítulo mostra `Pontuação: X   Recorde: Y`, e as opções
são **"Jogar novamente"** (repete a mesma dificuldade) e **"Menu principal"**.

| Aspecto | ANTES | DEPOIS |
|---|---|---|
| Fim de partida | Voltava direto ao menu principal (`ShowMenu()`) | Tela de resultado dedicada (`GameState.GameOver`) |
| Feedback de derrota | Nenhum aviso visível; a tela apenas trocava | Tela "GAME OVER" com pontuação e recorde |
| Condição/feedback de vitória | Não existia | Tela "VOCE VENCEU!" ao atingir a meta de pontos |
| Ações após o fim | Só a navegação normal do menu principal | "Jogar novamente" (mesma dificuldade, via `Settings.CurrentDifficulty`) e "Menu principal" |
| Atualização do recorde | Feita no encerramento genérico da partida | Centralizada em `EndGame(bool victory)`, que também guarda `lastGameWasVictory` |
| Estado do jogo | `Menu` / `Playing` / `Paused` | Novo valor `GameOver` no enum `GameState` |

**Arquivos alterados:** `GameState.cs` (valor `GameOver`), `Form1.cs`
(`gameOverMenu`, campo `lastGameWasVictory`, método `EndGame(bool victory)`,
desenho da tela em `UpdatePictureBoxGraphics`), `MainMenu.cs` (`Draw` ganhou os
parâmetros `title` e `subtitle`).

**Detalhes:**

- `EndGame(bool victory)` para o `gameTimer`, atualiza o recorde se necessário,
  registra `lastGameWasVictory`, muda o estado para `GameOver` e redesenha.
- `EatFood` dispara `EndGame(victory: true)` assim que `score >= TargetScore`; o
  `GameTimerEvent` checa logo depois se o estado ainda é `Playing` para não
  processar o resto do *tick*.
- `ActiveMenu()` devolve o menu correto para teclado/mouse conforme o estado
  (`menu`, `difficultyMenu` ou `gameOverMenu`); em `Playing`/`Paused` devolve
  `null`.

### 4.7. Recorde e aceleração exclusivos do modo PRO

**Descrição:** o **recorde** (*high score*) e a **aceleração progressiva** da
cobra passam a valer **somente no modo PRO** — o único sem meta de pontos, em
que o objetivo é justamente durar o máximo possível e bater a própria marca. Nos
modos Fácil e Médio o cabeçalho mostra apenas `Pontos: X` (sem "Recorde"), e a
velocidade continua fixa.

| Aspecto | ANTES | DEPOIS |
|---|---|---|
| Recorde | Atualizado e exibido em todos os modos | Só no PRO (`Settings.TracksHighScore`): `EndGame` só atualiza `highScore` se a flag estiver ligada |
| "Recorde" no cabeçalho | Sempre visível ("Pontos: X    Recorde: Y") | Exibido só quando `Settings.TracksHighScore` (novo parâmetro `showHighScore` em `GameHeader.Draw`) |
| "Recorde" na tela de resultado | Sempre no subtítulo ("Pontuacao: X   Recorde: Y") | Subtítulo mostra só "Pontuacao: X" fora do PRO |
| Velocidade da cobra | Fixa em `Settings.SnakeSpeedMs` (120 ms) em todos os modos | No PRO diminui `Settings.SpeedUpStepMs` (4 ms) por maçã comida, até o piso `Settings.MinSpeedMs` (60 ms); demais modos seguem fixos |
| Onde a aceleração é aplicada | — | `EatFood()`, logo após incrementar o placar, quando `Settings.SnakeSpeedsUp` é verdadeiro |
| Reinício de partida | — | `RestartGame()` volta `gameTimer.Interval` para `Settings.SnakeSpeedMs`, então a aceleração não vaza de uma partida para a outra |

**Arquivos alterados:** `Settings.cs` (novos: `SnakeSpeedsUp`, `TracksHighScore`,
constantes `SpeedUpStepMs` e `MinSpeedMs`; `ApplyDifficulty` liga as duas flags
só no `case Difficulty.Pro`), `Form1.cs` (`EatFood` acelera o timer; `EndGame`
condiciona o recorde; chamadas de `GameHeader.Draw` e o subtítulo do Game Over
passam `Settings.TracksHighScore`), `GameHeader.cs` (novo parâmetro
`showHighScore`).

**Detalhes:**

- A aceleração usa `Math.Max(Settings.MinSpeedMs, gameTimer.Interval - Settings.SpeedUpStepMs)`,
  garantindo que o intervalo nunca fique abaixo do piso.
- Com passo de 4 ms e piso de 60 ms, a cobra atinge a velocidade máxima por
  volta da 15ª maçã (120 → 60 ms).
- `highScore` é um campo da `Form1`, então sobrevive à troca de dificuldade
  dentro da mesma execução: jogar Médio depois de um PRO não zera nem exibe o
  recorde, e voltar ao PRO o mostra de novo.

---

## 5. Modelagem UML

### 5.1. Diagrama de casos de uso

_(inserir imagem do diagrama)_

Casos de uso já cobertos:

- **Iniciar partida** (via menu → "Jogar").
- **Sair do jogo** (via menu → "Sair").
- **Jogar Snake** (mover a cobra, comer, pontuar).
- **Capturar tela** (área "Snap" no cabeçalho).
- **Ver pontuação e recorde** (cabeçalho fixo).
- **Pausar/retomar partida** (botão "Pausar/Retomar" no cabeçalho ou teclas
  Esc/Espaço/P).
- **Escolher dificuldade** (tela "DIFICULDADE" → Fácil / Médio / PRO), que define
  o tamanho da arena, o comportamento das paredes e a meta de pontos.
- **Ver tela de resultado** ("GAME OVER" ou "VOCE VENCEU!") com a pontuação e,
  no modo PRO, o recorde; e escolher entre **Jogar novamente** e **Menu principal**.
- **Bater o recorde no modo PRO** (modo sem meta; o recorde só é contado e
  exibido nele, e a cobra acelera a cada maçã).

Casos de uso planejados: **Ligar/desligar som**.

### 5.2. Diagrama de classes (estado atual)

```
+-------------------------------+     +---------------------+
|            Form1              |     |     GameState        |  «enumeration»
+-------------------------------+     +---------------------+
| - state: GameState            |     | Menu                |
| - menu: MainMenu              | 1 1 | DifficultySelect    |
| - difficultyMenu: MainMenu    |---->| Playing             |
| - gameOverMenu: MainMenu      |     | Paused              |
| - header: GameHeader          |     | GameOver            |
| - Snake: List<Circle>         |     +---------------------+
| - food: Circle                |
| - score, highScore: int       |     +---------------------+
| - lastGameWasVictory: bool    | 1 1 |     Difficulty       |  «enumeration»
| + ProcessCmdKey()             |---->+---------------------+
| + ActiveMenu(): MainMenu?     |     | Facil               |
| + ShowMenu()                  |     | Medio               |
| + ShowDifficultySelect()      |     | Pro                 |
| + StartNewGame(Difficulty)    |     +---------------------+
| + TogglePause()               |
| + EndGame(victory: bool)      |     +---------------------------------+
| + TakeSnapShot()              | 1 3 |            MainMenu             |
+-------------------------------+---->+---------------------------------+
   | 1        | 1                     | - options: List<MenuOption>     |
   | 1        | *                     | + SelectedIndex: int            |
   v          v                       | + AddOption(label, Action, desc)|
+-------------------------+ +---------+ | + MoveUp() / MoveDown()         |
|       GameHeader        | |  Circle | | + ActivateSelected()            |
+-------------------------+ +---------+ | + HandleMouseMove(Point): bool  |
| + Height: const         | | + X:int | | + HandleMouseClick(Point): bool |
| + Draw(g, w, score, hi, | | + Y:int | | + Draw(g, area, title, subtitle)|
|   showHighScore,        | +---------+ +--------------+-----------------+
|   showPauseButton,      |                            | 1
|   isPaused)             |                            | *
| + SnapClicked(p)        |                    +-------v----------------+
| + PauseClicked(p)       |                    |      MenuOption         |
+-------------------------+                    +------------------------+
                                               | + Label: string        |
+----------------------------------+ «static»  | + OnSelected: Action   |
|            Settings              |           | + Description: string? |
+----------------------------------+           | + Bounds: Rectangle    |
| + CellSize / Columns / Rows: int |           +------------------------+
| + SnakeSpeedMs: int              |
| + SnakeSpeedsUp: bool    (só PRO) |
| + SpeedUpStepMs / MinSpeedMs: const int |
| + WallsAreSolid: bool            |
| + TargetScore: int?              |
| + TracksHighScore: bool  (só PRO) |
| + CurrentDifficulty: Difficulty  |
| + directions: string             |
| + ApplyDifficulty(Difficulty)    |
+----------------------------------+
```

_(substituir por diagrama formal — Astah / draw.io / Visual Studio)_

---

## 6. Uso de Inteligência Artificial

### 6.1. Nível da "AI Assessment Scale" empregado

**IA-2.** A IA auxiliou na **geração inicial de código** e na estruturação das
classes, com **revisão humana completa** antes da incorporação. O grupo mantém o
domínio da solução: as decisões de arquitetura, os critérios de aceitação e os
ajustes finais foram feitos pelos integrantes.

### 6.2. Ferramentas e alguns prompts utilizados

| Ferramenta | Uso |
|---|---|
| Claude Code (Claude Sonnet) | Geração inicial das classes do menu e ajustes guiados pelo grupo. |

**Prompts (resumo):**

- "Listar funcionalidades comuns a adicionar ao jogo: pausa, menu, dificuldade,
  som, game over visível. Começar pelo menu."
- "Menu desenhado no canvas (GDI+), opções Jogar e Sair, navegação por teclado e
  mouse, exibido ao abrir e após o game over."
- "As setas + Enter não iniciam o jogo; a seleção vai para o botão de Snap." →
  correção via `ProcessCmdKey`.
- "Desenhar o Snap e o score num cabeçalho que se mantém durante o jogo, na mesma
  tela do menu."
- "Mudar o tamanho do jogo para um tabuleiro pequeno e deixar a cobra mais lenta."
- "Tela de seleção de dificuldade (Fácil / Médio / PRO) que muda o tamanho da
  arena, se a parede mata ou faz *wrap*, e a meta de pontos para vencer." →
  enum `Difficulty`, `Settings.ApplyDifficulty`, estado `DifficultySelect`.
- "Tela de Game Over e de Vitória mostrando pontuação e recorde, com opções de
  jogar de novo (mesma dificuldade) e voltar ao menu." → estado `GameOver`,
  `gameOverMenu`, `EndGame(bool victory)`.

> Contribuição incorporada via **pull request** de um integrante do grupo (telas
> de dificuldade, game over, vitória e pontuação/meta). O merge foi feito no
> repositório; esta documentação foi atualizada em seguida para refletir o PR.

### 6.3. Critérios de revisão humana

- **Limpeza:** nomes claros, responsabilidade única por classe, comentários
  explicando o *porquê* (não o óbvio), sem código morto.
- **Consistência:** manter o estilo do arquivo original (chaves, indentação,
  nomenclatura em `PascalCase`/`camelCase`); não reformatar trechos não tocados.
- **Robustez:** tratar estado inválido (tick após game over, reinício de partida,
  objeto já descartado ao sair), reiniciar variáveis de movimento entre partidas.
- **Compilação:** `dotnet build` sem erros e sem *warnings* (o `CS8618` que
  existia em `Settings.cs` foi eliminado ao inicializar `directions` na
  declaração — ver seção 4.3).

### 6.4. Evidências comparativas (com IA x sem IA)

_(Preencher com um trecho reescrito manualmente pelo grupo e comparado com a
versão gerada pela IA — exigência da atividade. Sugestão: comparar a
implementação do `MainMenu.Draw` ou do `ProcessCmdKey`.)_

| Item | Versão gerada pela IA | Ajuste/checagem humana | Reflexão |
|---|---|---|---|
| _(preencher)_ | _(preencher)_ | _(preencher)_ | _(preencher: validade, limitações)_ |

### 6.5. Tarefas realizadas sem IA (IA-0)

_(Declarar explicitamente aqui quais tarefas foram feitas manualmente — por
exemplo, a modelagem UML formal, a redação da análise crítica e o planejamento
na ferramenta de gestão.)_

---

## 7. Registro de alterações (changelog)

| Data | Alteração | Arquivos |
|---|---|---|
| 2026-09-01 | Menu principal (Jogar/Sair), enum `GameState`, classes `MainMenu`/`MenuOption`; remoção do botão `Start`; ajustes de robustez no reinício e no laço do jogo. | `GameState.cs`, `MenuOption.cs`, `MainMenu.cs` (novos); `Form1.cs`, `Form1.Designer.cs` |
| 2026-09-01 | Correção da navegação do menu por teclado via `ProcessCmdKey` (setas + Enter deixavam de operar o menu e acionavam o botão `Snap`). | `Form1.cs` |
| 2026-09-01 | Cabeçalho fixo (HUD): classe `GameHeader` desenha pontuação, recorde e botão Snap no topo do canvas, no menu e no jogo. Removidos os controles `snapButton`, `txtScore` e `txtHighScore`. `MainMenu.Draw` passou a receber `Rectangle`. | `GameHeader.cs` (novo); `Form1.cs`, `Form1.Designer.cs`, `MainMenu.cs` |
| 2026-09-01 | Tabuleiro reduzido para 20×20 (célula 20 px) e cobra mais lenta (120 ms). Parâmetros movidos para `Settings` (`CellSize`, `Columns`, `Rows`, `SnakeSpeedMs`); `directions` inicializado na declaração (elimina *warning* `CS8618`). | `Settings.cs`, `Form1.cs`, `Form1.Designer.cs` |
| 2026-09-04 | Pausa: botão "Pausar/Retomar" no cabeçalho, teclas Esc/Espaço/P, novo valor `Paused` em `GameState`, overlay "PAUSADO" sobre o tabuleiro. Removida a legenda de pontuação/recorde que era sobreposta ao usar o Snap. | `GameState.cs`, `GameHeader.cs`, `Form1.cs` |
| 2026-09-04 | Tabuleiro com borda visível, fixo no topo e centralizado horizontalmente quando a janela é maior que ele. Corrigido repaint incompleto do menu/pausa ao redimensionar (`picCanvas.SizeChanged`). `picCanvas` passou a usar `Dock = Fill`, eliminando a margem fixa entre a janela e o canvas. | `Form1.cs`, `Form1.Designer.cs` |
| 2026-09-04 | Cobra passa a nascer com 3 segmentos já alinhados no centro do tabuleiro (cabeça em `meio+1`, corpo em `meio`, rabo em `meio-1`), em vez de 11 segmentos com o corpo empilhado em `(0,0)`. Direção inicial trocada de `"left"` para `"right"` para evitar colisão da cabeça com o próprio corpo no primeiro movimento. | `Form1.cs` |
| 2026-09-08 | **Dificuldade** (PR): enum `Difficulty` (`Facil`/`Medio`/`Pro`), tela de seleção (estado `DifficultySelect`), `Settings.ApplyDifficulty` define tamanho da arena, `WallsAreSolid` (parede mata x *wrap*) e `TargetScore` (meta de pontos). `MainMenu.Draw` passou a aceitar `title`/`subtitle`; `MenuOption` ganhou `Description` (dica no rodapé). | `Difficulty.cs` (novo); `Settings.cs`, `GameState.cs`, `MenuOption.cs`, `MainMenu.cs`, `Form1.cs` |
| 2026-09-08 | **Telas de Game Over e de Vitória** (PR): novo valor `GameOver` em `GameState`; `Form1.EndGame(bool victory)` para o timer, atualiza o recorde e mostra o resultado ("GAME OVER" / "VOCE VENCEU!") com pontuação e recorde e as opções "Jogar novamente" / "Menu principal". Campo `lastGameWasVictory`; `ActiveMenu()` roteia teclado/mouse para o menu certo. | `GameState.cs`, `Form1.cs` |
| 2026-09-08 | **Vitória por pontos** (PR): `EatFood` encerra a partida como vitória ao atingir `Settings.TargetScore` (Fácil = 10, Médio = 20; PRO sem meta). `picCanvas`/`ClientSize` ajustados para 418×468. | `Form1.cs`, `Form1.Designer.cs` |
| 2026-09-08 | **Recorde e aceleração exclusivos do PRO**: `Settings.TracksHighScore` (só o PRO atualiza/exibe o recorde, no cabeçalho e na tela de resultado) e `Settings.SnakeSpeedsUp` (no PRO, `EatFood` reduz `gameTimer.Interval` em `SpeedUpStepMs` = 4 ms por maçã até o piso `MinSpeedMs` = 60 ms). Novo parâmetro `showHighScore` em `GameHeader.Draw`. Corrigidos os 4 *warnings* de anotação nula herdados do PR (`string?` em `MenuOption`/`MainMenu`, `MainMenu?` em `ActiveMenu`). | `Settings.cs`, `Form1.cs`, `GameHeader.cs`, `MainMenu.cs`, `MenuOption.cs` |
