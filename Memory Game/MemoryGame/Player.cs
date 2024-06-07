using System;
using System.Linq;

namespace MemoryGame
{
    public class Player
    {
        private const string k_ComputerName = "COMPUTER";
        private readonly string r_PlayerName;
        private bool m_IsComputer;
        private bool m_IsPlayerTurn;
        private int m_Score;

        internal Player(string i_PlayerName)
        {
            m_Score = 0;
            m_IsComputer = i_PlayerName == k_ComputerName;
            m_IsPlayerTurn = false;
            if (m_IsComputer)
            {
                r_PlayerName = i_PlayerName;
            }
            else
            {
                r_PlayerName = i_PlayerName.First().ToString().ToUpper() + i_PlayerName.Substring(1).ToLower();
            }
        }

        public string Name
        {
            get
            {
                return r_PlayerName;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }

        public bool IsComputer
        {
            get
            {
                return m_IsComputer;
            }
            set
            {
                m_IsComputer = value;
            }
        }

        public bool IsPlayerTurn
        {
            get
            {
                return m_IsPlayerTurn;
            }
            set
            {
                m_IsPlayerTurn = value;
            }
        }
    }
}
