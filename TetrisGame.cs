using Raylib_cs;
using System;

class TetrisGame
{
    private Grid grid;
    private Tetromino currentPiece;
    private bool gameOver;
    private int gravityCounter;
    private int gravitySpeed;

    private int gravityAdd = 0;

    public TetrisGame(int screenWidth, int screenHeight)
    {
        grid = new Grid(12, 20, 20);
        currentPiece = Tetromino.CreateRandom(grid.Width / 2 - 2, 0);
        gameOver = false;
        gravityCounter = 0;
        gravitySpeed = 50;
    }

    public void Update()
    {
        Raylib.DrawText($"Score {grid.score}", 400, 100, 20, Color.VIOLET);
        if (!gameOver)
        {
            HandleInput();

            gravityCounter++;
            if (gravityCounter >= gravitySpeed)
            {
                if (!grid.MovePiece(currentPiece, 0, 1))
                {
                    grid.FreezePiece(currentPiece);
                    grid.CheckLines();
                    currentPiece = Tetromino.CreateRandom(grid.Width / 2 - 2, 0);

                    if (grid.CheckCollision(currentPiece))
                    {
                        gameOver = true;
                    }

                }
                gravityCounter = gravityAdd;
                Console.WriteLine(gravityCounter);
            }
            if (grid.scoreGravityCheck == 2) {
                grid.scoreGravityCheck = 0;
                gravityAdd+= 15;
                Console.WriteLine(grid.scoreGravityCheck);
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

        grid.Draw();

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
    }

    private void DrawGameOverMessage()
    {
        string message = "Game Over! Press R to restart";
        Raylib.DrawText(message, grid.Width * grid.SquareSize / 2 - Raylib.MeasureText(message, 20) / 2, grid.Height * grid.SquareSize / 2, 20, Color.RED);
    }

    private void ResetGame()
    {
        grid = new Grid(12, 20, 20);
        currentPiece = Tetromino.CreateRandom(grid.Width / 2 - 2, 0);
        gameOver = false;
        gravityCounter = 0;
    }
}
