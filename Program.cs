using Raylib_cs;
using System;
using System.Collections.Generic;

class Program
{
    const int ScreenWidth = 800;
    const int ScreenHeight = 450;

    static void Main()
    {
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Tetris with Raylib");
        Raylib.SetTargetFPS(60);

        TetrisGame game = new TetrisGame(ScreenWidth, ScreenHeight);

        while (!Raylib.WindowShouldClose())
        {
            game.Update();
            game.Draw();
        }

        Raylib.CloseWindow();
    }
}

abstract class Drawable
{
    public abstract void Draw();
}

class TetrisGame
{
    private Grid grid;
    private Tetromino currentPiece;
    private bool gameOver;
    private int gravityCounter;
    private int gravitySpeed;

    public TetrisGame(int screenWidth, int screenHeight)
    {
        grid = new Grid(12, 20, 20);
        currentPiece = Tetromino.CreateRandom(grid.Width / 2 - 2, 0);
        gameOver = false;
        gravityCounter = 0;
        gravitySpeed = 30;
    }

    public void Update()
    {
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

                gravityCounter = 0;
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

class Grid : Drawable
{
    public int Width { get; }
    public int Height { get; }
    public int SquareSize { get; }
    private GridSquare[,] grid;

    public Grid(int width, int height, int squareSize)
    {
        Width = width;
        Height = height;
        SquareSize = squareSize;
        grid = new GridSquare[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                grid[x, y] = (x == 0 || x == Width - 1 || y == Height - 1) ? GridSquare.Full : GridSquare.Empty;
            }
        }
    }

    public bool MovePiece(Tetromino piece, int dx, int dy)
    {
        piece.ClearFromGrid(grid);
        piece.PositionX += dx;
        piece.PositionY += dy;

        if (CheckCollision(piece))
        {
            piece.PositionX -= dx;
            piece.PositionY -= dy;
            piece.PlaceOnGrid(grid);
            return false;
        }

        piece.PlaceOnGrid(grid);
        return true;
    }

    public void FreezePiece(Tetromino piece)
    {
        piece.FreezeOnGrid(grid);
    }

    public bool CheckCollision(Tetromino piece)
    {
        foreach (var (x, y) in piece.OccupiedCells())
        {
            if (x < 0 || x >= Width || y >= Height || grid[x, y] == GridSquare.Full)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckLines()
    {
        for (int y = 0; y < Height - 1; y++)
        {
            bool lineFull = true;

            for (int x = 1; x < Width - 1; x++)
            {
                if (grid[x, y] != GridSquare.Full) lineFull = false;
            }

            if (lineFull)
            {
                for (int dy = y; dy > 0; dy--)
                {
                    for (int x = 1; x < Width - 1; x++)
                    {
                        grid[x, dy] = grid[x, dy - 1];
                    }
                }
            }
        }
    }

    public override void Draw()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Color color = Color.LIGHTGRAY;
                if (grid[x, y] == GridSquare.Full) color = Color.DARKGRAY;
                else if (grid[x, y] == GridSquare.Moving) color = Color.GRAY;

                Raylib.DrawRectangle(x * SquareSize, y * SquareSize, SquareSize - 1, SquareSize - 1, color);
            }
        }
    }
}

enum GridSquare { Empty, Moving, Full }

class Tetromino
{
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    private GridSquare[,] shape;

    public Tetromino(int x, int y, GridSquare[,] shape)
    {
        PositionX = x;
        PositionY = y;
        this.shape = shape;
    }

    public static Tetromino CreateRandom(int x, int y)
    {
        GridSquare[,] squareShape = new GridSquare[4, 4];
        squareShape[1, 0] = GridSquare.Moving;
        squareShape[1, 1] = GridSquare.Moving;
        squareShape[2, 1] = GridSquare.Moving;
        squareShape[2, 0] = GridSquare.Moving; // Simple square piece

        return new Tetromino(x, y, squareShape);
    }

    public void PlaceOnGrid(GridSquare[,] grid)
    {
        foreach (var (x, y) in OccupiedCells())
        {
            grid[x, y] = GridSquare.Moving;
        }
    }

    public void ClearFromGrid(GridSquare[,] grid)
    {
        foreach (var (x, y) in OccupiedCells())
        {
            grid[x, y] = GridSquare.Empty;
        }
    }

    public void FreezeOnGrid(GridSquare[,] grid)
    {
        foreach (var (x, y) in OccupiedCells())
        {
            grid[x, y] = GridSquare.Full;
        }
    }

    public IEnumerable<(int, int)> OccupiedCells()
    {
        for (int px = 0; px < 4; px++)
        {
            for (int py = 0; py < 4; py++)
            {
                if (shape[px, py] == GridSquare.Moving)
                {
                    yield return (PositionX + px, PositionY + py);
                }
            }
        }
    }
}
