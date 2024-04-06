/******************************************************************************
** PROGRAMME  PacManSimplifie.cs                                             **
**                                                                           **
** Lieu      : ETML - section informatique                                   **
** Auteur    : ABDULKADIR Salma                                              **
** Date      : 01.05.2023                                                    **
**                                                                           **
** Modifications                                                             **
**   Auteur  :                                                               **
**   Version :                                                               **
**   Date    :                                                               **
**   Raisons :                                                               **
**                                                                           **
**                                                                           **
******************************************************************************/

/******************************************************************************
** DESCRIPTION                                                               **
** Classe Position qui permet de définir la position x et y des objets dans  **
** la console.                                                               **
******************************************************************************/

namespace PacManSimplifie
{
    public class Position
    {
        /// <summary>
        /// Colonne de la console
        /// </summary>
        private byte _x;

        /// <summary>
        /// Ligne de la console
        /// </summary>
        private byte _y;

        /// <summary>
        /// Colonne de la console
        /// </summary>
        public byte X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        /// <summary>
        /// Ligne de la console
        /// </summary>
        public byte Y
        {
            get 
            { 
               return _y;
            }
            set
            {
                _y = value;
            }
        }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="x">Colonne de la console</param>
        /// <param name="y">Ligne de la console</param>
        public Position(byte x, byte y)
        {
            _x = x;
            _y = y;
        }
    }
}