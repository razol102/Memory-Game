using System;

namespace MemoryGame
{
    public class Game
    {
        private Board m_Board;
        private Player m_Player1;
        private Player m_Player2;

        public Game(short i_BoardRows, short i_BoardColumns, bool i_IsTwoPlayerGame, string i_Player1Name, string i_Player2Name)
        {
            m_Board = new Board(i_BoardRows, i_BoardColumns);
            m_Player1 = new Player(i_Player1Name);
            m_Player1.IsPlayerTurn = true;
            m_Player2 = new Player(i_Player2Name);
            if (!i_IsTwoPlayerGame)
            {
                m_Player2.IsComputer = true;
            }
        }

        public Board GameBoard
        {
            get
            {
                return m_Board;
            }
        }

        public Player Player1
        {
            get
            {
                return m_Player1;
            }
        }

        public Player Player2
        {
            get
            {
                return m_Player2;
            }
        }

        public void SwitchTurnBetweenPlayers()
        {
           Player1.IsPlayerTurn = !Player1.IsPlayerTurn;
           Player2.IsPlayerTurn = !Player2.IsPlayerTurn;
        }
    }
}
