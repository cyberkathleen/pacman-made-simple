/******************************************************************************
** PROGRAMME  PacManSimplifie.cs                                             **                                                                                                                      
**                                                                           **                                                                                 
** Lieu      : ETML - section informatique                                   **                                                            
** Auteur    : ABDULKADIR Salma, ABID Fatima, LU Kathleen                    **
** Date      : 01.05.2023                                                    **
**                                                                           **
** Modifications                                                             **
**   Auteur  : LU Kathleen                                                   **
**   Version : 1.6                                                           **
**   Date    : 01.01.2024                                                    **
**   Raisons : Fusion des 2 méthodes CheckCollisionPoint() et EatPoint() en  **
**             une seule. Amélioration : point mangé bout par bout et non en **
**             entier en une seule fois.                                     **
**             Amélioration de l'effacement (méthode EraseAndRedraw()) et de **
**             l'affichage de Pac-Man (méthode Print()) afin de pouvoir      **
**             afficher les parties de point non-mangées.                    **
**                                                                           **
** Modifications                                                             **
**   Auteur  : LU Kathleen                                                   **
**   Version : 1.5                                                           **
**   Date    : 31.12.2023                                                    **
**   Raisons : Affiche un nouveau niveau après avoir mangé tous les points   **
**                                                                           **
** Modifications                                                             **
**   Auteur  : LU Kathleen                                                   **
**   Version : 1.4                                                           **
**   Date    : 28.12.2023                                                    **
**   Raisons : Apparition du game over après 3 échecs au lieu d'un           **
**                                                                           **                                                                                       
** Modifications                                                             **                                                                             
** Auteur  : ABDULKADIR Salma, ABID Fatima, LU Kathleen                      **
** Version : 1.2                                                             **
** Date    : 28.05.2023                                                      **
** Raisons : Modification demandée par l'utilisateur                         **
**                                                                           **
**                                                                           **
******************************************************************************/

/******************************************************************************
** DESCRIPTION                                                               **
** Programme du jeu Pac-Man en version simplifié dans lequel PacMan se       **
** déplace, mange des pièces, et gagne la partie lorsque toutes les pièces   **
** sont mangées. Lorsque Pac-Man rencontre un fantôme il perd une de ses 3   **
** vies. Lorsqu'il gagne il passe au niveau suivant où les fantômes se       ** 
** déplacent plus vite.                                                      **
******************************************************************************/

using System;
using System.Collections.Generic;

