using System;
using Raylib_cs;
using System.Threading.Tasks;

namespace Tetris
{
    public class Pieces
    {
        private int posX;
        private int posY;
        private float timer; // Timer pour gérer la descente des pièces
        private const float moveInterval = 0.5f; // Intervalle en secondes entre chaque mouvement

        public Pieces(int posXu,int posYu)
        {
            this.posX = posXu;
            this.posY = posYu;
            // Position initiale de la pièce

            timer = 0;
        }

        public void Update(float deltaTime)
        {
            // Mettre à jour le timer
            timer += deltaTime;

            // Faire descendre la pièce toutes les "moveInterval" secondes
            if (timer >= moveInterval)
            {
                posY += 20; // Déplacer la pièce vers le bas
                timer = 0; // Réinitialiser le timer
            }
        }

        public void DrawSquarePiece()
        {
            // Dessiner la pièce carrée
            Raylib.DrawRectangle(posX, posY, 40, 40, Color.Yellow);
            Raylib.DrawLine(posX + 20, posY, posX + 20, posY + 40, Color.Brown);
            Raylib.DrawLine(posX, posY + 20, posX + 40, posY + 20, Color.Brown);
        }
    }
}