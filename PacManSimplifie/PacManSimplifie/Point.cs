/******************************************************************************
** PROGRAMME  PacManSimplifie.cs                                             **
**                                                                           **
** Lieu      : ETML - section informatique                                   **                                
** Auteur    : ABID Fatima                                                   **
** Date      : 01.05.2023                                                    **
**                                                                           **
** Modifications                                                             **
**   Auteur  : LU Kathleen                                                   **
**   Version : 1.6                                                           **
**   Date    : 01.01.2024                                                    **
**   Raisons : Modification de la manière d'afficher les points avec CENTER, **
**             TOP, RIGHT, BOTTOM ET LEFT.                                   **
**             Suppression de la méthode EatPoint() dans cette classe        **
**             remplacée par celle dans la classe Pacman. Amélioration :     **
**             point mangé bout par bout et non en entier en une seule fois. **
**             Amélioration de l'effacement (méthode EraseAndRedraw()) de    **
**             Pac-Man et des fantômes afin de pouvoir afficher les parties  **
**             de point non-mangées.                                         **
**                                                                           **
** Modifications                                                             **
**   Auteur  : LU Kathleen                                                   **
**   Version : 1.3                                                           **
**   Date    : 28.05.2023                                                    **
**   Raisons : Modification de la méthode EatPoint()                         **
**                                                                           **
** Modifications                                                             **
**   Auteur  : ABID Fatima                                                   **
**   Version : 1.2                                                           **
**   Date    : 27.05.2023                                                    **
**   Raisons : Optimisation du code                                          **
**                                                                           **
**                                                                           **
******************************************************************************/

/******************************************************************************
** DESCRIPTION                                                               **
** Création et gestion des points répartis sur le plateau. Dans cette classe **
** la méthode CreateAPoint() crée un modèle pour un point, puis la méthode   **
** DistributePointInTheTab() crée et répartit les points dans le tableau.    **
** Il y a 2 méthodes booléennes qui vérifient que le point est bien dans le  **
** tableau (IsInsideMap()) et que l'emplacement où mettre le point est vide  **
** (IsPositionAvailable()). Enfin la méthode ErasePoint() efface les points  **
** mangés par Pac-Man et incrémente le score à chaque point mangé.           **
******************************************************************************/

using System;

namespace PacManSimplifie
{
    public class Points
    {
        /// <summary>
        /// Map constituant le plateau
        /// </summary>
        private Map map = new Map();

        /// <summary>
        /// Caractère du point
        /// </summary>
        private char _SQUARE = '█';

        /// <summary>
        /// Caractère au centre du point
        /// </summary>
        private const char _CENTER = 'C';

        /// <summary>
        /// Caractère en haut du point
        /// </summary>
        private const char _TOP = '1';

        /// <summary>
        /// Caractère à droite du point
        /// </summary>
        private const char _RIGHT = '2';

        /// <summary>
        /// Caractère en bas du point
        /// </summary>
        private const char _BOTTOM = '3';

        /// <summary>
        /// Caractère à gauche du point
        /// </summary>
        private const char _LEFT = '4';

        /// <summary>
        /// Longueur du point
        /// </summary>
        private const byte _LENGTH = 3;

        /// <summary>
        /// Espace entre les points
        /// </summary>
        private const byte _DISTANCE_BETWEEN_POINTS = 4;

        /// <summary>
        /// Tableau des points
        /// </summary>
        public char[,] chrMap;//----------------------------------------

        /// <summary>
        /// Compteur du nombre de points mangés
        /// </summary>
        private byte _bytScore = 0;

        /// <summary>
        /// Nombre de points restants à manger
        /// </summary>
        private byte _bytRemaining = 54;

        /// <summary>
        /// Caractère du point
        /// </summary>
        public char SQUARE
        {
            get
            {
                return _SQUARE;
            }
        }

        /// <summary>
        /// Caractère au centre du point
        /// </summary>
        public char CENTER
        {
            get
            {
                return _CENTER;
            }
        }

        /// <summary>
        /// Caractère en haut du point
        /// </summary>
        public char TOP
        {
            get
            {
                return _TOP;
            }
        }

        /// <summary>
        /// Caractère à droite du point
        /// </summary>
        public char RIGHT
        {
            get
            {
                return _RIGHT;
            }
        }

        /// <summary>
        /// Caractère en bas du point
        /// </summary>
        public char BOTTOM
        {
            get
            {
                return _BOTTOM;
            }
        }

        /// <summary>
        /// Caractère à gauche du point
        /// </summary>
        public char LEFT
        {
            get
            {
                return _LEFT;
            }
        }

        /// <summary>
        /// Compteur du nombre de points mangés
        /// </summary>
        public byte BytScore
        {
            get
            {
                return _bytScore;
            }
            set
            {
                _bytScore = value;
            }
        }

        /// <summary>
        /// Tableau des points
        /// </summary>
        public char[,] ChrMap
        {
            get
            {
                return chrMap;
            }
        }

        /// <summary>
        /// Nombre de points restants à manger
        /// </summary>
        public byte BytRemaining
        {
            get
            {
                return _bytRemaining;
            }
            set
            {
                _bytRemaining = value;
            }
        }

        /// <summary>
        /// Constructeur  
        /// </summary>
        /// <param name="chrMap">Tableau de la classe Map</param>
        public Points(char[,] chrMap)
        {
            this.chrMap = chrMap;
        }

