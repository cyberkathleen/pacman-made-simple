/******************************************************************************
** PROGRAMME  PacManSimplifie.cs                                             **
**                                                                           **
** Lieu      : ETML - section informatique                                   **                                
** Auteur    : ABID Fatima                                                   **
** Date      : 01.05.2023                                                    **

** Modifications                                                             **
**   Auteur  : LU Kathleen                                                   **
**   Version : 1.6                                                           **
**   Date    : 01.01.2024                                                    **
**   Raisons : Modification de la manière d'afficher les points avec CENTER, **
**             TOP, RIGHT, BOTTOM ET LEFT.                                   **
**                                                                           **
** Modifications                                                             **
**   Auteur  : ABID Fatima                                                   **
**   Version : 1.2                                                           **
**   Date    : 27.05.2023                                                    **
**   Raisons : Optimisation du code (déplacement des méthodes DisplayScore() **
**             et DisplayPacManLives() dans la classe Game)                  **
**                                                                           **
******************************************************************************/

/******************************************************************************
** DESCRIPTION                                                               **
** Création et gestion du plateau de jeu avec un tableau. Dans ce tableau    **
** sont stockés des caractères pour créer les contours du plateau, et 9      **
** blocs de formes différentes. La méthode CreateMap() ajoute tous les       **
** composants nécessaires au plateau de jeu, et la méthode DisplayMap()      **
** permet d'afficher le plateau.                                             **
******************************************************************************/

using System;

namespace PacManSimplifie
{
    public class Map
    {
        /// <summary>
        /// Longueur de côté du plateau de jeu
        /// </summary>
        private const byte _SIDE_LENGTH = 65;

        /// <summary>
        /// Caractère pour dessiner les murs des blocs, du plateau et les points
        /// </summary>
        private const char _SQUARE = '█';

        /// <summary>
        /// Caractère pour représenter les murs du plateau et des blocs l
        /// </summary>
        private const char _WALL = 'W';

        /// <summary>
        /// Caractère pour représenter les tunnels du plateau
        /// </summary>
        private const char _TUNNEL = 'T';

        /// <summary>
        /// Caractère pour remplir le plateau d'espace
        /// </summary>
        private const char _SPACE = ' ';

        /// <summary>
        /// Caractère pour remplir l'intérieur des blocs pour qu'ils ne contiennent pas de points
        /// </summary>
        private const char _INSIDE = 'I';

        /// <summary>
        /// Création et initialisation du tableau de caractères du plateau de jeu
        /// </summary>
        private char[,] _chrMap;

        /// <summary>
        /// Longueur et largeur du plateau
        /// </summary>
        public byte SIDE_LENGTH
        {
            get
            {
                return _SIDE_LENGTH;
            }
        }

        /// <summary>
        /// Caractère composant un mur
        /// </summary>
        public char WALL
        {
            get
            {
                return _WALL;
            }
        }

        /// <summary>
        /// Caractère à afficher pour représenter des lignes
        /// </summary>
        public char SQUARE
        {
            get
            {
                return _SQUARE;
            }
        }

        /// <summary>
        /// Caractère pour remplir l'intérieur des blocs du plateau
        /// </summary>
        public char INSIDE
        {
            get
            {
                return _INSIDE;
            }

        }

        /// <summary>
        /// Caractère pour mettre des espaces dans le plateau
        /// </summary>
        public char SPACE
        {
            get
            {
                return _SPACE;
            }
        }

        /// <summary>
        /// Tableau du plateau de jeu
        /// </summary>
        public char[,] chrMap
        {
            get
            {
                return _chrMap;
            }
        }

        /// <summary>
        /// Constructeur 
        /// </summary>
        public Map()
        {
            // Initialise le tableau du plateau de jeu
            _chrMap = new char[_SIDE_LENGTH, _SIDE_LENGTH];

            // Appel de la méthode qui crée le plateau de jeu complet
            CreateCompleteMap();
        }

