using System;
using Raylib_cs;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(800, 480, "Tetris");

            Grid grid = new Grid(); // Création d'une instance de Grid

            while (!Raylib.WindowShouldClose())
            {
                grid.GridDraw(); // Appel via l'instance
            }
            Raylib.CloseWindow();
        }
    }
}
