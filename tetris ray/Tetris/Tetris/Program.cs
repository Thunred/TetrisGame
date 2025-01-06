using System;
using Raylib_cs;

namespace Tetris
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
            Raylib.InitWindow(800, 480, "Hello World");

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);

                Raylib.DrawRectangle(275, 200, 100, 200, Color.Blue);

                for (int i = 285; i < 375;) {
                    Raylib.DrawLine(i, 200, i, 400, Color.Black);
                    i += 10;
                }
                for (int j = 210; j < 400;)
                {
                    Raylib.DrawLine(275, j, 375, j, Color.Black);
                    j += 10;
                }
                

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
