/******************************************************************************
** PROGRAMME  PacManSimplifie.cs                                             **
**                                                                           **
** Lieu      : ETML - section informatique                                   **                                
** Auteur    : LU Kathleen                                                   **
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
**   Version : 1.4                                                           **
**   Date    : 28.05.2023                                                    **
**   Raisons : Optmisation de la méthode Erase()                             **
**                                                                           **
** Modifications                                                             **
**   Auteur  : LU Kathleen                                                   **
**   Version : 1.3                                                           **
**   Date    : 28.05.2023                                                    **
**   Raisons : Optmisation du code                                           **
**                                                                           **
** Modifications                                                             **
**   Auteur  : ABID Fatima                                                   **
**   Version : 1.2                                                           **
**   Date    : 28.05.2023                                                    **
**   Raisons : Ajout de la méthode CheckCollisionPoint()                     **
**                                                                           **
**                                                                           **
******************************************************************************/

/******************************************************************************
** DESCRIPTION                                                               **
** Programme de la classe Pacman qui permet de créer un Pac-Man.             **
** Le Pac-Man possède une couleur, un ouverture de bouche, une direction de  **
** déplacement et une position. Il est possible d'afficher le Pac-Man dans   **
** la bonne direction avec la bonne ouverture de bouche, de le déplacer, de  **
** vérifier s'il entre en collision avec un mur, de lui faire traverser le   **
** tunnel et de vérifier s'il a mangé un point.                              **
******************************************************************************/

using System;
using System.Reflection;

namespace PacManSimplifie
{
    public class Pacman
    {
        /// <summary>
        /// Longueur du dessin du Pac-Man
        /// </summary>
        private const byte _LENGTH = 7;

        /// <summary>
        /// Couleur du Pac-Man
        /// </summary>
        private ConsoleColor _colColor;

        /// <summary>
        /// Caractère utilisé pour dessiner Pac-Man
        /// </summary>
        private const char _SYMBOL = '█';

