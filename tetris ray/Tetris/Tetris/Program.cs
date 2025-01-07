using Raylib_cs;

namespace Tetris
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialisation de Raylib
            Raylib.InitWindow(800, 600, "Tetris");
            Raylib.SetTargetFPS(60);

            // Création d'une pièce
            Pieces firstSquare = new Pieces(Raylib.GetRenderWidth() / 2 - 100, Raylib.GetRenderHeight() / 2 - 200);
            Pieces secondSquare = new Pieces(Raylib.GetRenderWidth() / 2 - 60, Raylib.GetRenderHeight() / 2 - 120);
            Pieces thirdSquare = new Pieces(Raylib.GetRenderWidth() / 2 - 20, Raylib.GetRenderHeight() / 2 - 160);
            //Pieces firstSquare = new Pieces(Raylib.GetRenderWidth() / 2 - 100, Raylib.GetRenderHeight() / 2 - 200);
            Grid grid= new Grid();

            while (!Raylib.WindowShouldClose())
            {
                // Calcul du temps écoulé entre chaque frame
                float deltaTime = Raylib.GetFrameTime();

                // Mise à jour de la pièce
                firstSquare.Update(deltaTime);
                secondSquare.Update(deltaTime);
                thirdSquare.Update(deltaTime);

                // Rendu
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);

                // Dessiner la pièce

                grid.GridDraw();
                firstSquare.DrawSquarePiece();
                secondSquare.DrawSquarePiece();
                thirdSquare.DrawSquarePiece();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
