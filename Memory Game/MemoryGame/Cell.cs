using System;

namespace MemoryGame
{
    public struct Cell
    {
        public char m_Letter;
        public short m_Row;
        public short m_Column;
        public bool m_IsShown;

        public Cell(char i_Letter, short i_Row, short i_Column)
        {
            m_Letter = i_Letter;
            m_Row = i_Row;
            m_Column = i_Column;
            m_IsShown = false;
        }

        public char Letter
        {
            get
            {
                return m_Letter;
            }
            set
            {
                m_Letter = value;
            }
        }

        public short Row
        {
            get
            {
                return m_Row;
            }
        }

        public short Column
        {
            get
            {
                return m_Column;
            }
        }

        public bool IsShown
        {
            get
            {
                return m_IsShown;
            }
            set
            {
                m_IsShown = value;
            }
        }

        public bool AreSameCellsInBoard(Cell i_Cell)
        {
            return (m_Row == i_Cell.Row && m_Column == i_Cell.Column);
        }
    }
}