        /// <summary>
        /// Initialise le tableau du plateau avec ses contours, et ses blocs 
        /// </summary>
        private void CreateCompleteMap()
        {
            ///////////////////////////////////////////////////////////// CONSTANTES //////////////////////////////////////////////////////////////////
            const byte TUNNEL_START = 29;               // Ligne où le tunnel commence
            const byte TUNNEL_END = TUNNEL_START + 7;   // Ligne où le tunnel se termine
            ///////////////////////////////////////////////////////////// VARIABLES //////////////////////////////////////////////////////////////////
            byte i = 0;     //Indice pour parcourir les lignes du tableau
            byte j = 0;     //Indice pour parcourir les colonnes du tableau
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Remplit le tableau d'espaces 
            for (i = 0; i < _SIDE_LENGTH; i++)
            {
                for (j = 0; j < _SIDE_LENGTH; j++)
                {
                    _chrMap[i, j] = _SPACE;
                }
            }

            //Crée les contours horizontaux du plateau
            for (i = 0; i < _SIDE_LENGTH; i++)
            {
                _chrMap[0, i] = _WALL;
                _chrMap[_SIDE_LENGTH - 1, i] = _WALL;
            }

            //Crée les contours verticaux du plateau
            for (i = 0; i < _SIDE_LENGTH; i++)
            {
                _chrMap[i, 0] = _WALL;
                _chrMap[i, _SIDE_LENGTH - 1] = _WALL;
            }

            // Ajoute l'emplacement des tunnels
            for (i = TUNNEL_START; i < TUNNEL_END; i++)
            {
                _chrMap[i, 0] = _TUNNEL;
                _chrMap[i, _SIDE_LENGTH - 1] = _TUNNEL;
            }

            // Crée les blocs en L dans la partie haute du plateau
            CreateLBlock(8, 8, true, true);
            CreateLBlock(8, 36, false, true);

            // Crée les blocs rectangulaires dans la partie haute du plateau
            CreateRectanglesSideBlock(22, 0, true);
            CreateRectanglesSideBlock(22, 50, false);

            // Crée un bloc au centre du plateau
            CreateCentralBlock(29, 22);

            // Crée les blocs rectangulaires dans la partie basse du plateau
            CreateRectanglesSideBlock(36, 0, true);
            CreateRectanglesSideBlock(36, 50, false);

            // Crée les blocs en L dans la partie basse du plateau
            CreateLBlock(43, 8, true, false);
            CreateLBlock(43, 36, false, false);
        }