        /// <summary>
        /// Dessins du Pac-Man lorsqu'il a la bouche complètement ouverte dans les 4 directions
        /// </summary>
        private char[,,] _tab_chrMouthOpened = new char[,,]
        {
            {
                {' ', ' ', '█', '█', '█', ' ', ' '},
                {' ', '█', '█', '█', '█', '█', ' '},
                {'█', '█', '█', '█', ' ', ' ', ' '},
                {'█', '█', '█', ' ', ' ', ' ', ' '},
                {'█', '█', '█', '█', ' ', ' ', ' '},
                {' ', '█', '█', '█', '█', '█', ' '},
                {' ', ' ', '█', '█', '█', ' ', ' '}
            },
            {
                {' ', ' ', '█', '█', '█', ' ', ' '},
                {' ', '█', '█', '█', '█', '█', ' '},
                {'█', '█', '█', '█', '█', '█', '█'},
                {'█', '█', '█', ' ', '█', '█', '█'},
                {'█', '█', ' ', ' ', ' ', '█', '█'},
                {' ', '█', ' ', ' ', ' ', '█', ' '},
                {' ', ' ', ' ', ' ', ' ', ' ', ' '}
            },
            {
                {' ', ' ', '█', '█', '█', ' ', ' '},
                {' ', '█', '█', '█', '█', '█', ' '},
                {' ', ' ', ' ', '█', '█', '█', '█'},
                {' ', ' ', ' ', ' ', '█', '█', '█'},
                {' ', ' ', ' ', '█', '█', '█', '█'},
                {' ', '█', '█', '█', '█', '█', ' '},
                {' ', ' ', '█', '█', '█', ' ', ' '}
            },
            {
                {' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {' ', '█', ' ', ' ', ' ', '█', ' '},
                {'█', '█', ' ', ' ', ' ', '█', '█'},
                {'█', '█', '█', ' ', '█', '█', '█'},
                {'█', '█', '█', '█', '█', '█', '█'},
                {' ', '█', '█', '█', '█', '█', ' '},
                {' ', ' ', '█', '█', '█', ' ', ' '}
            }
        };

        /// <summary>
        /// Dessins du Pac-Man lorsqu'il a la bouche à moitié ouverte dans les 4 directions
        /// </summary>
        private char[,,] _tab_chrMouthHalfOpened = new char[,,]
        {
            {
                {' ', ' ', '█', '█', '█', ' ', ' '},
                {' ', '█', '█', '█', '█', '█', ' '},
                {'█', '█', '█', '█', '█', '█', '█'},
                {'█', '█', '█', ' ', ' ', ' ', ' '},
                {'█', '█', '█', '█', '█', '█', '█'},
                {' ', '█', '█', '█', '█', '█', ' '},
                {' ', ' ', '█', '█', '█', ' ', ' '}
            },
            {
                {' ', ' ', '█', '█', '█', ' ', ' '},
                {' ', '█', '█', '█', '█', '█', ' '},
                {'█', '█', '█', '█', '█', '█', '█'},
                {'█', '█', '█', ' ', '█', '█', '█'},
                {'█', '█', '█', ' ', '█', '█', '█'},
                {' ', '█', '█', ' ', '█', '█', ' '},
                {' ', ' ', '█', ' ', '█', ' ', ' '}
            },
            {
                {' ', ' ', '█', '█', '█', ' ', ' '},
                {' ', '█', '█', '█', '█', '█', ' '},
                {'█', '█', '█', '█', '█', '█', '█'},
                {' ', ' ', ' ', ' ', '█', '█', '█'},
                {'█', '█', '█', '█', '█', '█', '█'},
                {' ', '█', '█', '█', '█', '█', ' '},
                {' ', ' ', '█', '█', '█', ' ', ' '}
            },
            {
                {' ', ' ', '█', ' ', '█', ' ', ' '},
                {' ', '█', '█', ' ', '█', '█', ' '},
                {'█', '█', '█', ' ', '█', '█', '█'},
                {'█', '█', '█', ' ', '█', '█', '█'},
                {'█', '█', '█', '█', '█', '█', '█'},
                {' ', '█', '█', '█', '█', '█', ' '},
                {' ', ' ', '█', '█', '█', ' ', ' '}
            }
        };

        /// <summary>
        /// Dessin du Pac-Man lorsqu'il a la bouche fermée
        /// </summary>
        private char[,] _tab_chrMouthClosed = new char[,]
        {
            {' ', ' ', '█', '█', '█', ' ', ' '},
            {' ', '█', '█', '█', '█', '█', ' '},
            {'█', '█', '█', '█', '█', '█', '█'},
            {'█', '█', '█', '█', '█', '█', '█'},
            {'█', '█', '█', '█', '█', '█', '█'},
            {' ', '█', '█', '█', '█', '█', ' '},
            {' ', ' ', '█', '█', '█', ' ', ' '}
        };

        /// <summary>
        /// Etat d'ouverture de la bouche du Pac-Man lorsque qu'il se déplace
        ///     0 -> bouche fermée
        ///     1 -> bouche à moitié ouverte
        ///     2 -> bouche ouverte
        ///     3 -> bouche à moitié ouverte
        /// </summary>
        private byte _bytMouthOpening;

        /// <summary>
        /// Index utilisé pour afficher la direction du Pac-Man dans les attributs _tab_chrMouthOpened et _tab_chrMouthHalfOpened
        /// Les valeurs possibles sont :
        ///     0 -> bouche vers la droite
        ///     1 -> bouche vers le bas
        ///     2 -> bouche vers la gauche
        ///     3 -> bouche vers le haut
        /// </summary>
        private byte _bytDirection;

        /// <summary>
        /// Position du Pac-Man dans la console
        /// </summary>
        private Position _posPosition;

        /// <summary>
        /// Est-ce que le Pac-Man est en train de traverser le tunnel
        /// </summary>
        private bool _boolIsTunnel = false;

        /// <summary>
        /// Longueur du dessin du Pac-Man
        /// </summary>
        public byte LENGTH
        {
            get
            {
                return _LENGTH;
            }
        }

        /// <summary>
        /// Longueur du dessin du Pac-Man
        /// </summary>
        public byte BytLength
        {
            get
            {
                return _LENGTH;
            }
        }

        /// <summary>
        /// Index utilisé pour afficher la direction du Pac-Man dans les attributs _tab_chrMouthOpened et _tab_chrMouthHalfOpened
        /// Les valeurs possibles sont :
        ///     0 -> bouche vers la droite
        ///     1 -> bouche vers le bas
        ///     2 -> bouche vers la gauche
        ///     3 -> bouche vers le haut
        /// </summary>
        //public byte BytDirection
        //{
        //    get
        //    {
        //        return _bytDirection;
        //    }
        //}

        /// <summary>
        /// Position du Pac-Man dans la console
        /// </summary>
        public Position PosPosition
        {
            get
            {
                return _posPosition;
            }
        }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Pacman()
        {

        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="posPosition">Position du Pac-Man dans la console</param>
        /// <param name="bytDirection">Direction dans laquelle le Pac-Man regarde : 0 = droite, 1 = en-bas, 2 = gauche ou 3 = en-haut</param>
        /// <param name="bytMouthOpening">Taille d'ouverture de la bouche : 0 = fermée, 1 = à moitié ouverte, 2 = ouverte, 3 = à moitié ouverte</param>
        public Pacman(Position posPosition, byte bytDirection, byte bytMouthOpening)
        {
            _posPosition = posPosition;
            _colColor = ConsoleColor.Yellow;
            _bytDirection = bytDirection;
            _bytMouthOpening = bytMouthOpening;

            // Affiche le pacman
            this.Print();
        }

        /// <summary>
        /// Affiche le dessin du Pac-Man en fonction de la direction dans laquelle il regarde avec la bonne taille d'ouverture de sa bouche 
        /// </summary>
        public void Print()
        {
            // Mettre la couleur jaune du Pac-Man
            Console.ForegroundColor = _colColor;

            // Parcourt les lignes du Pac-Man
            for (byte i = 0; i < _LENGTH; i++)
            {
                // Parcourt les colonnes du Pac-Man
                for (byte j = 0; j < _LENGTH; j++)
                {
                    // Position de chaque case du Pac-Man
                    Console.SetCursorPosition(_posPosition.X + j, _posPosition.Y + i);

                    // Si le Pac-Man doit avoir la bouche fermée
                    if (_bytMouthOpening == 0)
                    {
                        // Dessine seulement sur les cases du Pac-Man et pas le vide
                        if (_tab_chrMouthClosed[i, j] == _SYMBOL)
                        {
                            Console.Write(_tab_chrMouthClosed[i, j]);
                        }

                    }
                    // Si le Pac-Man doit avoir la bouche complètement ouverte
                    else if (_bytMouthOpening == 2)
                    {
                        // Dessine seulement sur les cases du Pac-Man et pas le vide
                        if (_tab_chrMouthOpened[_bytDirection, i, j] == _SYMBOL)
                        {
                            Console.Write(_tab_chrMouthOpened[_bytDirection, i, j]);
                        }
                    }
                    // Si le Pac-Man doit avoir la bouche à moitié ouverte
                    else if (_bytMouthOpening == 1 || _bytMouthOpening == 3)
                    {
                        // Dessine seulement sur les cases du Pac-Man et pas le vide
                        if (_tab_chrMouthHalfOpened[_bytDirection, i, j] == _SYMBOL)
                        {
                            Console.Write(_tab_chrMouthHalfOpened[_bytDirection, i, j]);
                        }
                    }// End if else (_bytMouthOpening == 0)
                }// End for (byte j
            }// End for (byte i
        } // End Print()

        /// <summary>
        /// Vérifie s'il y a un mur dans la direction où l'utilisateur souhaite faire déplacer le Pac-Man
        /// </summary>
        /// <param name="mapCurrentMap">la map actuelle</param>
        /// <param name="keyDirection">la touche de direction pressée par l'utilisateur</param>
        /// <returns>true s'il n'y a pas de mur, sinon false</returns>
        public bool CheckCollisionWall(Map mapCurrentMap, ConsoleKey keyDirection)
        {
            /////////////////////////////////////// Constantes ///////////////////////////////////////
            const char WALL = 'W';          // Charactère utilisé pour dessiné les murs
            const char TUNNEL = 'T';        // Charactère utilisé pour dessiné les tunnels
            /////////////////////////////////////// Variables ///////////////////////////////////////
            bool boolNoWall = false;        // Est-ce qu'il y a un mur là où le Pac-Man veut aller ?
            /////////////////////////////////////////////////////////////////////////////////////////

            // Détermine la direction en fonction de la touche pressée
            switch (keyDirection)
            {
                // Si l'utilisateur veut aller à droite
                case ConsoleKey.RightArrow:

                    // Vérifie s'il y a un mur juste à droite de chaque ligne du Pac-Man 
                    for (int i = 0; i < _LENGTH; i++)
                    {
                        // Mur ?
                        if (mapCurrentMap.chrMap[_posPosition.Y + i, _posPosition.X + _LENGTH] == WALL)
                        {
                            boolNoWall = false;
                            break;
                        }
                        else
                        {
                            boolNoWall = true;
                        }
                    }

                    // S'il n'y a pas de mur à droite, vérifie s'il y a le tunnel
                    if (boolNoWall)
                    {
                        // Tunnel ?
                        if (mapCurrentMap.chrMap[_posPosition.Y, _posPosition.X + _LENGTH] == TUNNEL)
                        {
                            _boolIsTunnel = true;
                        }
                    }
                    break;

                // Si l'utilisateur veut aller à gauche
                case ConsoleKey.LeftArrow:
                    // Vérifie s'il y a un mur juste à gauche de chaque ligne du Pac-Man
                    for (int i = 0; i < _LENGTH; i++)
                    {
                        // Mur ?
                        if (mapCurrentMap.chrMap[_posPosition.Y + i, _posPosition.X - 1] == WALL)
                        {
                            boolNoWall = false;
                            break;
                        }
                        else
                        {
                            boolNoWall = true;
                        }
                    }

                    // S'il n'y a pas de mur à gauche, vérifie s'il y a le tunnel
                    if (boolNoWall)
                    {
                        // Tunnel ?
                        if (mapCurrentMap.chrMap[_posPosition.Y, _posPosition.X - 1] == TUNNEL)
                        {
                            _boolIsTunnel = true;
                        }
                    }
                    break;

                // Si l'utilisateur veut aller en-haut
                case ConsoleKey.UpArrow:
                    // Vérifie s'il y a un mur juste en-haut de chaque colonne du Pac-Man
                    for (int i = 0; i < _LENGTH; i++)
                    {
                        // Mur ?
                        if (mapCurrentMap.chrMap[_posPosition.Y - 1, _posPosition.X + i] == WALL)
                        {
                            boolNoWall = false;
                            break;
                        }
                        else
                        {
                            boolNoWall = true;
                        }
                    }
                    break;

                // Si l'utilisateur veut aller en-bas
                case ConsoleKey.DownArrow:
                    // Vérifie s'il y a un mur juste en-bas de chaque colonne du Pac-Man
                    for (int i = 0; i < _LENGTH; i++)
                    {
                        // Mur ?
                        if (mapCurrentMap.chrMap[_posPosition.Y + _LENGTH, _posPosition.X + i] == WALL)
                        {
                            boolNoWall = false;
                            break;
                        }
                        else
                        {
                            boolNoWall = true;
                        }
                    }
                    break;
                default:
                    break;
            }// End switch (keyDirection)

            // Retourne s'il y a un mur ou pas
            return boolNoWall;
        }// End CheckCollisionWall()

        /// <summary>
        /// Vérifie s'il y a un point dans la direction où l'utilisateur souhaite faire déplacer le Pac-Man
        /// Vérifie si le Pac-Man a rencontré un bout d'un point (le bout de point et Pac-Man sont superposés)
        /// </summary>
        /// <param name="poiPoints">Points à manger</param>
        /// <param name="mapCurrentMap">La map actuelle</param>
        
        public void EatPoint(Points poiPoints, Map mapCurrentMap)
        {
            ///////////////////////////////////////////////////////////// CONSTANTES /////////////////////////////////////////////////////////////////
            byte bytPointsRowToSearch = _posPosition.Y;         // Ligne de départ de recherche de point
            byte bytPointsColToSearch = _posPosition.X;         // Colonne de départ de recherche de point
            byte bytPcmRowToSearch = 0;                         // Ligne de départ de recherche dans Pac-Man
            byte bytPcmColToSearch = 0;                         // Colonne de départ de recherche dans Pac-Man
            //char chrNextCharacter = ' ';                        // Caractère dans le tableau
            //byte bytDistanceWithPoints = 2;                     // Distance horizontale ou verticale entre Pac-Man et le centre du point à manger
            byte bytDistanceToPoint = 2;                        // Distance horizontale ou verticale entre Pac-Man et une des parties du point à manger
            byte bytPacmanCenterY = (byte)(_posPosition.Y + 3); // Ligne du centre de Pac-Man
            byte bytPacmanCenterX = (byte)(_posPosition.X + 3); // Colonne du centre de Pac-Man

            byte bytEndFori = 0;                                // Nombre de fois après lesquel la boucle i s'arrête
            byte bytEndForj = 0;                                // Nombre de fois après lesquel la boucle j s'arrête
            bool boolIsOnPacman = false;                        // Est-ce que le corps Pac-Man est dessiné sur la case à analyser
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Si le centre de Pac-Man est aligné sur le centre d'un point (à la position de départ et avant et après avoir traversé les tunnels),
            // il mange le point entier et incrémente le score et décrémente le nombre de points restants à manger de 1
            if (poiPoints.chrMap[bytPacmanCenterY, bytPacmanCenterX] == poiPoints.CENTER)
            {
                poiPoints.ChrMap[bytPacmanCenterY - 1, bytPacmanCenterX] = mapCurrentMap.SPACE;
                poiPoints.ChrMap[bytPacmanCenterY, bytPacmanCenterX + 1] = mapCurrentMap.SPACE;
                poiPoints.ChrMap[bytPacmanCenterY, bytPacmanCenterX] = mapCurrentMap.SPACE;
                poiPoints.ChrMap[bytPacmanCenterY + 1, bytPacmanCenterX] = mapCurrentMap.SPACE;
                poiPoints.ChrMap[bytPacmanCenterY, bytPacmanCenterX - 1] = mapCurrentMap.SPACE;
                poiPoints.BytScore++;
                poiPoints.BytRemaining--;
            }

            // Met à jour les données des lignes et colonnes où chercher un point selon la direction de Pac-Man
            switch (_bytDirection)
            {
                // Va à droite
                case 0:
                    bytPointsRowToSearch += bytDistanceToPoint;
                    bytPcmRowToSearch += bytDistanceToPoint;
                    bytEndFori = 3;
                    bytEndForj = _LENGTH;
                    break;
                // Va en bas
                case 1:
                    bytPointsColToSearch += bytDistanceToPoint;
                    bytPcmColToSearch += bytDistanceToPoint;
                    bytEndFori = _LENGTH;
                    bytEndForj = 3;
                    break;
                // Va à gauche
                case 2:
                    bytPointsRowToSearch += bytDistanceToPoint;
                    bytPcmRowToSearch += bytDistanceToPoint;
                    bytEndFori = 3;
                    bytEndForj = _LENGTH;
                    break;
                // Va en haut
                case 3:
                    bytPointsColToSearch += bytDistanceToPoint;
                    bytPcmColToSearch += bytDistanceToPoint;
                    bytEndFori = _LENGTH;
                    bytEndForj = 3;
                    break;
            }

            // Parcourt les lignes du Pac-Man à analyser
            for (byte i = 0; i < bytEndFori; i++)
            {
                // Parcourt les colonnes du Pac-Man à analyser
                for (byte j = 0; j < bytEndForj; j++)
                {
                    // Réinitialise à faux avant d'analyser
                    boolIsOnPacman = false;

                    // Récupère la case à analyser dans Pac-Man en fonction de son ouverture de bouche :
                    // Bouche fermée
                    if (_bytMouthOpening == 0)
                    {
                        // Est-ce que le corps de Pac-Man est dessiné sur la case à analyser ?
                        if (_tab_chrMouthClosed[bytPcmRowToSearch + i, bytPcmColToSearch + j] == _SYMBOL)
                        {
                            boolIsOnPacman = true;
                        }
                    }
                    // Bouche à moitié ouverte
                    else if (_bytMouthOpening == 1 || _bytMouthOpening == 3)
                    {
                        // Est-ce que le corps de Pac-Man est dessiné sur la case à analyser ?
                        if (_tab_chrMouthHalfOpened[_bytDirection, bytPcmRowToSearch + i, bytPcmColToSearch + j] == _SYMBOL)
                        {
                            boolIsOnPacman = true;
                        }
                    }
                    // Bouche ouverte
                    else if (_bytMouthOpening == 2)
                    {
                        // Est-ce que le corps de Pac-Man est dessiné sur la case à analyser ?
                        if (_tab_chrMouthOpened[_bytDirection, bytPcmRowToSearch + i, bytPcmColToSearch + j] == _SYMBOL)
                        {
                            boolIsOnPacman = true;
                        }
                    }// End if else (_bytMouthOpening == 0)

                    // Si == Pac-Man est superposé avec la partie du point à analyser
                    if (boolIsOnPacman)
                    {
                        // Si Pac-Man mange la partie du haut d'un point
                        if (poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j] == poiPoints.TOP)
                        {
                            // Fait disparaitre cette partie du point du tableau des points
                            poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j] = mapCurrentMap.SPACE;

                            // Si les autres parties du points sont déjà mangées, fait disparaitre le centre du point, incrémente le score et décrémente le nombre de points restants à manger de 1 
                            if (poiPoints.chrMap[bytPointsRowToSearch + i + 1, bytPointsColToSearch + j - 1] == mapCurrentMap.SPACE && poiPoints.chrMap[bytPointsRowToSearch + i + 1, bytPointsColToSearch + j + 1] == mapCurrentMap.SPACE && poiPoints.chrMap[bytPointsRowToSearch + i + 2, bytPointsColToSearch + j] == mapCurrentMap.SPACE)
                            {
                                poiPoints.chrMap[bytPointsRowToSearch + i + 1, bytPointsColToSearch + j] = mapCurrentMap.SPACE;
                                poiPoints.BytScore++;
                                poiPoints.BytRemaining--;
                            }
                        }
                        // Si Pac-Man mange la partie droite d'un point
                        else if (poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j] == poiPoints.RIGHT)
                        {
                            // Fait disparaitre cette partie du point du tableau des points
                            poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j] = mapCurrentMap.SPACE;

                            // Si les autres parties du points sont déjà mangées, fait disparaitre le centre du point, incrémente le score et décrémente le nombre de points restants à manger de 1 
                            if (poiPoints.chrMap[bytPointsRowToSearch + i - 1, bytPointsColToSearch + j - 1] == mapCurrentMap.SPACE && poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j - 2] == mapCurrentMap.SPACE && poiPoints.chrMap[bytPointsRowToSearch + i + 1, bytPointsColToSearch + j - 1] == mapCurrentMap.SPACE)
                            {
                                poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j - 1] = mapCurrentMap.SPACE;
                                poiPoints.BytScore++;
                                poiPoints.BytRemaining--;
                            }
                        }
                        // Si Pac-Man mange la partie du bas d'un point
                        else if (poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j] == poiPoints.BOTTOM)
                        {
                            // Fait disparaitre cette partie du point du tableau des points
                            poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j] = mapCurrentMap.SPACE;

