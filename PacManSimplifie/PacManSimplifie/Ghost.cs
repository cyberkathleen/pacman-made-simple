/******************************************************************************
** PROGRAMME  PacManSimplifie.cs                                             **
**                                                                           **
** Lieu      : ETML - section informatique                                   **
** Auteur    : ABDULKADIR Salma                                              **
** Date      : 01.05.2023                                                    **
**                                                                           **
** Modifications                                                             **
**   Auteur  : LU Kathleen                                                   **
**   Version : 1.6                                                           **
**   Date    : 01.01.2024                                                    **
**   Raisons : Suppression de la méthode EraseGhost() remplacée par          **
**             EraseAndRedraw()) dans la classe Points.                      **
**                                                                           **
** Modifications                                                             **
**   Auteur  : LU Kathleen                                                   **
**   Version : 1.2                                                           **
**   Date    : 28.05.2023                                                    **
**   Raisons : Optmisation du code                                           **
**                                                                           **
**                                                                           **
******************************************************************************/

/******************************************************************************
** DESCRIPTION                                                               **
** Classe Ghost (fantôme) qui permet la création des fantômes, leur          **
** déplacement aléatoire et leur rencontre avec Pac-Man.                     **
******************************************************************************/

using System;
using System.Collections.Generic;

namespace PacManSimplifie
{
    public class Ghost
    {
        /// <summary>
        /// Longueur du dessin du fantôme
        /// </summary>
        private const byte _LENGTH = 7;

        /// <summary>
        /// Position du fantôme dans la console
        /// </summary>
        private Position _posGhostPosition;

        /// <summary>
        /// Liste des directions qu'un fantôme peut prendre
        /// </summary>
        List<Position> _possibleDirections = new List<Position>();

        /// <summary>
        /// Permet de générer la direction de manière aléatoire
        /// </summary>
        private Random _random;

        /// <summary>
        /// Tableau de caractères qui dessine le fantôme dans la console
        /// </summary>
        private char[,] _tab_charGhost = new char[_LENGTH, _LENGTH];

        /// <summary>
        /// Couleur du fantôme
        /// </summary>
        private string _strColor;

        /// <summary>
        /// La direction dans laquelle le fantôme est en train d'aller, soit "right", "left", "up" ou "down"
        /// </summary>
        private string _strDirection;

        /// <summary>
        /// Est-ce que le fantôme est à une intersection de la map
        /// </summary>
        private bool _boolIsIntersection = false;

        /// <summary>
        /// Plateau du jeu
        /// </summary>
        private Map _mapMap;

        /// <summary>
        /// Les points du jeu
        /// </summary>
        private Points _ptsPoints;

        /// <summary>
        /// Pacman 
        /// </summary>
        private Pacman _pcmPacman;

        /// <summary>
        /// Tableau des directions dans lesquelles le fantôme peut se déplacer
        /// </summary>
        private string[] _directions = new string[] { "right", "left", "up", "down" };

        /// <summary>
        /// Est-ce que le fantôme est en train de traverser le tunnel
        /// </summary>
        private bool _boolIsTunnel = false;

        /// <summary>
        /// Crée d'un tableau de caractères à deux dimensions qui dessine un fantôme dans la console
        /// </summary>
        /// <returns>Retourne un tableau de caractères qui dessine un fantôme dans la console</returns>
        private char[,] Create()
        {
            _tab_charGhost = new char[,] { {' ','█','█','█','█','█',' ',},
                                            {'█','█','█','█','█','█','█',},
                                            {'█','█','█','█','█','█','█',},
                                            {'█','█','█','█','█','█','█',},
                                            {'█','█','█','█','█','█','█',},
                                            {'█','█','█','█','█','█','█'},
                                            {'█',' ','█',' ','█',' ','█',} };
            return _tab_charGhost;

        }

