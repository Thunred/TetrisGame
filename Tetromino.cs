using System.CodeDom;
using System.Collections.Generic;
using Raylib_cs;

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
