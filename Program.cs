using Raylib_cs;

class Program
{
    const int ScreenWidth = 800;
    const int ScreenHeight = 450;

    static void Main()
    {
        Raylib.InitWindow(ScreenWidth, ScreenHeight, "Tetris Main Menu");
        Raylib.SetTargetFPS(60);

        MainMenu menu = new MainMenu(ScreenWidth, ScreenHeight);
        TetrisGame game = null;

        while (!Raylib.WindowShouldClose())
        {
            if (game == null && menu.Update())
            {
                game = new TetrisGame(ScreenWidth, ScreenHeight);
            }

            if (game != null)
            {
                game.Update();
                game.Draw();
            }
            else
            {
                menu.Draw();
            }
        }

        Raylib.CloseWindow();
    }
}