        /// <summary>
        /// Longueur du dessin du fantôme
        /// </summary>
        public byte LENGTH
        {
            get
            {
                return _LENGTH;
            }
        }

        /// <summary>
        /// Accesseur de la position du fantôme
        /// </summary>
        public Position Position
        {
            get { return _posGhostPosition; }
            set { _posGhostPosition = value; }
        }

        /// <summary>
        /// Constructeur personnalisé avec paramètres
        /// </summary>
        /// <param name="position">paramètre de la position des fantômes dans la console</param>
        /// <param name="ghostColor">paramètre de la couleur des fantômes</param>

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="position">La position du fantôme dans la console</param>
        /// <param name="ghostColor">La couleur du fantôme</param>
        /// <param name="direction">La direction de déplacement de départ du fantôme</param>
        /// <param name="map">Le plateau de jeu dans lequel se trouve le fantôme</param>
        /// <param name="points">Les points du jeu</param>
        /// <param name="pacman">Le Pac-Man du jeu</param>
        public Ghost(Position position, string ghostColor, string direction, Map map, Points points, Pacman pacman)
        {
            _posGhostPosition = position;
            _strColor = ghostColor;
            _strDirection = direction;
            _random = new Random();
            _mapMap = map;
            _ptsPoints = points;
            _pcmPacman = pacman;

            // Création du tableau de caractères qui représente le fantôme
            _tab_charGhost = Create();

        }// End public Ghost()

