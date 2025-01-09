using Raylib_cs;
using System;

class Tetris
{
    const int ScreenWidth = 800;
    const int ScreenHeight = 450;
    const int GridHorizontalSize = 12;
    const int GridVerticalSize = 20;
    const int SquareSize = 20;

    static GridSquare[,] grid = new GridSquare[GridHorizontalSize, GridVerticalSize];
    static GridSquare[,] piece = new GridSquare[4, 4];
    static bool gameOver = false;
    static int piecePositionX = 0;
    static int piecePositionY = 0;
    static int gravityCounter = 0;
    static int gravitySpeed = 30;

    enum GridSquare { Empty, Moving, Full };

    static void Main()
    {
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Tetris with Raylib");
        Raylib.SetTargetFPS(60);

        InitGame();

        while (!Raylib.WindowShouldClose())
        {
            if (!gameOver)
            {
                UpdateGame();
                DrawGame();
            }
            else
            {
                DrawGameOver();
            }
        }

        Raylib.CloseWindow();
    }

    static void InitGame()
    {
        for (int x = 0; x < GridHorizontalSize; x++)
        {
            for (int y = 0; y < GridVerticalSize; y++)
            {
                grid[x, y] = (x == 0 || x == GridHorizontalSize - 1 || y == GridVerticalSize - 1) ? GridSquare.Full : GridSquare.Empty;
            }
        }

        piecePositionX = GridHorizontalSize / 2 - 2;
        piecePositionY = 0;

        GeneratePiece();
    }

    static void GeneratePiece()
    {
        Array.Clear(piece, 0, piece.Length);
        piece[1, 0] = GridSquare.Moving;
        piece[1, 1] = GridSquare.Moving;
        piece[2, 1] = GridSquare.Moving;
        piece[2, 0] = GridSquare.Moving; // Simple square piece
    }

    static void UpdateGame()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_LEFT)) MovePiece(-1, 0);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_RIGHT)) MovePiece(1, 0);

        gravityCounter++;
        if (gravityCounter >= gravitySpeed)
        {
            MovePiece(0, 1);
            gravityCounter = 0;
        }

        if (CheckCollision())
        {
            FreezePiece();
            CheckLines();
            GeneratePiece();

            if (CheckCollision()) gameOver = true; // New piece overlaps
        }
    }

    static void MovePiece(int dx, int dy)
    {
        ClearPieceFromGrid();
        piecePositionX += dx;
        piecePositionY += dy;

        if (CheckCollision())
        {
            piecePositionX -= dx;
            piecePositionY -= dy;
        }

        PlacePieceOnGrid();
    }

    static void PlacePieceOnGrid()
    {
        for (int px = 0; px < 4; px++)
        {
            for (int py = 0; py < 4; py++)
            {
                if (piece[px, py] == GridSquare.Moving)
                {
                    int gx = piecePositionX + px;
                    int gy = piecePositionY + py;
                    grid[gx, gy] = GridSquare.Moving;
                }
            }
        }
    }

    static void ClearPieceFromGrid()
    {
        for (int x = 0; x < GridHorizontalSize; x++)
        {
            for (int y = 0; y < GridVerticalSize; y++)
            {
                if (grid[x, y] == GridSquare.Moving) grid[x, y] = GridSquare.Empty;
            }
        }
    }

    static bool CheckCollision()
    {
        for (int px = 0; px < 4; px++)
        {
            for (int py = 0; py < 4; py++)
            {
                if (piece[px, py] == GridSquare.Moving)
                {
                    int gx = piecePositionX + px;
                    int gy = piecePositionY + py;

                    if (gx < 0 || gx >= GridHorizontalSize || gy >= GridVerticalSize || grid[gx, gy] == GridSquare.Full)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    static void FreezePiece()
    {
        for (int x = 0; x < GridHorizontalSize; x++)
        {
            for (int y = 0; y < GridVerticalSize; y++)
            {
                if (grid[x, y] == GridSquare.Moving) grid[x, y] = GridSquare.Full;
            }
        }
    }

    static void CheckLines()
    {
        for (int y = 0; y < GridVerticalSize - 1; y++)
        {
            bool lineFull = true;

            for (int x = 1; x < GridHorizontalSize - 1; x++)
            {
                if (grid[x, y] != GridSquare.Full) lineFull = false;
            }

            if (lineFull)
            {
                for (int dy = y; dy > 0; dy--)
                {
                    for (int x = 1; x < GridHorizontalSize - 1; x++)
                    {
                        grid[x, dy] = grid[x, dy - 1];
                    }
                }
            }
        }
    }

    static void DrawGame()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RAYWHITE);

        for (int x = 0; x < GridHorizontalSize; x++)
        {
            for (int y = 0; y < GridVerticalSize; y++)
            {
                Color color = Color.LIGHTGRAY;
                if (grid[x, y] == GridSquare.Full) color = Color.DARKGRAY;
                else if (grid[x, y] == GridSquare.Moving) color = Color.GRAY;

                Raylib.DrawRectangle(x * SquareSize, y * SquareSize, SquareSize - 1, SquareSize - 1, color);
            }
        }

        Raylib.EndDrawing();
    }

    static void DrawGameOver()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.RAYWHITE);

        string message = "Game Over! Press R to restart";
        Raylib.DrawText(message, ScreenWidth / 2 - Raylib.MeasureText(message, 20) / 2, ScreenHeight / 2, 20, Color.RED);

        if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
        {
            gameOver = false;
            InitGame();
        }

        Raylib.EndDrawing();
    }
}
