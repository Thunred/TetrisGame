using Raylib_cs;

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
