using System.CodeDom;
using System;
using System.Collections.Generic;
using Raylib_cs;

class Tetromino
{
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public GridSquare[,] Shape { get; private set; }
    public Color Color { get; private set; }
    public static Color CYAN = new Color(0, 255, 255, 255);

    public Tetromino(int x, int y, GridSquare[,] shape, Color color)
    {
        PositionX = x;
        PositionY = y;
        Shape = shape;
        Color = color;
    }

    public static Tetromino Square(int x, int y)
    {
        GridSquare[,] squareShape = new GridSquare[4, 4];
        squareShape[1, 0] = GridSquare.Moving;
        squareShape[1, 1] = GridSquare.Moving;
        squareShape[2, 1] = GridSquare.Moving;
        squareShape[2, 0] = GridSquare.Moving;
        return new Tetromino(x, y, squareShape, Color.YELLOW);
    }

    public static Tetromino L(int x, int y)
    {
        GridSquare[,] lShape = new GridSquare[4, 4];
        lShape[1, 0] = GridSquare.Moving;
        lShape[2, 0] = GridSquare.Moving;
        lShape[3, 0] = GridSquare.Moving;
        lShape[1, 1] = GridSquare.Moving;
        return new Tetromino(x, y, lShape, Color.ORANGE);
    }

    public static Tetromino lReverse(int x, int y)
    {
        GridSquare[,] lReverseShape = new GridSquare[4, 4];
        lReverseShape[1, 0] = GridSquare.Moving;
        lReverseShape[2, 0] = GridSquare.Moving;
        lReverseShape[3, 0] = GridSquare.Moving;
        lReverseShape[3, 1] = GridSquare.Moving;
        return new Tetromino(x, y, lReverseShape, Color.BLUE);
    }

    public static Tetromino S(int x, int y) {
        GridSquare[,] sShape = new GridSquare[4, 4];
        sShape[1, 1] = GridSquare.Moving;
        sShape[2, 1] = GridSquare.Moving;
        sShape[2, 0] = GridSquare.Moving;
        sShape[3, 0] = GridSquare.Moving;

        return new Tetromino(x, y, sShape, Color.GREEN);
    }

    public static Tetromino sReverse(int x, int y) {
        GridSquare[,] sReverseShape = new GridSquare[4, 4];
        sReverseShape[1, 0] = GridSquare.Moving;
        sReverseShape[2, 0] = GridSquare.Moving;
        sReverseShape[2, 1] = GridSquare.Moving;
        sReverseShape[3, 1] = GridSquare.Moving;

        return new Tetromino(x, y, sReverseShape, Color.RED);
    }

    public static Tetromino T(int x, int y) {
        GridSquare[,] tShape = new GridSquare[4, 4];
        tShape[1, 1] = GridSquare.Moving;
        tShape[2, 1] = GridSquare.Moving;
        tShape[2, 0] = GridSquare.Moving;
        tShape[3, 1] = GridSquare.Moving;

        return new Tetromino(x, y, tShape, Color.PURPLE);
    }

    public static Tetromino I(int x, int y)
    {
        GridSquare[,] iShape = new GridSquare[4, 4];
        iShape[1, 0] = GridSquare.Moving;
        iShape[1, 1] = GridSquare.Moving;
        iShape[1, 2] = GridSquare.Moving;
        iShape[1, 3] = GridSquare.Moving;

        return new Tetromino(x, y, iShape, CYAN);
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
        for (int px = 0; px < Shape.GetLength(0); px++)
        {
            for (int py = 0; py < Shape.GetLength(1); py++)
            {
                if (Shape[px, py] == GridSquare.Moving)
                {
                    yield return (PositionX + px, PositionY + py);
                }
            }
        }
    }

    public static Tetromino RandomPiece(int gridWidth) {
    Random random = new Random();
    int number = random.Next(0, 7); // De 0 à 6 pour inclure toutes les pièces
    int startX = gridWidth / 2 - 2;

    return number switch {
        0 => Square(startX, 0),
        1 => L(startX, 0),
        2 => lReverse(startX, 0),
        3 => S(startX, 0),
        4 => sReverse(startX, 0),
        5 => T(startX, 0),
        6 => I(startX, 0),
        _ => Square(startX, 0), // Par défaut
    };
}

public void Rotate(GridSquare[,] grid)
{
    var originalShape = (GridSquare[,])Shape.Clone();
    GridSquare[,] rotatedShape = new GridSquare[4, 4];

    for (int x = 0; x < 4; x++)
    {
        for (int y = 0; y < 4; y++)
        {
            rotatedShape[y, 3 - x] = Shape[x, y];
        }
    }

    Shape = rotatedShape;

    if (grid != null && IsCollision(grid))
    {
        Shape = originalShape;
    }
}

private bool IsCollision(GridSquare[,] grid)
{
    foreach (var (x, y) in OccupiedCells())
    {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1) || grid[x, y] == GridSquare.Full)
        {
            return true;
        }
    }
    return false;
}

public IEnumerable<(int, int, Color)> ColoredCells()
{
    for (int px = 0; px < 4; px++)
    {
        for (int py = 0; py < 4; py++)
        {
            if (Shape[px, py] == GridSquare.Moving)
            {
                yield return (PositionX + px, PositionY + py, Color);
            }
        }
    }
}


}
