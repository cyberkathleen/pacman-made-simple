/******************************************************************************
** PROGRAMME  PacManSimplifie.cs                                             **
**                                                                           **
** Lieu      : ETML - section informatique                                   **                                
** Auteur    : ABDULKADIR Salma, ABID Fatima, LU Kathleen                    **
** Date      : 27.05.2023                                                    **
**                                                                           **
** Modifications                                                             **
**   Auteur  : LU Kathleen                                                   **
**   Version : 1.4                                                           **
**   Date    : 28.12.2023                                                    **
**   Raisons : Optimisation de la méthode DisplayPacManLives()               **
**                                                                           **
** Modifications                                                             **
**   Auteur  : ABDULKADIR Salma, ABID Fatima, LU Kathleen                    **
**   Version : 1.2                                                           **
**   Date    : 28.05.2023                                                    **
**   Raisons : Modification demandée par l'utilisateur                       **
**                                                                           **
**                                                                           **
******************************************************************************/

/******************************************************************************
** DESCRIPTION                                                               **
** Programme de la classe Game qui permet de gérer les interactions          **
** principales dans le jeu du Pac-Man : affiche le score du joueur, affiche  **
** le nombre de vies restantes, affiche l'introduction du jeu, affiche le    **
** Game Over.                                                                **
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;

namespace PacManSimplifie
{
    public class Game
    {
        /// <summary>
        /// Caractère pour remplir le plateau d'espace
        /// </summary>
        private const char _SPACE = ' ';

        /// <summary>
        /// Caractère du point
        /// </summary>
        private const char _SQUARE = '█';

        /// <summary>
        /// Longueur d'un pion (Pac-Man ou fantôme)
        /// </summary>
        private const byte _PAWN_LENGTH = 7;

        /// <summary>
        /// Nombre de vies restant au joueur
        /// </summary>
        private byte _bytNbLives = 3;

        /// <summary>
        /// Nombre de vies restant au Pac-Man
        /// </summary>
        public byte BytNbLives
        {
            get
            {
                return _bytNbLives;
            }
            set
            {
                _bytNbLives = value;
            }
        }

        /// <summary>
        /// Affiche le score
        /// </summary>
        /// <param name="bytScore">Score à afficher</param>
        public void DisplayScore(byte bytScore)
        {
            ////////////////////////////////////////////////////// CONSTANTES/////////////////////////////////////////////////////
            const byte FIXED_ROW = 68;                  // Position de la ligne d'affichage
            const byte FIXED_COL = 16;                  // Position de la colonne d'affichage
            ////////////////////////////////////////////////////// VARIABLES /////////////////////////////////////////////////////
            string strScore = "";                       // Score à afficher 
            byte i = 0;                                 // Indice pour parcourir les lignes du tableau
            byte j = 0;                                 // Indice pour parcourir les colonnes du tableau
            byte bytDigitsHeight = 5;                   // Hauteur et largeur du digit
            byte[] bytTabDigits;                        // Tableau des digits à afficher
            ConsoleColor white = ConsoleColor.White;    // Colore en blanc le score en digits

            // Tableau de caractères représentant chaque chiffre à représenter
            // 0
            char[,] chrDigit0 =  {{'█', '█', '█'},
                                  {'█', ' ', '█'},
                                  {'█', ' ', '█'},
                                  {'█', ' ', '█'},
                                  {'█', '█', '█'}};

            // 1
            char[,] chrDigit1 =  {{' ', ' ', '█'},
                                  {' ', ' ', '█'},
                                  {' ', ' ', '█'},
                                  {' ', ' ', '█'},
                                  {' ', ' ', '█'}};

            // 2
            char[,] chrDigit2 =  {{'█', '█', '█'},
                                  {' ', ' ', '█'},
                                  {'█', '█', '█'},
                                  {'█', ' ', ' '},
                                  {'█', '█', '█'}};

            // 3
            char[,] chrDigit3 =  {{'█', '█', '█'},
                                  {' ', ' ', '█'},
                                  {'█', '█', '█'},
                                  {' ', ' ', '█'},
                                  {'█', '█', '█'}};

            // 4
            char[,] chrDigit4 =  {{'█', ' ', '█'},
                                  {'█', ' ', '█'},
                                  {'█', '█', '█'},
                                  {' ', ' ', '█'},
                                  {' ', ' ', '█'}};

            // 5
            char[,] chrDigit5 =  {{'█', '█', '█'},
                                  {'█', ' ', ' '},
                                  {'█', '█', '█'},
                                  {' ', ' ', '█'},
                                  {'█', '█', '█'}};

            // 6
            char[,] chrDigit6 =  {{'█', '█', '█'},
                                  {'█', ' ', ' '},
                                  {'█', '█', '█'},
                                  {'█', ' ', '█'},
                                  {'█', '█', '█'}};

            // 7
            char[,] chrDigit7 =  {{'█', '█', '█'},
                                  {' ', ' ', '█'},
                                  {' ', ' ', '█'},
                                  {' ', ' ', '█'},
                                  {' ', ' ', '█'}};

            // 8
            char[,] chrDigit8 =  {{'█', '█', '█'},
                                  {'█', ' ', '█'},
                                  {'█', '█', '█'},
                                  {'█', ' ', '█'},
                                  {'█', '█', '█'}};

            // 9
            char[,] chrDigit9 =  {{'█', '█', '█'},
                                  {'█', ' ', '█'},
                                  {'█', '█', '█'},
                                  {' ', ' ', '█'},
                                  {'█', '█', '█'}};
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Convertit le score du joueur
            strScore = bytScore.ToString();

            // Initialise le tableau des digits
            bytTabDigits = new byte[strScore.Length];

            // Parcourt le score à afficher et le convertit en byte pour le stocker dans le tableau des digits
            for (i = 0; i < strScore.Length; i++)
            {
                bytTabDigits[i] = byte.Parse(strScore[i].ToString());
            }

            // Parcourt le tableau de digits
            for (i = 0; i < bytDigitsHeight; i++)
            {
                // Positionne le score en dessous du plateau
                Console.SetCursorPosition(FIXED_COL, FIXED_ROW + i);

                for (j = 0; j < bytTabDigits.Length; j++)
                {
                    // Affiche chaque digit du score
                    switch (bytTabDigits[j])
                    {
                        case 0:
                            Console.ForegroundColor = white;
                            Console.Write(chrDigit0[i, 0]);
                            Console.Write(chrDigit0[i, 1]);
                            Console.Write(chrDigit0[i, 2]);
                            break;
                        case 1:
                            Console.ForegroundColor = white;
                            Console.Write(chrDigit1[i, 0]);
                            Console.Write(chrDigit1[i, 1]);
                            Console.Write(chrDigit1[i, 2]);
                            break;
                        case 2:
                            Console.ForegroundColor = white;
                            Console.Write(chrDigit2[i, 0]);
                            Console.Write(chrDigit2[i, 1]);
                            Console.Write(chrDigit2[i, 2]);
                            break;
                        case 3:
                            Console.ForegroundColor = white;
                            Console.Write(chrDigit3[i, 0]);
                            Console.Write(chrDigit3[i, 1]);
                            Console.Write(chrDigit3[i, 2]);
                            break;
                        case 4:
                            Console.ForegroundColor = white;
                            Console.Write(chrDigit4[i, 0]);
                            Console.Write(chrDigit4[i, 1]);
                            Console.Write(chrDigit4[i, 2]);
                            break;
                        case 5:
                            Console.ForegroundColor = white;
                            Console.Write(chrDigit5[i, 0]);
                            Console.Write(chrDigit5[i, 1]);
                            Console.Write(chrDigit5[i, 2]);
                            break;
                        case 6:
                            Console.ForegroundColor = white;
                            Console.Write(chrDigit6[i, 0]);
                            Console.Write(chrDigit6[i, 1]);
                            Console.Write(chrDigit6[i, 2]);
                            break;
                        case 7:
                            Console.ForegroundColor = white;
                            Console.Write(chrDigit7[i, 0]);
                            Console.Write(chrDigit7[i, 1]);
                            Console.Write(chrDigit7[i, 2]);
                            break;
                        case 8:
                            Console.ForegroundColor = white;
                            Console.Write(chrDigit8[i, 0]);
                            Console.Write(chrDigit8[i, 1]);
                            Console.Write(chrDigit8[i, 2]);
                            break;
                        case 9:
                            Console.ForegroundColor = white;
                            Console.Write(chrDigit9[i, 0]);
                            Console.Write(chrDigit9[i, 1]);
                            Console.Write(chrDigit9[i, 2]);
                            break;
                        default:
                            break;
                    }
                    // Sépare chaque digit d'un espace
                    Console.Write(_SPACE);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Crée les Pac-Mans représentant les vies du joueur et les affiche
        /// </summary>
        public void DisplayPacManLives()
        {
            ////////////////////////////////////////////////////// CONSTANTES/////////////////////////////////////////////////////
            const byte FIXED_ROW = 67;                  // Position de la ligne d'affichage
            const byte FIXED_COL = 56;                  // Position de la colonne d'affichage
            const byte GAP = 8;                         // Ecart entre les Pac-Mans
            const byte MINIMUM_NUMBER_OF_LIVES = 0;     // Nombre minimum de vie du joueur avant qu'il perde la partie
            const byte INITIAL_NUMBER_OF_LIVES = 3;     // Nombre de vie du joueur quand il commence le jeu
            const byte MOUTH_OPENING = 2;               // Ouverture de la bouche de Pac-Man
            ////////////////////////////////////////////////////// VARIABLES//////////////////////////////////////////////////////
            Pacman pacPacman;                                                       // Représentation visuelle de Pac-Man
            Position posPosition;                                                   // Tableau de position de Pac-Man
            Position posPositionPacmanToErase = new Position(FIXED_COL, FIXED_ROW); // Position du Pac-Man à effacer
            byte bytPacManDirection = 2;                                            // Pac-Man va vers la gauche
            byte bytCol = 0;                                                        // Colonne où commence le Pac-Man
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Vérifie que le nombre de vies est compris entre 1 et 3
            if (_bytNbLives >= MINIMUM_NUMBER_OF_LIVES && _bytNbLives <= INITIAL_NUMBER_OF_LIVES)
            {
                // Crée autant de Pac-Man que de vies restantes au joueur
                for (byte i = 0; i < _bytNbLives; i++)
                {
                    // Met à jour la colonne où commence le Pac-Man
                    bytCol = (byte)(FIXED_COL - GAP * i);

                    // Initialise la position du Pac-Man à afficher
                    posPosition = new Position(bytCol, FIXED_ROW);

                    // Initialise et affiche un Pac-Man représentant une vies
                    pacPacman = new Pacman(posPosition, bytPacManDirection, MOUTH_OPENING);
                }

                // Efface le(s) Pac-Man(s) mort(s)
                for (byte i = 1; i <= INITIAL_NUMBER_OF_LIVES - _bytNbLives; i++)
                {
                    // Met à jour la colonne où commence le 4e Pac-Man (imaginaire) afin de pouvoir utiliser bytCol pour effacer la dernière vie
                    if (_bytNbLives == 0)
                    {
                        bytCol = (byte)(FIXED_COL - GAP * -1);
                    }

                    // Met à jour la position du Pac-Man mort à effacer
                    posPositionPacmanToErase.X = (byte)(bytCol - GAP * i);

                    // Efface le Pac-Man mort
                    pacPacman = new Pacman(posPositionPacmanToErase, bytPacManDirection, MOUTH_OPENING);
                    pacPacman.Erase(posPositionPacmanToErase);
                }
            }
        }// End DisplayPacManLives()

        /// <summary>
        /// Méthode affichant l'introduction au jeu
        /// </summary>
        public void StartGame()
        {
            // Affiche le nom du jeu
            Console.SetCursorPosition(0, 12);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("   ██████╗   █████╗   ███████╗ ███╗   ███╗  █████╗  ███╗   ██╗");
            Console.WriteLine("   ██╔══██╗ ██╔══██╗  ██╔════╝ ████╗ ████║ ██╔══██╗ ████╗  ██║");
            Console.WriteLine("   ██████╔╝ ███████║  ██║      ██╔████╔██║ ███████║ ██╔██╗ ██║");
            Console.WriteLine("   ██╔═══╝  ██╔══██║  ██║      ██║╚██╔╝██║ ██╔══██║ ██║╚██╗██║");
            Console.WriteLine("   ██║      ██║  ██║  ╚██████╗ ██║ ╚═╝ ██║ ██║  ██║ ██║ ╚████║");
            Console.WriteLine("   ╚═╝      ╚═╝  ╚═╝   ╚═════╝ ╚═╝     ╚═╝ ╚═╝  ╚═╝ ╚═╝  ╚═══╝");

            // Affiche les instructions
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\t Bienvenue dans le jeu Pac-Man !\n\n");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("   █ Utilisez les touches fléchées pour déplacer le Pac-Man.\n\n");
            Console.WriteLine();
            Console.WriteLine("   █ Essayez de manger tous les points.\n\n");
            Console.WriteLine();
            Console.WriteLine("   █ Ne vous faites pas attrapper par les fantômes.\n\n");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("   █ Appuyez sur une touche pour commencer...\n\n");

            // Rend le curseur invisible
            Console.CursorVisible = false;

            // Position initiale du mini Pac-Man qui défile
            int pacmanX = 1;
            int pacmanY = Console.WindowHeight / 2;

            // Boucle pour faire déplacer le mini Pac-Man de gauche à droite
            do
            {
                if (Console.KeyAvailable)
                {
                    // Arrête le thread si une touche est pressée par l'utilisateur
                    break;
                }

                // Affiche le mini Pac-Man
                Console.SetCursorPosition(pacmanX, pacmanY);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("C");
                Console.ResetColor();

                // Efface la position précédente du mini Pac-Man
                Console.SetCursorPosition(pacmanX - 1, pacmanY);
                Console.Write(" ");

                // Pause pour ralentir le mouvement du Pac-Man
                Thread.Sleep(100);

                // Avance la position du Pac-Man
                pacmanX++;
            } while (pacmanX < Console.WindowWidth - 1);

            // Eface la console
            Console.Clear();
        }

        /// <summary>
        /// Affiche que le joueur a perdu
        /// </summary>
        public void DisplayGameOver()
        {
            ////////////////////////////////////////////////////// CONSTANTES /////////////////////////////////////////////////////
            const byte FIXED_ROW = 80;                          // Position de la ligne d'affichage
            const byte FIXED_COL = 7;                           // Position de la colonne d'affichage
            string[] strGAMEOVER;                               // Tableau contenant le texte GAME-OVER
            ConsoleColor textColor = ConsoleColor.DarkMagenta;  // Couleur du texte
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Initialise le tableau contenant le message de fin de jeu
            strGAMEOVER = new string[] {
                "█████   █   █   █ ████   ███  █   █ ████ ███",
                "█      █ █  ██ ██ █     █   █ █   █ █    █  █",
                "█ ███ █   █ █ █ █ ███   █   █ █   █ ███  ███",
                "█   █ █████ █   █ █     █   █  █ █  █    █  █",
                "█████ █   █ █   █ ████   ███    █   ████ █  █"
            };

            // Parcourt le tableau du message pour l'afficher
            for (byte i = 0; i < strGAMEOVER.Length; i++)
            {
                // Positionne le score en dessous du plateau
                Console.SetCursorPosition(FIXED_COL, FIXED_ROW + i);

                //Affiche le texte en dark magenta
                Console.ForegroundColor = textColor;
                Console.WriteLine(strGAMEOVER[i]);
            }
        }

        /// <summary>
        /// Permet de vérifier si Pac-Man est entré en collision avec un fantôme
        /// </summary>
        /// <returns>true si la collision a eu lieu, sinon false</returns>
        public bool CheckCollisionGhost(Position posPacmanPosition, List<Position> posGhostsPositions)
        {
            // Vérifie s'il y a une collision avec chaque fantôme
            foreach (Position posGhostPosition in posGhostsPositions)
            {
                // Vérifie pour chaque case d'une ligne/colonne extérieure du Pac-Man s'il y a une collision
                for (byte i = 0; i < _PAWN_LENGTH; i++)
                {
                    // Vérifie pour chaque case d'une ligne/colonne extérieure du fantôme s'il y a une collision
                    for (byte j = 0; j < _PAWN_LENGTH; j++)
                    {
                        // Vérifie s'il y a une collision entre une des deux premières lignes du haut du Pac-Man et la ligne du bas du fantôme
                        if (posPacmanPosition.X + i == posGhostPosition.X + j && (posPacmanPosition.Y == posGhostPosition.Y + _PAWN_LENGTH - 1 || posPacmanPosition.Y + 1 == posGhostPosition.Y + _PAWN_LENGTH - 1))
                        {
                            return true;
                        }
                        // Vérifie s'il y a une collision entre une des deux premières lignes du bas du Pac-Man et la ligne du haut du fantôme
                        else if (posPacmanPosition.X + i == posGhostPosition.X + j && (posPacmanPosition.Y + _PAWN_LENGTH - 1 == posGhostPosition.Y || posPacmanPosition.Y + _PAWN_LENGTH - 2 == posGhostPosition.Y))
                        {
                            return true;
                        }
                        // Vérifie s'il y a une collision entre une des deux premières colonne de droite du Pac-Man et la colonne de gauche du fantôme
                        else if ((posPacmanPosition.X + _PAWN_LENGTH - 1 == posGhostPosition.X || posPacmanPosition.X + _PAWN_LENGTH - 2 == posGhostPosition.X) && posPacmanPosition.Y + i == posGhostPosition.Y + j)
                        {
                            return true;
                        }
                        // Vérifie s'il y a une collision entre une des deux premières colonne de gauche du Pac-Man et la colonne de droite du fantôme
                        else if ((posPacmanPosition.X == posGhostPosition.X + _PAWN_LENGTH - 1 || posPacmanPosition.X + 1 == posGhostPosition.X + _PAWN_LENGTH - 1) && posPacmanPosition.Y + i == posGhostPosition.Y + j)
                        {
                            return true;
                        }

                    }// End for (byte j
                }// End for (byte i
            }// End foreach
            
            // Il n'y pas de collision
            return false;
        }// public bool MeetGhost
    }
}