        /// <summary>
        /// Affiche les fantômes à l'aide du tableau de caractères, selon la couleur et la direction des yeux
        /// </summary>
        public void DisplayGhost()
        {
            // Positions des pupilles du fantôme
            byte bytLeftPupilRow = 2;
            byte bytLeftPupilColumn = 2;
            byte bytRightPupilRow = 2;
            byte bytRightPupilColumn = 5;

            // Décalage des pupilles en fonction de la position de Pacman :
            // PacMan vient d'en haut à droite
            if (_pcmPacman.PosPosition.X > _posGhostPosition.X && _pcmPacman.PosPosition.Y > _posGhostPosition.Y || _pcmPacman.PosPosition.X > _posGhostPosition.X && _pcmPacman.PosPosition.Y == _posGhostPosition.Y)
            {
                bytRightPupilColumn = 2;
                bytRightPupilRow = 3;
                bytLeftPupilColumn = 5;
                bytLeftPupilRow = 3;
            }
            // PacMan vient d'en haut à gauche
            else if (_pcmPacman.PosPosition.X < _posGhostPosition.X && _pcmPacman.PosPosition.Y < _posGhostPosition.Y || _pcmPacman.PosPosition.X < _posGhostPosition.X && _pcmPacman.PosPosition.Y == _posGhostPosition.Y)
            {
                bytRightPupilColumn = 1;
                bytRightPupilRow = 3;
                bytLeftPupilColumn = 4;
                bytLeftPupilRow = 3;
            }
            // PacMan vient d'en bas à gauche
            if (_pcmPacman.PosPosition.X < _posGhostPosition.X && _pcmPacman.PosPosition.Y > _posGhostPosition.Y || _pcmPacman.PosPosition.X == _posGhostPosition.X && _pcmPacman.PosPosition.Y > _posGhostPosition.Y)
            {
                bytRightPupilColumn = 1;
                bytRightPupilRow = 3;
                bytLeftPupilColumn = 4;
                bytLeftPupilRow = 3;
            }
            // PacMan vient d'en bas à droite
            else if (_pcmPacman.PosPosition.X < _posGhostPosition.X && _pcmPacman.PosPosition.Y < _posGhostPosition.Y || _pcmPacman.PosPosition.X == _posGhostPosition.X && _pcmPacman.PosPosition.Y < _posGhostPosition.Y)
            {
                bytRightPupilColumn = 1;
                bytRightPupilRow = 2;
                bytLeftPupilColumn = 4;
                bytLeftPupilRow = 2;
            }

            // Boucles pour parcourir le tableau de caractères (fantôme)
            for (byte i = 0; i < 7; i++)
            {
                Console.SetCursorPosition(_posGhostPosition.X, _posGhostPosition.Y + i);

                for (byte j = 0; j < 7; j++)
                {
                    // Colorie la pupille en bleu
                    if ((i == bytLeftPupilRow && j == bytLeftPupilColumn) || (i == bytRightPupilRow && j == bytRightPupilColumn))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(_tab_charGhost[i, j]);
                        Console.ResetColor();
                    }
                    // Colorie les yeux
                    else if (i == 2 && j == 1 || i == 3 && j == 1 || i == 3 && j == 2 || i == 2 && j == 4 || i == 3 && j == 4 || i == 3 && j == 5 || i == 2 && j == 2 || i == 2 && j == 5)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(_tab_charGhost[i, j]);
                        Console.ResetColor();
                    }
                    // Colorie le reste du fantôme selon la couleur
                    else
                    {
                        if (_strColor == "red")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(_tab_charGhost[i, j]);
                            Console.ResetColor();
                        }
                        else if (_strColor == "yellow")
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(_tab_charGhost[i, j]);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(_tab_charGhost[i, j]);
                            Console.ResetColor();
                        }
                    }//End  if ((i == leftPupilRow
                }//End for (byte j = 0
            }//End  for (byte i = 0
        }//End Display()


        /// <summary>
        /// Méthode de déplacement des fantômes
        /// </summary>
        public void MoveGhost()
        {
            // Efface le fantôme à l'ancienne position
            //_ptsPoints.EraseAndRedraw(_posGhostPosition, _LENGTH);

            // Vérifie s'il y a un mûr là oû le fantôme veut aller (= si le fantôme est à une intersection)
            _boolIsIntersection = CheckCollisionWall(_strDirection);

            // S'il est à une intersection, il obtient une nouvelle direction valides
            if (_boolIsIntersection)
            {
                // Obtient la liste des directions valides
                List<string> validDirections = GetValidDirection();

                // Vérifie que la liste des directions valides n'est pas vide et choisit une nouvelle direction parmi les directions possibles
                if (validDirections.Count > 0)
                {
                    // Choisit une nouvelle direction aléatoirement dans la liste de directions valides et la stocke dans une variable
                    _strDirection = validDirections[_random.Next(validDirections.Count)];
                }
            }

            // Met à jour la position du fantôme avec la nouvelle position
            switch (_strDirection)
            {
                case "right":
                    _posGhostPosition.X++;
                    break;
                case "left":
                    _posGhostPosition.X--;
                    break;
                case "up":
                    _posGhostPosition.Y--;
                    break;
                case "down":
                    _posGhostPosition.Y++;
                    break;
                default:
                    break;
            }

            // Affiche le fantôme à sa nouvelle position
            DisplayGhost();
        }// End public void MoveGhost()

        /// <summary>
        /// Obtient une liste des prochaines positions valides (= il n'y a pas de mur) pour le fantôme
        /// </summary>
        /// <returns>La liste des prochaines positions valides</returns>
        public List<string> GetValidDirection()
        {
            // Ajoute les 4 positions autour du fantôme à la liste des directions possible
            _possibleDirections.Add(new Position((byte)(Position.X + 1), Position.Y));
            _possibleDirections.Add(new Position((byte)(Position.X - 1), Position.Y));
            _possibleDirections.Add(new Position(Position.X, (byte)(Position.Y - 1)));
            _possibleDirections.Add(new Position(Position.X, (byte)(Position.Y + 1)));

            // Liste de directions valides sans mur
            List<string> validDirections = new List<string>();

            // Vérifie s'il y a un mur dans chaque direction autour du fantôme
            for (byte i = 0; i < _directions.Length; i++)
            {
                // S'il n'y a pas de mur, enregistre la direction possible
                if (!CheckCollisionWall(_directions[i]))
                {
                    // Enregistre les directions sans mur dans une liste de directions valides
                    validDirections.Add(_directions[i]);
                }
            }

            // Retourne la liste des prochaines positions valides
            return validDirections;
        }// End public List<string> GetValidDirection()

        /// <summary>
        /// Vérifie s'il y a un mur dans la direction où le fantôme souhaite se déplacer
        /// </summary>
        /// <param name="direction">La direction où le fantômne souhaite aller</param>
        /// <returns>true s'il y a une collision, sinon false</returns>
        public bool CheckCollisionWall(string direction)
        {
            /////////////////////////////////////// Constantes ///////////////////////////////////////
            const char WALL = 'W';          // Charactère utilisé pour dessiné les murs
            const char TUNNEL = 'T';        // Charactère utilisé pour dessiné les tunnels
            /////////////////////////////////////// Variables ///////////////////////////////////////
            bool noWall = false;            // Est-ce qu'il y a un mur là où le fantôme veut aller ?
            /////////////////////////////////////////////////////////////////////////////////////////

            // Vérifie s'il y a un mur dans la direction où le fantôme veut aller
            switch (direction)
            {
                // A droite
                case "right":

                    // Vérifie s'il y a un mur juste à droite de chaque ligne du fantôme 
                    for (int i = 0; i < _LENGTH; i++)
                    {
                        // ?
                        if (_mapMap.chrMap[_posGhostPosition.Y + i, _posGhostPosition.X + _LENGTH] == WALL)
                        {
                            noWall = false;
                            break;
                        }
                        else
                        {
                            noWall = true;
                        }
                    }

                    // S'il n'y a pas de mur à droite, vérifie s'il y a le tunnel
                    if (noWall)
                    {
                        // Est-ce qu'il y a le tunnel
                        if (_mapMap.chrMap[_posGhostPosition.Y, _posGhostPosition.X + _LENGTH] == TUNNEL)
                        {
                            _boolIsTunnel = true;
                        }
                    }
                    break;

                // A gauche
                case "left":
                    // Vérifie s'il y a un mur juste à gauche de chaque ligne du fantôme
                    for (int i = 0; i < _LENGTH; i++)
                    {
                        // ?
                        if (_mapMap.chrMap[_posGhostPosition.Y + i, _posGhostPosition.X - 1] == WALL)
                        {
                            noWall = false;
                            break;
                        }
                        else
                        {
                            noWall = true;
                        }
                    }

                    // S'il n'y a pas de mur à gauche, vérifie s'il y a le tunnel
                    if (noWall)
                    {
                        // Est-ce qu'il y a le tunnel
                        if (_mapMap.chrMap[_posGhostPosition.Y, _posGhostPosition.X - 1] == TUNNEL)
                        {
                            _boolIsTunnel = true;
                        }
                    }
                    break;

                // En-haut
                case "up":
                    // Vérifie s'il y a un mur juste en-haut de chaque colonne du fantôme
                    for (int i = 0; i < _LENGTH; i++)
                    {
                        // ?
                        if (_mapMap.chrMap[_posGhostPosition.Y - 1, _posGhostPosition.X + i] == WALL)
                        {
                            noWall = false;
                            break;
                        }
                        else
                        {
                            noWall = true;
                        }
                    }
                    break;

                // En-bas
                case "down":
                    // Vérifie s'il y a un mur juste en-bas de chaque colonne du fantôme
                    for (int i = 0; i < _LENGTH; i++)
                    {
                        if (_mapMap.chrMap[_posGhostPosition.Y + _LENGTH, _posGhostPosition.X + i] == WALL)
                        {
                            noWall = false;
                            break;
                        }
                        else
                        {
                            noWall = true;
                        }
                    }
                    break;
                default:
                    break;
            }

            // Retourne s'il y a un mur ou pas
            return !noWall;
        }// End CheckCollisionWall()
    }
}