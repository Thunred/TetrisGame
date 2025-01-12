using Raylib_cs;
using System;

class TetrisGame
{
    private Grid grid;
    private Tetromino currentPiece;
    private Tetromino nextPiece;
    private Tetromino heldPiece; // La pièce gardée

    private bool gameOver;
    private int gravityCounter = 0;
    private int gravitySpeed;

    private int gravityAdd = 10;
    private bool canHold = true; // Pour s'assurer qu'on peut garder une pièce seulement une fois

    public TetrisGame(int screenWidth, int screenHeight)
    {
        grid = new Grid(12, 20, 20);
        currentPiece = Tetromino.RandomPiece(grid.Width);
        nextPiece = Tetromino.RandomPiece(grid.Width); // Initialiser la pièce suivante
        heldPiece = null; // Aucune pièce gardée au début
        gameOver = false;
        gravityCounter = 0;
        gravitySpeed = 50;
    }

    public void Update()
    {
        Raylib.DrawText($"Score {grid.score}", 300, 150, 20, Color.VIOLET);

        if (!gameOver)
        {
            HandleInput();

            gravityCounter++;

            if (gravityCounter >= gravitySpeed)
            {
                grid.SetActivePiece(currentPiece);
                if (!grid.MovePiece(currentPiece, 0, 1))
                {
                    grid.FreezePiece(currentPiece);
                    grid.CheckLines();
                    currentPiece = nextPiece; // La pièce suivante devient la pièce actuelle
                    nextPiece = Tetromino.RandomPiece(grid.Width); // Générer une nouvelle pièce suivante

                    if (grid.CheckCollision(currentPiece))
                    {
                        gameOver = true;
                    }
                }
                gravityCounter = gravityAdd;
            }

            // Accélérer la gravité au fur et à mesure du jeu
            if (grid.scoreGravityCheck >= 1) {
                grid.scoreGravityCheck -= 1;
                if (gravityAdd < 46) {
                    gravityAdd += 2;
                }
                else if (gravityAdd == 46) {
                    gravityAdd += 2;
                }
            }
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
        {
            ResetGame();
        }
    }

    public void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RAYWHITE);

        // Dessiner la grille et la pièce active
        grid.Draw();

        // Dessiner la prochaine pièce sur le côté droit de l'écran
        DrawNextPiece();

        // Dessiner la pièce gardée à gauche de l'écran
        DrawHeldPiece();

        if (gameOver)
        {
            DrawGameOverMessage();
        }

        Raylib.EndDrawing();
    }

    private void HandleInput()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT)) grid.MovePiece(currentPiece, -1, 0);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT)) grid.MovePiece(currentPiece, 1, 0);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN)) grid.MovePiece(currentPiece, 0, 1);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP)) grid.MovePiece(currentPiece, 0, 0, true);

        // Permettre de garder une pièce en appuyant sur la touche 'C' (pour "Hold")
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_C) && canHold)
        {
            HoldPiece();
        }
    }

    private void HoldPiece()
    {
        if (heldPiece == null)
        {
            heldPiece = currentPiece; // Garder la pièce courante
            currentPiece = nextPiece; // La prochaine pièce devient la courante
            nextPiece = Tetromino.RandomPiece(grid.Width); // Générer une nouvelle pièce suivante
        }
        else
        {
            // Échanger la pièce courante avec la pièce gardée
            currentPiece = heldPiece;
            heldPiece = null;
        }

        // Réinitialiser la position de la pièce gardée en haut de la grille
        currentPiece.PositionY = 0;
        currentPiece.PositionX = grid.Width / 2 - 2; // Centrer la pièce gardée
    }

    private void DrawGameOverMessage()
    {
        string message = "Game Over! Press R to restart";
        Raylib.DrawText(message, grid.Width * grid.SquareSize / 2 - Raylib.MeasureText(message, 20) / 2, grid.Height * grid.SquareSize / 2, 20, Color.RED);
    }

    private void ResetGame()
    {
        grid = new Grid(12, 20, 20);
        gravityAdd = 10;
        currentPiece = Tetromino.RandomPiece(grid.Width);
        nextPiece = Tetromino.RandomPiece(grid.Width);
        heldPiece = null; // Réinitialiser la pièce gardée
        gameOver = false;
        gravityCounter = 0;
        canHold = true; // Permettre de garder une nouvelle pièce
    }

    // Méthode pour dessiner la prochaine pièce à droite de l'écran
    private void DrawNextPiece()
    {
        string nextPieceText = "Next Piece:";
        int nextPieceX = 300;
        int nextPieceY = 50;

        Raylib.DrawText(nextPieceText, nextPieceX-20, nextPieceY, 20, Color.DARKGRAY);

        foreach (var (x, y, color) in nextPiece.ColoredCells())
        {
            Raylib.DrawRectangle(nextPieceX + (x * grid.SquareSize), nextPieceY + (y * grid.SquareSize), grid.SquareSize, grid.SquareSize, color);
        }
    }

    // Méthode pour dessiner la pièce gardée à gauche de l'écran
    private void DrawHeldPiece()
    {
        if (heldPiece != null)
        {
            string heldPieceText = "Held Piece:";
            int heldPieceX = 300; // Position à gauche de l'écran
            int heldPieceY = 250;

            Raylib.DrawText(heldPieceText, heldPieceX, heldPieceY, 20, Color.DARKGRAY);

            foreach (var (x, y, color) in heldPiece.ColoredCells())
            {
                Raylib.DrawRectangle(heldPieceX + (x * grid.SquareSize), heldPieceY + (y * grid.SquareSize), grid.SquareSize, grid.SquareSize, color);
            }
        }
    }
}
