using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Raylib_cs;

class Grid : Drawable
{
    public int scoreGravityCheck = 0;
    public int score = 0;
    public int Width { get; }
    public int Height { get; }
    public int SquareSize { get; }
    private GridSquare[,] grid;

    private Tetromino activePiece;

    public void SetActivePiece(Tetromino piece)
    {
        activePiece = piece;
    }

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

    public bool CanPlacePiece(Tetromino piece)
    {
    foreach (var (x, y) in piece.OccupiedCells())
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height || grid[x, y] == GridSquare.Full)
        {
            return false;
        }
    }
    return true;
    }

    public bool MovePiece(Tetromino piece, int dx, int dy, bool rotate = false)
    {
        piece.ClearFromGrid(grid);

        if (rotate)
        {
            piece.Rotate(grid);
            if (!CanPlacePiece(piece))
            {
                piece.Rotate(grid);
                piece.Rotate(grid);
                piece.Rotate(grid);
            }
        }
        else
        {
            piece.PositionX += dx;
            piece.PositionY += dy;

            if (CheckCollision(piece))
            {
                piece.PositionX -= dx;
                piece.PositionY -= dy;
                piece.PlaceOnGrid(grid);
                return false;
            }
        }

        piece.PlaceOnGrid(grid);
        return true;
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
                score++;
                scoreGravityCheck++;
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

                if (grid[x, y] == GridSquare.Full)
                {
                    color = Color.DARKGRAY;
                }
                else if (grid[x, y] == GridSquare.Moving && activePiece != null)
                {
                    // Vérifie si la pièce active occupe cette case
                    foreach (var (pieceX, pieceY) in activePiece.OccupiedCells())
                    {
                        if (pieceX == x && pieceY == y)
                        {
                            color = activePiece.Color;
                            break;
                        }
                    }
                }

                Raylib.DrawRectangle(x * SquareSize, y * SquareSize, SquareSize - 1, SquareSize - 1, color);
            }
        }
    }

}