namespace PacManSimplifie
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ///////////////////////////////////////////////////////////////////// CONSTANTES /////////////////////////////////////////////////////////////////////
            const byte MOUTH_HALF_OPENED = 1;       // Pour indiquer que le Pac-Man doit commencer avec la bouche à moitié ouverte
            const string GAME_TITLE = "Pac-Man";    // Titre du jeu
            const byte POINT_INITIAL_POSITION = 3;  // Position de départ des points sur le plateau
            ///////////////////////////////////////////////////////////////////// VARIABLES /////////////////////////////////////////////////////////////////////
            bool boolNoWall = true;                                         // Est-ce qu'il y a un mur devant le Pac-Man
            bool boolIsCollision = false;                                   // Est-ce qu'il y a une collision avec un fantôme
            bool boolIsStart = false;
            byte bytPacManInitialRowPosition = 29;                          // Ligne de départ de Pac-Man
            byte bytPacManInitialColPosition = 22;                          // Colonne de départ de Pac-Man
            byte bytPacManMouthInitialPosition = 0;                         // Position de la bouche de Pac-Man au lancement du jeu
            ///////////////////////////////////////////////////////////////////// PROGRAMME /////////////////////////////////////////////////////////////////////

            // Donne un titre au programme dans son onglet
            Console.Title = GAME_TITLE;

            // Crée le plateau de jeu
            Map boardMap = new Map();

            // Crée les points
            Points myPoints = new Points(boardMap.chrMap);

            // Crée la logique de jeu
            Game game = new Game();
            
            // Affiche l'introduction du jeu
            game.StartGame();
            boolIsStart = true;

            // Permet au joueur de jouer une partie et passer au niveau suivant tant qu'il lui reste des vies
            do
            {
                // Place les points sur le plateau
                myPoints.DistributePointInTheTab(POINT_INITIAL_POSITION);

                // Initialise le nombre de points restants à 54
                myPoints.BytRemaining = 54;

                // Affiche le plateau
                Console.SetCursorPosition(0, 0);
                boardMap.DisplayMap();

                // Crée, affiche et positionne Pac-Man
                Position pacmanStart = new Position(bytPacManInitialRowPosition, bytPacManInitialColPosition);
                Pacman pacman = new Pacman(pacmanStart, bytPacManMouthInitialPosition, MOUTH_HALF_OPENED);

                // Création de la liste des positions initiales des fantômes avec les positions initiales
                List<Position> lis_posGhostsPositions = new List<Position>();
                lis_posGhostsPositions.Add(new Position(1, 1));
                lis_posGhostsPositions.Add(new Position(57, 1));
                lis_posGhostsPositions.Add(new Position((58 / 2), 57));

                // Création de la liste des couleurs des fantômes avec les couleurs
                List<string> colors = new List<string>();
                colors.Add("red");
                colors.Add("yellow");
                colors.Add("green");

                // Création de la liste des directions des fantômes avec les directions de départ
                List<string> ghostsDirections = new List<string>();
                ghostsDirections.Add("right");
                ghostsDirections.Add("down");
                ghostsDirections.Add("left");

                // Création de la liste des fantômes
                List<Ghost> ghosts = new List<Ghost>();

                // Création et ajout des fantômes à la liste des fantômes
                for (int i = 0; i < lis_posGhostsPositions.Count; i++)
                {
                    ghosts.Add(new Ghost(lis_posGhostsPositions[i], colors[i], ghostsDirections[i], boardMap, myPoints, pacman));
                }

                // Affichage des fantômes
                foreach (Ghost cursorGhost in ghosts)
                {
                    cursorGhost.DisplayGhost();
                }

                // Affiche le score
                game.DisplayScore(myPoints.BytScore);

                // Affiche le nombre de vies du joueur
                game.DisplayPacManLives();

                // Mange le premier point derrière la position initiale de Pac-Man
                pacman.EatPoint(myPoints, boardMap);

                // Permet au Pac-Man de se déplacer tant qu'il n'entre pas en collision avec un fantôme
                do
                {
                    // Enregistre la touche pressée par l'utilisateur
                    Console.SetCursorPosition(2, 66);
                    ConsoleKey pressedKey = Console.ReadKey().Key;

                    // Pour pas que la touche pressée lors de l'introduction au jeu soit considérée comme premier mouvement du Pac-Man
                    if (boolIsStart)
                    {
                        pressedKey = Console.ReadKey().Key;
                        boolIsStart = false;
                    }

                    // Vérifie s'il y a un mur là où le Pac-Man veut aller
                    boolNoWall = pacman.CheckCollisionWall(boardMap, pressedKey);

                    // Fait ouvrir/fermer la bouche du Pac-Man d'un cran
                    pacman.ChangeMouth();

                    // Met à jour la direction dans lequel le Pac-Man doit aller en fonction de la touche pressée
                    pacman.ChangeDirection(pressedKey);

                    // S'il n'y a pas de mur, déplace le Pac-Man et compte les points mangés, sinon fait juste bouger sa bouche
                    if (boolNoWall)
                    {
                        // Déplace le Pac-Man
                        pacman.Move(myPoints);

                        // Vérifie que lorsque Pac-Man rencontre un point, il le mange
                        pacman.EatPoint(myPoints, boardMap);

                        // Affiche le score
                        game.DisplayScore(myPoints.BytScore);
                    }
                    else
                    {
                        // Efface le Pac-Man et affiche les points pas encore mangés
                        myPoints.EraseAndRedraw(pacman.PosPosition, pacman.LENGTH);

                        // Affiche le Pac-Man avec la bonne ouverture de bouche
                        pacman.Print();
                    }// End if (boolNoWall)


                    // Efface les fantômes et réaffiche les points pas encore mangés
                    for (byte i = 0; i < ghosts.Count; i++)
                    {
                        myPoints.EraseAndRedraw(lis_posGhostsPositions[i], ghosts[i].LENGTH);
                    }

                    // Déplace les fantômes et met à jour leur position dans la liste des positions des fantômes
                    for (byte i = 0; i < ghosts.Count; i++)
                    {
                        ghosts[i].MoveGhost();
                        lis_posGhostsPositions[i] = ghosts[i].Position;
                    }

                    // Vérifie s'il y a une collision avec un fantôme
                    boolIsCollision = game.CheckCollisionGhost(pacman.PosPosition, lis_posGhostsPositions);

                    // Vérifie si le joueur a mangé tous les points sans mourir et qu'il faut recommencer un niveau
                    if (myPoints.BytRemaining == 0 && !boolIsCollision)
                    {
                        break;
                    }
                } while (!boolIsCollision);

                // Le joueur perd une vie s'il y a eu une collision avec un fantôme
                if (boolIsCollision)
                {
                    game.BytNbLives -= 1;
                }
            } while (game.BytNbLives > 0);

            // Affiche le nombre de vies du joueur
            game.DisplayPacManLives();

            // Affiche le Game Over
            game.DisplayGameOver();

            Console.ReadLine();
        }
    }
}
