using Raylib_cs;

class MainMenu
{
    private int screenWidth;
    private int screenHeight;
    private bool isGameStarted;

    public MainMenu(int screenWidth, int screenHeight)
    {
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        isGameStarted = false;
    }

    public bool Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
        {
            isGameStarted = true;
        }

        return isGameStarted;
    }

    public void Draw()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.BLACK);

        string title = "TETRIS";
        string instructions = "Press ENTER to Play";

        Raylib.DrawText(title, screenWidth / 2 - Raylib.MeasureText(title, 50) / 2, screenHeight / 2 - 100, 50, Color.YELLOW);
        Raylib.DrawText(instructions, screenWidth / 2 - Raylib.MeasureText(instructions, 20) / 2, screenHeight / 2, 20, Color.WHITE);

        Raylib.EndDrawing();
    }
}
