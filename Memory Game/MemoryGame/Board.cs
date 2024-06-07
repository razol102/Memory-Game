using System;
using System.Collections.Generic;


namespace MemoryGame
{
    public class Board
    {
        private const char k_EmptyCellSign = ' ';
        private const char k_LastLetterInEngishABC = 'Z';
        private short m_BoardRows, m_BoardColumns;
        private const string k_Rows = "Rows";
        private const string k_Columns = "Columns";
        private Cell[,] m_Board;
        private Dictionary<char, Cell> m_ShownCellsMemoryAI;

        internal Board(short i_BoardRows, short i_BoardColumns)
        {
            m_ShownCellsMemoryAI = new Dictionary<char, Cell>();
            m_BoardRows = i_BoardRows;
            m_BoardColumns = i_BoardColumns;
            m_Board = new Cell[m_BoardRows, m_BoardColumns];
            initGameBoard();
        }

        public Cell[,] CellBoard
        {
            get
            {
                return m_Board;
            }
        }
        
        public Dictionary<char, Cell> ShownCellsMemory
        {
            get
            {
                return m_ShownCellsMemoryAI;
            }
        }

        public short Rows
        {
            get
            {
                return m_BoardRows;
            }
        }

        public short Columns
        {
            get
            {
                return m_BoardColumns;
            }
        }

        private void initGameBoard()
        {
            int gameBoardSize = m_BoardRows * m_BoardColumns;

            SetGameBoardToEmptyCells();
            for (short i = 0; i < gameBoardSize / 2; i++)
            {
                SetLetterInEmptyCellInBoard(i);
                SetLetterInEmptyCellInBoard(i);
            }
        }

        internal void SetLetterInEmptyCellInBoard(short i_LetterOffset)
        {
            int     randomRowCellInRange = 0, randomColumnCellInRange = 0;
            Random  random = new Random();

            do
            {
                randomRowCellInRange = random.Next(0, m_BoardRows);
                randomColumnCellInRange = random.Next(0, m_BoardColumns);
            } while (m_Board[randomRowCellInRange, randomColumnCellInRange].Letter != k_EmptyCellSign);

            m_Board[randomRowCellInRange, randomColumnCellInRange].Letter = (char)(k_LastLetterInEngishABC - i_LetterOffset);
        }

        public Cell GetRandomEmptyCellInBoard()
        {
            int     randomRowCellInRange = 0, randomColumnCellInRange = 0;
            Random  random = new Random();

            do
            {
                randomRowCellInRange = random.Next(0, m_BoardRows);
                randomColumnCellInRange = random.Next(0, m_BoardColumns);
            } while (m_Board[randomRowCellInRange, randomColumnCellInRange].IsShown);

            return m_Board[randomRowCellInRange, randomColumnCellInRange];
        }

        internal void SetGameBoardToEmptyCells()
        {
            for (short i = 0; i < m_BoardRows; i++)
            {
                for (short j = 0; j < m_BoardColumns; j++)
                {
                    Cell boardIJ = new Cell(k_EmptyCellSign, i, j);
                    m_Board[i, j] = boardIJ;
                }
            }
        }

        internal bool SetChosenCellToBeShown(Cell i_ChosenCell)
        {
            bool validToShow = isValidCellToShow(i_ChosenCell);

            if (validToShow)
            {
                m_Board[i_ChosenCell.Row, i_ChosenCell.Column].IsShown = true;
            }

            return validToShow;
        }

        public bool IsBoardFullyShown()
        {
            bool fullyShown = true;

            for (short row = 0; row < m_BoardRows; row++)
            {
                for (short column = 0; column < m_BoardColumns; column++)
                {
                    if (!m_Board[row, column].IsShown)
                    {
                        fullyShown = false;
                        break;
                    }
                }
            }

            return fullyShown;
        }

        private bool isValidCellToShow(Cell i_ChosenCell)
        {
            return isValidCoordinateValue(i_ChosenCell.Row, k_Rows) && 
                isValidCoordinateValue(i_ChosenCell.Column, k_Columns) &&
                !i_ChosenCell.IsShown;
        }

        private bool isValidCoordinateValue(short i_Value, string i_BoardAxis)
        {
            bool isValidCoordinate = false;

            if (i_BoardAxis == k_Rows)
            {
                isValidCoordinate = i_Value >= 0 && i_Value < this.Rows;
            }
            else
            {
                isValidCoordinate = i_Value >= 0 && i_Value < this.Columns;
            }

            return isValidCoordinate;
        }

        public bool IsRowInRange(string i_PlayerRowInput)
        {
            bool isRowInValidRange = false;

            if (short.TryParse(i_PlayerRowInput, out short chosenRow))
            {
                if (chosenRow >= 1 && chosenRow <= m_BoardRows)
                {
                    isRowInValidRange = true;
                }
            }

            return isRowInValidRange;
        }

        public Cell GetSmartEmptyCell(Cell i_FirstCellChoice)
        {
            Cell smartEmptyCell = new Cell();

            if (m_ShownCellsMemoryAI.ContainsKey(i_FirstCellChoice.Letter)
                && (!i_FirstCellChoice.AreSameCellsInBoard(m_ShownCellsMemoryAI[i_FirstCellChoice.Letter]))
                && (!m_ShownCellsMemoryAI[i_FirstCellChoice.Letter].IsShown))
                {
                    smartEmptyCell = m_ShownCellsMemoryAI[i_FirstCellChoice.Letter];
                }
            else
            {
                smartEmptyCell = GetRandomEmptyCellInBoard();
            }

            return smartEmptyCell;
        }
    }
}