        /// <summary>
        /// Ajoute 4 blocs en L dans le tableau du plateau
        /// </summary>
        /// <param name="bytInitialRow">Position de la ligne de départ</param>
        /// <param name="bytInitialCol">Position de la colonne de départ</param>
        /// <param name="boolLeftSide">Confirme si le L à dessiner est à gauche (true) ou à droite (false)</param>
        /// <param name="boolUpperSide">Confirme si le L à dessiner est dans la partie supérieure du plateau (true) ou dans la partie inférieure (false)</param>
        public void CreateLBlock(byte bytInitialRow, byte bytInitialCol, bool boolLeftSide = true, bool boolUpperSide = true)
        {
            ///////////////////////////////////////////////////////////// CONSTANTES //////////////////////////////////////////////////////////////////
            const byte LONGER_HORIZONTAL_LENGTH = 20;         // Longueur de la plus grande ligne horizontale  
            const byte MIDDLE_HORIZONTAL_LENGTH = 13;         // Longueur de la ligne horizontale intermédiaire 
            const byte SHORTER_HORIZONTAL_LENGTH = 6;         // Longueur de la plus courte ligne horizontale  
            const byte LONGER_VERTICAL_LENGTH = 13;           // Longueur de la plus grande ligne verticale 
            const byte SHORTER_VERTICAL_LENGTH = 6;           // Longueur des petites lignes verticales
            const byte GAP = 1;                               // Ecart à l'angle du L
            const byte GAP_X = 1;                             //Ecart pour que les X soient uniquement à l'intérieur des blocs 
            ///////////////////////////////////////////////////////////// VARIABLES //////////////////////////////////////////////////////////////////
            byte i = 0;                                      //Indice pour parcourir les lignes du tableau
            byte j = 0;                                      //Indice pour parcourir les colonnes du tableau
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Vérifie que le L à créer est dans la partie supérieure du plateau
            if (boolUpperSide)
            {
                // Vérifie que le L à créer est à gauche
                if (boolLeftSide)
                {
                    // Crée la ligne horizontale du haut
                    for (i = bytInitialCol; i < bytInitialCol + LONGER_HORIZONTAL_LENGTH; i++)
                    {
                        _chrMap[bytInitialRow, i] = _WALL;
                    }

                    // Crée la ligne horizontale du milieu
                    for (i = bytInitialCol; i < bytInitialCol + MIDDLE_HORIZONTAL_LENGTH; i++)
                    {
                        _chrMap[bytInitialRow + SHORTER_VERTICAL_LENGTH, i + GAP] = _WALL;
                    }

                    // Crée la ligne horizontale du bas
                    for (i = bytInitialCol; i < bytInitialCol + SHORTER_HORIZONTAL_LENGTH + 1; i++)
                    {
                        _chrMap[bytInitialRow + LONGER_VERTICAL_LENGTH, i + MIDDLE_HORIZONTAL_LENGTH + GAP] = _WALL;
                    }

                    // Crée la ligne verticale de droite
                    for (i = bytInitialRow; i < bytInitialRow + LONGER_VERTICAL_LENGTH; i++)
                    {
                        _chrMap[i, bytInitialCol + LONGER_HORIZONTAL_LENGTH] = _WALL;
                    }

                    // Crée les lignes verticales de gauche
                    for (i = bytInitialRow; i < bytInitialRow + SHORTER_VERTICAL_LENGTH; i++)
                    {
                        // Crée la ligne du haut 
                        _chrMap[i + GAP, bytInitialCol] = _WALL;
                        // Crée la ligne du bas
                        _chrMap[i + SHORTER_VERTICAL_LENGTH + GAP, bytInitialCol + MIDDLE_HORIZONTAL_LENGTH + GAP] = _WALL;
                    }

                    // Crée les 'X' à l'intérieur du L dans la partie haute
                    for (i = (byte)(bytInitialRow + GAP_X); i < bytInitialRow + SHORTER_VERTICAL_LENGTH + GAP_X; i++)
                    {
                        for (j = (byte)(bytInitialCol + GAP_X); j < bytInitialCol + LONGER_HORIZONTAL_LENGTH; j++)
                        {
                            // Vérifie que l'emplacement est vide et ne comporte pas de murs pour y ajouter un 'X'
                            if (_chrMap[i, j] != _WALL || chrMap[i, j] == _SPACE)
                            {
                                _chrMap[i, j] = _INSIDE;
                            }
                        }
                    }

                    // Crée les 'X' à l'intérieur du L dans la partie basse
                    for (i = (byte)(bytInitialRow + SHORTER_VERTICAL_LENGTH + GAP_X); i < bytInitialRow + LONGER_VERTICAL_LENGTH; i++)
                    {
                        for (j = (byte)(bytInitialCol + MIDDLE_HORIZONTAL_LENGTH + GAP_X); j < bytInitialCol + LONGER_HORIZONTAL_LENGTH; j++)
                        {
                            // Vérifie que l'emplacement est vide et ne comporte pas de murs pour y ajouter un 'X'
                            if (_chrMap[i, j] != _WALL || chrMap[i, j] == _SPACE)
                            {
                                _chrMap[i, j] = _INSIDE;
                            }
                        }
                    }
                }
                // Crée le L de droite
                else
                {
                    // Crée la ligne horizontale du haut
                    for (i = bytInitialCol; i < bytInitialCol + LONGER_HORIZONTAL_LENGTH; i++)
                    {
                        _chrMap[bytInitialRow, i] = _WALL;
                    }

                    // Crée la ligne horizontale du milieu  
                    for (i = bytInitialCol; i < bytInitialCol + MIDDLE_HORIZONTAL_LENGTH + GAP; i++)
                    {
                        _chrMap[bytInitialRow + SHORTER_VERTICAL_LENGTH, i + SHORTER_HORIZONTAL_LENGTH + GAP] = _WALL;
                    }

                    // Crée la ligne horizontale du bas
                    for (i = bytInitialCol; i < bytInitialCol + SHORTER_HORIZONTAL_LENGTH + GAP; i++)
                    {
                        _chrMap[bytInitialRow + LONGER_VERTICAL_LENGTH, i] = _WALL;
                    }

                    // Crée la ligne verticale de gauche
                    for (i = bytInitialRow; i < bytInitialRow + LONGER_VERTICAL_LENGTH; i++)
                    {
                        _chrMap[i, bytInitialCol] = _WALL;
                    }

                    // Crée les lignes verticales de droite 
                    for (i = bytInitialRow; i < bytInitialRow + SHORTER_VERTICAL_LENGTH; i++)
                    {
                        // Dessine la ligne du haut
                        _chrMap[i, bytInitialCol + LONGER_HORIZONTAL_LENGTH] = _WALL;
                        // Dessine la ligne du bas
                        _chrMap[i + SHORTER_VERTICAL_LENGTH + GAP, bytInitialCol + SHORTER_VERTICAL_LENGTH] = _WALL;
                    }

                    // Crée les 'X' à l'intérieur du L dans la partie haute
                    for (i = (byte)(bytInitialRow + GAP_X); i < bytInitialRow + SHORTER_VERTICAL_LENGTH + GAP_X; i++)
                    {
                        for (j = (byte)(bytInitialCol + GAP_X); j < bytInitialCol + LONGER_HORIZONTAL_LENGTH; j++)
                        {
                            // Vérifie que l'emplacement est vide et ne comporte pas de murs pour y ajouter un 'X'
                            if (_chrMap[i, j] != _WALL || chrMap[i, j] == _SPACE)
                            {
                                _chrMap[i, j] = _INSIDE;
                            }
                        }
                    }

                    // Crée les 'X' à l'intérieur du L dans la partie basse
                    for (i = (byte)(bytInitialRow + SHORTER_VERTICAL_LENGTH + GAP_X); i < bytInitialRow + LONGER_VERTICAL_LENGTH; i++)
                    {
                        for (j = (byte)(bytInitialCol + GAP_X); j < bytInitialCol + SHORTER_HORIZONTAL_LENGTH; j++)
                        {
                            // Vérifie que l'emplacement est vide et ne comporte pas de murs pour y ajouter un 'X'
                            if (_chrMap[i, j] != _WALL || chrMap[i, j] == _SPACE)
                            {
                                _chrMap[i, j] = _INSIDE;
                            }
                        }
                    }
                }
            }
            // Crée les L situés dans la partie inférieure du plateau
            else
            {
                // Crée le L de gauche
                if (boolLeftSide)
                {
                    // Crée la ligne horizontale du haut
                    for (i = bytInitialCol; i < bytInitialCol + SHORTER_HORIZONTAL_LENGTH; i++)
                    {
                        _chrMap[bytInitialRow, i + MIDDLE_HORIZONTAL_LENGTH + GAP] = _WALL;
                    }

                    // Crée la ligne horizontale du milieu
                    for (i = bytInitialCol; i < bytInitialCol + MIDDLE_HORIZONTAL_LENGTH; i++)
                    {
                        _chrMap[bytInitialRow + SHORTER_VERTICAL_LENGTH + GAP, i + GAP] = _WALL;
                    }

                    // Crée la ligne horizontale du bas
                    for (i = bytInitialCol; i < bytInitialCol + LONGER_HORIZONTAL_LENGTH + GAP; i++)
                    {
                        _chrMap[bytInitialRow + LONGER_VERTICAL_LENGTH, i] = _WALL;
                    }

                    // Crée la ligne verticale de droite
                    for (i = bytInitialRow; i < bytInitialRow + LONGER_VERTICAL_LENGTH; i++)
                    {
                        _chrMap[i, bytInitialCol + LONGER_HORIZONTAL_LENGTH] = _WALL;
                    }

                    // Crée les lignes verticales de gauche
                    for (i = bytInitialRow; i < bytInitialRow + SHORTER_VERTICAL_LENGTH; i++)
                    {
                        // Crée la ligne du haut 
                        _chrMap[i + GAP, bytInitialCol + MIDDLE_HORIZONTAL_LENGTH + GAP] = _WALL;
                        // Crée la ligne du bas
                        _chrMap[i + SHORTER_VERTICAL_LENGTH + GAP, bytInitialCol] = _WALL;
                    }

                    // Crée les 'X' à l'intérieur du L dans la partie haute
                    for (i = (byte)(bytInitialRow + GAP_X + SHORTER_VERTICAL_LENGTH); i < bytInitialRow + LONGER_VERTICAL_LENGTH; i++)
                    {
                        for (j = (byte)(bytInitialCol + GAP_X); j < bytInitialCol + LONGER_HORIZONTAL_LENGTH; j++)
                        {
                            //Vérifie que l'emplacement est vide et ne comporte pas de murs pour y ajouter un 'X'
                            if (_chrMap[i, j] != _WALL || chrMap[i, j] == _SPACE)
                            {
                                _chrMap[i, j] = _INSIDE;
                            }
                        }
                    }

                    // Crée les 'X' à l'intérieur du L dans la partie basse
                    for (i = (byte)(bytInitialRow + GAP_X); i < bytInitialRow + LONGER_VERTICAL_LENGTH; i++)
                    {
                        for (j = (byte)(bytInitialCol + MIDDLE_HORIZONTAL_LENGTH + GAP_X); j < bytInitialCol + LONGER_HORIZONTAL_LENGTH; j++)
                        {
                            //Vérifie que l'emplacement est vide et ne comporte pas de murs pour y ajouter un 'X'
                            if (_chrMap[i, j] != _WALL || chrMap[i, j] == _SPACE)
                            {
                                _chrMap[i, j] = _INSIDE;
                            }
                        }
                    }
                }
                //Crée le L de droite
                else
                {
                    // Crée la ligne horizontale du haut
                    for (i = bytInitialCol; i < bytInitialCol + SHORTER_HORIZONTAL_LENGTH; i++)
                    {
                        _chrMap[bytInitialRow, i] = _WALL;
                    }

                    // Crée la ligne horizontale du milieu
                    for (i = bytInitialCol; i < bytInitialCol + MIDDLE_HORIZONTAL_LENGTH; i++)
                    {
                        _chrMap[bytInitialRow + SHORTER_VERTICAL_LENGTH + GAP, i + SHORTER_HORIZONTAL_LENGTH + GAP] = _WALL;
                    }

                    // Crée la ligne horizontale du bas
                    for (i = bytInitialCol; i < bytInitialCol + LONGER_HORIZONTAL_LENGTH; i++)
                    {
                        _chrMap[bytInitialRow + LONGER_VERTICAL_LENGTH, i] = _WALL;
                    }

                    // Crée la ligne verticale de gauche
                    for (i = bytInitialRow; i < bytInitialRow + LONGER_VERTICAL_LENGTH; i++)
                    {
                        _chrMap[i, bytInitialCol] = _WALL;
                    }

                    // Crée les lignes verticales de droite
                    for (i = bytInitialRow; i < bytInitialRow + SHORTER_VERTICAL_LENGTH + GAP; i++)
                    {
                        // Crée la ligne du haut 
                        _chrMap[i, bytInitialCol + SHORTER_HORIZONTAL_LENGTH] = _WALL;
                        // Crée la ligne du bas
                        _chrMap[i + SHORTER_VERTICAL_LENGTH + GAP, bytInitialCol + LONGER_HORIZONTAL_LENGTH] = _WALL;
                    }

                    // Crée les 'X' à l'intérieur du L dans la partie haute
                    for (i = (byte)(bytInitialRow + GAP_X + SHORTER_VERTICAL_LENGTH); i < bytInitialRow + LONGER_VERTICAL_LENGTH; i++)
                    {
                        for (j = (byte)(bytInitialCol + GAP_X); j < bytInitialCol + LONGER_HORIZONTAL_LENGTH; j++)
                        {
                            //Vérifie que l'emplacement est vide et ne comporte pas de murs pour y ajouter un 'X'
                            if (_chrMap[i, j] != _WALL || chrMap[i, j] == _SPACE)
                            {
                                _chrMap[i, j] = _INSIDE;
                            }
                        }
                    }

                    // Crée les 'X' à l'intérieur du L dans la partie basse
                    for (i = (byte)(bytInitialRow + GAP_X); i < bytInitialRow + LONGER_VERTICAL_LENGTH; i++)
                    {
                        for (j = (byte)(bytInitialCol + GAP_X); j < bytInitialCol + SHORTER_HORIZONTAL_LENGTH; j++)
                        {
                            //Vérifie que l'emplacement est vide et ne comporte pas de murs pour y ajouter un 'X'
                            if (_chrMap[i, j] != _WALL || chrMap[i, j] == _SPACE)
                            {
                                _chrMap[i, j] = _INSIDE;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ajoute 4 blocs rectangulaires dans le tableau du plateau
        /// </summary>
        /// <param name="bytInitialRow">Position de la ligne de départ du dessin</param>
        /// <param name="bytInitialCol">Position de la colonne de départ du dessin</param>
        /// <param name="boolUpperSide">Confirme si le L à dessiner est dans la partie supérieure du plateau (false) ou dans la partie inférieure (true)</param>
        public void CreateRectanglesSideBlock(byte bytInitialRow, byte bytInitialCol, bool boolUpperSide = false)
        {
            /////////////////////////////////////////////////////////////CONSTANTES/////////////////////////////////////////////////////////////////
            const byte WIDTH = 14;        //Largeur du bloc
            const byte HEIGHT = 7;        //Longueur du bloc  
            const byte GAP_X = 1;         //Ecart pour que les X soient uniquement à l'intérieur des blocs 
            /////////////////////////////////////////////////////////////VARIABLES//////////////////////////////////////////////////////////////////
            byte i = 0;                   //Indice pour parcourir les lignes du tableau
            byte j = 0;                   //Indice pour parcourir les colonnes du tableau
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Vérifie que les blocs à dessiner sont dans la partie supérieure du plateau
            if (boolUpperSide)
            {
                //Crée la ligne horizontale du haut
                for (i = bytInitialCol; i < bytInitialCol + WIDTH; i++)
                {
                    _chrMap[bytInitialRow, i] = _WALL;
                }

                //Crée la ligne horizontale du bas
                for (i = bytInitialCol; i < bytInitialCol + WIDTH; i++)
                {
                    _chrMap[bytInitialRow + HEIGHT - 1, i] = _WALL;
                }

                //Crée la ligne verticale de droite
                for (i = bytInitialRow; i < bytInitialRow + HEIGHT; i++)
                {
                    _chrMap[i, bytInitialCol + WIDTH] = _WALL;
                }

                // Crée les 'X' à l'intérieur du bloc 
                for (i = (byte)(bytInitialRow + GAP_X); i < bytInitialRow + HEIGHT - GAP_X; i++)
                {
                    for (j = (byte)(bytInitialCol + GAP_X); j < bytInitialCol + WIDTH; j++)
                    {
                        //Vérifie que l'emplacement est vide pour y ajouter un 'X'
                        if (_chrMap[i, j] == _SPACE)
                        {
                            _chrMap[i, j] = _INSIDE;
                        }
                    }
                }
            }
            //Crée les blocs de la partie inférieure du plateau
            else
            {
                //Crée la ligne horizontale du haut
                for (i = bytInitialCol; i < bytInitialCol + WIDTH; i++)
                {
                    _chrMap[bytInitialRow, i] = _WALL;
                }

                //Crée la ligne horizontale du bas
                for (i = bytInitialCol; i < bytInitialCol + WIDTH; i++)
                {
                    _chrMap[bytInitialRow + HEIGHT - 1, i] = _WALL;
                }

                //Crée la ligne verticale de gauche
                for (i = bytInitialRow; i < bytInitialRow + HEIGHT; i++)
                {
                    _chrMap[i, bytInitialCol] = _WALL;
                }

                //Crée les 'X' à l'intérieur du bloc 
                for (i = (byte)(bytInitialRow + GAP_X); i < bytInitialRow + HEIGHT - GAP_X; i++)
                {
                    for (j = (byte)(bytInitialCol + GAP_X); j < bytInitialCol + WIDTH; j++)
                    {
                        //Vérifie que l'emplacement est vide pour y ajouter un X
                        if (_chrMap[i, j] == _SPACE)
                        {
                            _chrMap[i, j] = _INSIDE;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ajoute le bloc rectangulaire au centre du plateau
        /// </summary>
        /// <param name="bytInitialRow">Position de la ligne de départ du dessin</param>
        /// <param name="bytInitialCol">Position de la colonne de départ du dessin</param>
        public void CreateCentralBlock(byte bytInitialRow, byte bytInitialCol)
        {
            /////////////////////////////////////////////////////////////CONSTANTES/////////////////////////////////////////////////////////////////
            const byte BLOCK_WIDTH = 20;        //Largeur du bloc
            const byte BLOCK_HEIGHT = 6;        //Longueur du bloc
            const byte GAP_X = 1;               //Ecart pour que les X soient uniquement à l'intérieur des blocs 
            /////////////////////////////////////////////////////////////VARIABLES//////////////////////////////////////////////////////////////////
            byte i = 0;                         //Indice pour parcourir les lignes du tableau
            byte j = 0;                         //Indice pour parcourir les colonnes du tableau
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Crée la ligne horizontale du haut
            for (i = bytInitialCol; i < bytInitialCol + BLOCK_WIDTH; i++)
            {
                _chrMap[bytInitialRow, i] = _WALL;
            }

            // Crée la ligne horizontale du bas
            for (i = bytInitialCol; i < bytInitialCol + BLOCK_WIDTH; i++)
            {
                _chrMap[bytInitialRow + BLOCK_HEIGHT, i] = _WALL;
            }

            // Crée la ligne verticale de gauche
            for (i = bytInitialRow; i < bytInitialRow + BLOCK_HEIGHT; i++)
            {
                _chrMap[i, bytInitialCol] = _WALL;
            }

            // Crée la ligne verticale de droite
            for (i = bytInitialRow; i < bytInitialRow + BLOCK_HEIGHT + 1; i++)
            {
                _chrMap[i, bytInitialCol + BLOCK_WIDTH] = _WALL;
            }

            // Crée les 'X' à l'intérieur du bloc 
            for (i = (byte)(bytInitialRow + GAP_X); i < bytInitialRow + BLOCK_HEIGHT; i++)
            {
                for (j = (byte)(bytInitialCol + GAP_X); j < bytInitialCol + BLOCK_WIDTH; j++)
                {
                    //Vérifie que l'emplacement est vide pour y ajouter un X
                    if (_chrMap[i, j] == _SPACE)
                    {
                        _chrMap[i, j] = _INSIDE;
                    }
                }
            }
        }

        /// <summary>
        /// Affiche le plateau et colorie les éléments du tableau
        /// </summary>
        public void DisplayMap()
        {
            /////////////////////////////////////////////////////////////CONSTANTES/////////////////////////////////////////////////////////////////
            const byte TOP_LINE = 0;            // Position de la ligne horizontale du haut
            const byte BOTTOM_LINE = 64;        // Position de la ligne horizontale du haut
            const byte LEFT_LINE = 0;           // Position de la ligne verticale à gauche 
            const byte RIGHT_LINE = 64;         // Position de la ligne verticale à droite  
            /////////////////////////////////////////////////////////////VARIABLES//////////////////////////////////////////////////////////////////
            byte i = 0;                                 // Indice pour parcourir les lignes du tableau
            byte j = 0;                                 // Indice pour parcourir les colonnes du tableau
            ConsoleColor grey = ConsoleColor.Gray;      // Colore en gris l'élément concerné
            ConsoleColor black = ConsoleColor.Black;    // Colore en noir l'élément concerné
            ConsoleColor white = ConsoleColor.White;    // Colore en blanc l'élément concerné
            ConsoleColor blue = ConsoleColor.Blue;      // Colore en gris l'élément concerné
            Points myPoints = new Points(_chrMap);      // Les points
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Parcourt le tableau de caractères composant le plateau
            for (i = 0; i < _SIDE_LENGTH; i++)
            {
                for (j = 0; j < _SIDE_LENGTH; j++)
                {
                    // Colore en noir l'intérieur des blocs et des points pour qu'ils se fondent dans la console
                    if (_chrMap[i, j] == _INSIDE || _chrMap[i, j] == myPoints.CENTER)
                    {
                        Console.ForegroundColor = black;
                    }
                    // Colore en gris les contours du plateau
                    else if (i == TOP_LINE || i == BOTTOM_LINE || j == LEFT_LINE || j == RIGHT_LINE)
                    {
                        Console.ForegroundColor = grey;
                    }
                    // Colore en blanc les points
                    else if (_chrMap[i, j] == myPoints.TOP || _chrMap[i, j] == myPoints.RIGHT || _chrMap[i, j] == myPoints.BOTTOM || _chrMap[i, j] == myPoints.LEFT)
                    {
                        Console.ForegroundColor = white;
                    }
                    // Colore en bleu les murs des blocs
                    else if (_chrMap[i, j] == _WALL)
                    {
                        Console.ForegroundColor = blue;
                    }

                    // Vérifie que le caractère est un mur ou un tunnel ou un point pour l'afficher comme un carré
                    if (_chrMap[i, j] == _WALL || _chrMap[i, j] == _TUNNEL || _chrMap[i, j] == myPoints.TOP || _chrMap[i, j] == myPoints.RIGHT || _chrMap[i, j] == myPoints.BOTTOM || _chrMap[i, j] == myPoints.LEFT)
                    {
                        Console.Write(_SQUARE);
                    }
                    else
                    {
                        Console.Write(_chrMap[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