                            // Si les autres parties du points sont déjà mangées, fait disparaitre le centre du point, incrémente le score et décrémente le nombre de points restants à manger de 1 
                            if (poiPoints.chrMap[bytPointsRowToSearch + i - 2, bytPointsColToSearch + j] == mapCurrentMap.SPACE && poiPoints.chrMap[bytPointsRowToSearch + i - 1, bytPointsColToSearch + j - 1] == mapCurrentMap.SPACE && poiPoints.chrMap[bytPointsRowToSearch + i - 1, bytPointsColToSearch + j + 1] == mapCurrentMap.SPACE)
                            {
                                poiPoints.chrMap[bytPointsRowToSearch + i - 1, bytPointsColToSearch + j] = mapCurrentMap.SPACE;
                                poiPoints.BytScore++;
                                poiPoints.BytRemaining--;
                            }
                        }
                        // Si Pac-Man mange la partie gauche d'un point
                        else if (poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j] == poiPoints.LEFT)
                        {
                            // Fait disparaitre cette partie du point du tableau des points
                            poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j] = mapCurrentMap.SPACE;

                            // Si les autres parties du points sont déjà mangées, fait disparaitre le centre du point, incrémente le score et décrémente le nombre de points restants à manger de 1 
                            if (poiPoints.chrMap[bytPointsRowToSearch + i - 1, bytPointsColToSearch + j + 1] == mapCurrentMap.SPACE && poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j + 2] == mapCurrentMap.SPACE && poiPoints.chrMap[bytPointsRowToSearch + i + 1, bytPointsColToSearch + j + 1] == mapCurrentMap.SPACE)
                            {
                                poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j + 1] = mapCurrentMap.SPACE;
                                poiPoints.BytScore++;
                                poiPoints.BytRemaining--;
                            }
                        }// End if else (poiPoints.chrMap[bytPointsRowToSearch + i, bytPointsColToSearch + j] == poiPoints.TOP)
                    }// End if (boolIsOnPacman)
                }// End for (byte j = 0
            }// End for (byte i = 0
        }// End CheckCollisionPoint()

        /// <summary>
        /// Efface l'affichage actuel d'un Pac-Man et le remplace par des espaces
        /// </summary>
        /// <param name="posPosition">Position du Pac-Man à effacer</param>
        public void Erase(Position posPosition)
        {
            // Parcourt Pac-Man dans sa hauteur
            for (byte i = 0; i < _LENGTH; i++)
            {
                // Place le curseur en début de ligne
                Console.SetCursorPosition(posPosition.X, posPosition.Y + i);

                // Parcourt Pac-Man dans sa largeur
                for (byte j = 0; j < _LENGTH; j++)
                {
                    // Remplace par un espace
                    Console.Write(" ");
                }
            }
        } // End Erase()

        /// <summary>
        /// Permet de déplacement le Pac-Man dans la direction voulue 
        /// </summary>
        public void Move(Points ptsCurrentPoints)
        {
            /////////////////////////////////////////////// Constantes ///////////////////////////////////////////////
            const byte bytRightTunnelExit = 65 - 1 - _LENGTH;   // Colonne dans la console juste avant le tunnel de droite 
            //////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Efface l'affichage actuel du Pac-Man et réaffiche les points pas mangés
            ptsCurrentPoints.EraseAndRedraw(_posPosition, _LENGTH);

            // Déplace le Pac-Man d'une case dans la direction voulue
            switch (_bytDirection)
            {
                // S'il doit aller à droite
                case 0:
                    // S'il doit traverser le tunnel
                    if (_boolIsTunnel)
                    {
                        _posPosition.X = 1;
                        _boolIsTunnel = false;
                    }
                    else
                    {
                        _posPosition.X++;
                    }
                    break;

                // S'il doit aller en-bas
                case 1:
                    _posPosition.Y++;
                    break;

                // S'il doit aller à gauche
                case 2:
                    // S'il doit traverser le tunnel
                    if (_boolIsTunnel)
                    {
                        _posPosition.X = bytRightTunnelExit;
                        _boolIsTunnel = false;
                    }
                    else
                    {
                        _posPosition.X--;
                    }
                    break;

                // S'il doit aller en-haut
                case 3:
                    _posPosition.Y--;
                    break;
                default:
                    break;
            }

            // Affiche le Pac-Man à sa nouvelle position
            Print();
        } // End Move(byte direction)

        /// <summary>
        /// Fait ouvrir/fermer la bouche du Pac-Man d'un cran en mettant à jour l'état d'ouverture de bouche
        /// </summary>
        public void ChangeMouth()
        {
            // Passe à l'état d'ouverture de bouche suivante
            if (_bytMouthOpening < 3)
            {
                _bytMouthOpening++;
            }
            else
            {
                _bytMouthOpening = 0;
            }            
        }// End ChangeMouth()

        /// <summary>
        /// Met à jour la direction de déplacement de Pac-Man en fonction de la touche pressée par l'utilisateur
        /// </summary>
        /// <param name="keyDirection">la touche de direction pressée par l'utilisateur</param>
        public void ChangeDirection(ConsoleKey keyDirection)
        {
            // Met à jour la direction dans lequel le Pac-Man doit aller en fonction de la touche pressée
            switch (keyDirection)
            {
                // Si l'utilisateur veut aller à droite
                case ConsoleKey.RightArrow:
                    _bytDirection = 0;
                    break;

                // Si l'utilisateur veut aller en-bas
                case ConsoleKey.DownArrow:
                    _bytDirection = 1;
                    break;

                // Si l'utilisateur veut aller à gauche
                case ConsoleKey.LeftArrow:
                    _bytDirection = 2;
                    break;

                // Si l'utilisateur veut aller en-haut
                case ConsoleKey.UpArrow:
                    _bytDirection = 3;
                    break;
                default:
                    break;
            }
        }
    }
}