        /// <summary>
        /// Vérifie si un point est à l'intérieur du plateau de jeu
        /// </summary>
        /// <param name="bytRow">La position verticale du point</param>
        /// <param name="bytCol">La position horizontale du point</param>
        /// <returns>True si le point est à l'intérieur du plateau, False sinon</returns>
        private bool IsInsideMap(byte bytRow, byte bytCol)
        {
            // Vérifie si le point n'est pas à l'intérieur des contours du plateau
            if (bytRow < 0 || bytRow >= map.SIDE_LENGTH || bytCol < 0 || bytCol >= map.SIDE_LENGTH)
            {
                return false;
            }
            // Le point est dans les limites du tableau
            else
            {
                return true;
            }
        }// End IsInsideMap()

        /// <summary>
        /// Vérifie que l'emplacement est occupé s'il contient un 'X'
        /// </summary>
        /// <param name="bytRow">La position verticale du point</param>
        /// <param name="bytCol">La position horizontale du point</param>
        /// <returns>True si l'emplacement est occupé par un caractère, false si l'emplacement n'est pas occupé</returns>
        private bool IsPositionAvailable(byte bytRow, byte bytCol)
        {
            // Vérifie s'il y a un mur ou un X dans le tableau pour signifier que le point ne peut pas être à cet emplacement
            if (chrMap[bytRow, bytCol] == map.INSIDE || chrMap[bytRow, bytCol] == map.WALL)
            {
                return false;
            }
            // Emplacement disponible
            else
            {
                return true;
            }

        }// End IsPositionAvailable()

        /// <summary>
        /// Crée un point à partir de la position spécifiée
        /// </summary>
        /// <param name="bytRow">La position verticale du point</param>
        /// <param name="bytCol">La position horizontale du point</param>
        private void CreateAPoint(byte bytRow, byte bytCol)
        {
            ////////////////////////////////////////////////////// VARIABLES /////////////////////////////////////////////////////
            byte bytCenterRow = (byte)(bytRow + 1);     // Position de la ligne du centre du point
            byte bytCenterCol = (byte)(bytCol + 1);     // Position de la colonne du centre du point
            byte bytGap = 1;                            // Décalage entre les points
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Vérifie que les caractères sont à l'intérieur de la carte
            if (IsInsideMap(bytRow, bytCol) && IsInsideMap((byte)(bytRow + bytGap), bytCol) && IsInsideMap(bytRow, (byte)(bytCol + bytGap))
                && IsInsideMap((byte)(bytRow + bytGap), (byte)(bytCol + bytGap)))
            {
                // Vérifie que l'emplacement est disponible
                if ((IsPositionAvailable(bytRow, bytCenterCol) && IsPositionAvailable(bytCenterRow, bytCol) && IsPositionAvailable(bytCenterRow, bytCenterCol)
                    && IsPositionAvailable((byte)(bytCenterRow + bytGap), bytCenterCol) && IsPositionAvailable(bytCenterRow, (byte)(bytCenterCol + bytGap))))
                {
                    // Ajoute les parties constituant le point dans le tableau 
                    chrMap[bytRow, bytCenterCol] = _TOP;
                    chrMap[bytCenterRow, bytCol] = _LEFT;
                    chrMap[bytCenterRow, bytCenterCol] = _CENTER;
                    chrMap[bytCenterRow + 1, bytCenterCol] = _BOTTOM;
                    chrMap[bytCenterRow, bytCenterCol + 1] = _RIGHT;
                }
            }
        }// End private void CreateAPoint()

        /// <summary>
        /// Ajoute tous les points dans le tableau en les espaçant
        /// </summary>
        /// <param name="bytPositionInitial">Position initiale du premier point</param>
        public void DistributePointInTheTab(byte bytPositionInitial)
        {
            ////////////////////////////////////////////////////// VARIABLES /////////////////////////////////////////////////////
            byte bytSpacing = (byte)(_LENGTH + _DISTANCE_BETWEEN_POINTS);     // Ecart entre les points
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Parcourt le tableau pour créer et disposer les points à distance égale
            for (byte i = 0; i < chrMap.GetLength(0); i += bytSpacing)
            {
                for (byte j = 0; j < chrMap.GetLength(1); j += bytSpacing)
                {
                    CreateAPoint((byte)(bytPositionInitial + i), (byte)(bytPositionInitial + j));
                }
            }
        }// End public void DistributePointInTheTab()

        /// <summary>
        /// Efface l'affichage d'une figure (Pac-Man ou fantôme) à sa position actuelle et rédessine les points pas encore mangés après le passage de la figure
        /// </summary>
        /// <param name="posFigure">La position actuelle de la figure pas encore mise à jour après son déplacement</param>
        public void EraseAndRedraw(Position posFigure, Byte bytFigureLength)
        {
            // Parcourt la figure dans sa hauteur
            for (byte i = 0; i < bytFigureLength; i++)
            {
                // Place le curseur en début de ligne
                Console.SetCursorPosition(posFigure.X, posFigure.Y + i);

                // Parcourt la figure dans sa largeur
                for (byte j = 0; j < bytFigureLength; j++)
                {
                    // Colore en blanc et affiche les points s'il y en avait, sinon affiche du vide
                    if (chrMap[posFigure.Y + i, posFigure.X + j] == _TOP || chrMap[posFigure.Y + i, posFigure.X + j] == _RIGHT || chrMap[posFigure.Y + i, posFigure.X + j] == _BOTTOM || chrMap[posFigure.Y + i, posFigure.X + j] == _LEFT)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(_SQUARE);
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
            }
        }// End public void EraseAndRedraw()
    }
}

