using System;
using Raylib_cs;

namespace Tetris
{
    class Grid
    {

        int posX;
        int posY;

        public void GridDraw()
        {
            posX = (Raylib.GetRenderWidth()/2) - 100;
            posY = (Raylib.GetRenderHeight() / 2) - 200;
            //Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            Raylib.DrawRectangle(posX, posY, 200, 400, Color.Blue);

            for (int i = posX+20; i < posX + 200;)
            {
                Raylib.DrawLine(i, posY, i, posY+400, Color.Black);
                i += 20;
            }

            for (int j = posY+20; j < posY + 400;)
            {
                Raylib.DrawLine(posX, j, posX+200, j, Color.Black);
                j += 20;
            }

            //Raylib.EndDrawing();
        }
    }
}