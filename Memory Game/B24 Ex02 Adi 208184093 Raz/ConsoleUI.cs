using System;
using System.Linq;
using System.Threading;
using MemoryGame;

namespace ConsoleUserInterface
{
    public class ConsoleUI
    {
        private const char k_FirstColumnLetter = 'A';
        private const char k_QuitSign = 'Q';
        private const char k_RestartGameSign = 'R';
        private const char k_FirstRowLetter = '1';
        private const char k_EqualSign = '=';
        private const char k_SeperateCellSign = '|';
        private const short k_NumOfEqualSingToPrintInBoard = 3;
        private const char k_SpaceSign = ' ';
        private const string k_Height = "height";
        private const string k_Width = "width";
        private const string k_Computer = "COMPUTER";
        private const int k_MinBoardSize = 4;
        private const int k_MaxBoardSize = 6;
        private const int k_DelayAfterTurn = 2000;
        private Game m_Game;
        private bool m_IsGameStillRunning;

        public ConsoleUI()
        {
            m_Game = null;
            m_IsGameStillRunning = true;
        }

        public void RunGame(bool i_IsFirstGame)
        {
            printWelcomeToGame();
            getPreGameUserInfo(i_IsFirstGame);
            clearScreen();
            playMemoryGame();
        }

        private void printWelcomeToGame()
        {
            {
                Console.WriteLine("*****************************");
                Console.WriteLine("*                           *");
                Console.WriteLine("*   Welcome to Memory Game! *");
                Console.WriteLine("*                           *");
                Console.WriteLine("*****************************");
                Console.WriteLine();
            }
        }

        private void getPreGameUserInfo(bool i_IsFirstGame)
        {
            string  player1Name, player2Name = k_Computer;
            bool    isTwoPlayerGame = false;
            short   boardHeightSize = 0, boardWidthSize = 0;

            if (i_IsFirstGame)
            {
                player1Name = getPlayerInfo();
                isTwoPlayerGame = isTwoPlayersGame();

                if (isTwoPlayerGame)
                {
                    player2Name = getPlayerInfo();
                }
            }
            else
            {
                player1Name = m_Game.Player1.Name;
                if (!m_Game.Player2.IsComputer)
                {
                    isTwoPlayerGame = true;
                }
                player2Name = m_Game.Player2.Name;
            }

            clearScreen();
            getBoardSize(ref boardHeightSize, ref boardWidthSize, isTwoPlayerGame, player1Name, player2Name);
            m_Game = new Game(boardHeightSize, boardWidthSize, isTwoPlayerGame, player1Name, player2Name);
        }

        private string getPlayerInfo()
        {
            string playerName;

            Console.Write("Enter player name: ");
            playerName = Console.ReadLine();

            while (string.IsNullOrEmpty(playerName) ||
                k_Computer.Equals(playerName, StringComparison.OrdinalIgnoreCase) ||
                playerName.Length < 2)
            {
                Console.WriteLine("Invalid name entered!\nName should be with at least 2 chars and not equals to 'computer' in any letter case.\n");
                Console.Write("Enter player name: ");
                playerName = Console.ReadLine();
                Console.WriteLine();
            }

            playerName = playerName.First().ToString().ToUpper() + playerName.Substring(1).ToLower();
            return playerName;
        }

        private bool isTwoPlayersGame()
        {
            bool    isTwoPlayersGame = false;
            bool    isValidChoice = false;
            string  input;

            while (!isValidChoice)
            {
                Console.Write("Enter:\n1 - Play against the computer\n2 - Play two-players game\n");
                input = Console.ReadLine();
                Console.WriteLine();
                if (short.TryParse(input, out short choice))
                {
                    isValidChoice = checkIfValidNumberOfPlayersChoice(choice);
                    isTwoPlayersGame = (choice == 2);
                }
                else if ((input.Length == 1) && (input[0] == k_QuitSign))
                {
                    m_IsGameStillRunning = false;
                    isValidChoice = true;
                }
                else
                {
                    Console.WriteLine("Invalid input! Enter a number or quit.");
                }
            }

            return isTwoPlayersGame;
        }

        private bool checkIfValidNumberOfPlayersChoice(short i_Input)
        {
            bool isValid = (i_Input == 1 || i_Input == 2);

            if (!isValid)
            {
                Console.WriteLine("Invalid input!");
            }

            return isValid;
        }

        private void getBoardSize(ref short io_BoardHeightSize, ref short io_BoardWidthSize, bool i_IsTwoPlayerGame, string i_Player1Name, string i_Player2Name)
        {
            bool isBoardCellsAmountEven = false;

            if (!i_IsTwoPlayerGame)
            {
                Console.WriteLine("Hi " + i_Player1Name + "!");
            }
            else
            {
                Console.WriteLine("Hi " + i_Player1Name + " and " + i_Player2Name + "!");
            }

            Console.WriteLine("\nWe'll start with board size.\nBoard cells amount should be even.");
            while (!isBoardCellsAmountEven)
            {
                readBoardAxisSize(ref io_BoardHeightSize, k_Height);
                readBoardAxisSize(ref io_BoardWidthSize, k_Width);
                isBoardCellsAmountEven = ((io_BoardHeightSize * io_BoardWidthSize) % 2 == 0);
                if (!isBoardCellsAmountEven)
                {
                    Console.WriteLine("Board cells amount should be even!\n");
                }
            }
        }

        private void readBoardAxisSize(ref short io_BoardAxisSize, string i_AxisOfBoard)
        {
            bool    isValidInput = false;
            string  axisInputStr;

            while (!isValidInput)
            {
                Console.Write("Enter board {0} between {1} and {2}: ", i_AxisOfBoard, k_MinBoardSize, k_MaxBoardSize);
                axisInputStr = Console.ReadLine();
                checkBoardAxisValidation(ref io_BoardAxisSize, ref isValidInput, axisInputStr, i_AxisOfBoard);
            }
        }

        private void checkBoardAxisValidation(ref short io_BoardAxisSize, ref bool io_IsValidInput, string i_InputStr, string i_AxisOfBoard)
        {
            if (short.TryParse(i_InputStr, out io_BoardAxisSize))
            {
                io_IsValidInput = checkBoardSizeInputValidation(io_BoardAxisSize, i_AxisOfBoard);
            }
            else
            {
                Console.WriteLine("Invalid input! Enter a number.");
            }
        }

        private bool checkBoardSizeInputValidation(int i_Input, string i_AxisOfBoard)
        {
            bool isValid = ((i_Input >= k_MinBoardSize) && (i_Input <= k_MaxBoardSize));

            if (!isValid)
            {
                Console.WriteLine("Invalid input! Board {0} must be between {1} and {2}.\n",
                    i_AxisOfBoard, k_MinBoardSize, k_MaxBoardSize);
            }

            return isValid;
        }

        private void clearScreen()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }

        private void playMemoryGame()
        {
            while (!isGameFinished())
            {
                if (m_Game.Player1.IsPlayerTurn || !m_Game.Player2.IsComputer)
                {
                    printGameBoardAndInfo();
                    humanPlayerTurn();
                }
                else
                {
                    computerTurn();
                }

                if (m_IsGameStillRunning)
                {
                    clearScreen();
                }
            }
        }

        private bool isGameFinished()
        {
            bool    isGameOver = false;
            string  userGameChoice;
            bool    isValidChoiceInput;

            if (!m_IsGameStillRunning)
            {
                isGameOver = true;
            }
            else if (m_Game.GameBoard.IsBoardFullyShown())
            {
                isGameOver = true;
                clearScreen();

                if (m_Game.Player1.Score > m_Game.Player2.Score)
                {
                    printWinningMessage(true, m_Game.Player1.Name);
                }
                else if (m_Game.Player1.Score < m_Game.Player2.Score)
                {
                    printWinningMessage(true, m_Game.Player2.Name);
                }
                else if ((m_Game.Player1.Score + m_Game.Player2.Score) == (m_Game.GameBoard.Rows * m_Game.GameBoard.Columns / 2))
                {
                    printWinningMessage(false, "Tied game!!!");
                }

                do
                {
                    Console.Write("Press {0} to play another round, or {1} to exit: ", k_RestartGameSign, k_QuitSign);
                    userGameChoice = Console.ReadLine();
                    isValidChoiceInput = ((!string.IsNullOrEmpty(userGameChoice)) && (userGameChoice.Length == 1) &&
                        ((userGameChoice[0] == k_QuitSign) || (userGameChoice[0] == k_RestartGameSign)));
                    if (!isValidChoiceInput)
                    {
                        Console.WriteLine("Invalid input!");
                    }
                } while (!isValidChoiceInput);

                if (userGameChoice[0] == k_RestartGameSign)
                {
                    RunGame(false);
                }
                else
                {
                    Console.WriteLine("The game has ended.");
                    Console.WriteLine("Goodbye!\n");
                }
            }

            return isGameOver;
        }

        private void printWinningMessage(bool i_HasWinner, string i_WinnerName)
        {
            Console.WriteLine("*********************************");
            if (i_HasWinner)
            {
                Console.WriteLine("      Congratulations!!!");
                Console.WriteLine("      {0} is the Winner!", i_WinnerName);
            }
            else
            {
                Console.WriteLine("          {0}", i_WinnerName);
                Console.WriteLine("      You both are winners!!");
            }

            Console.WriteLine();
            Console.WriteLine("      " + m_Game.Player1.Name.ToString() + "'s score: " + m_Game.Player1.Score.ToString());
            Console.WriteLine("      " + m_Game.Player2.Name.ToString() + "'s score: " + m_Game.Player2.Score.ToString());
            Console.WriteLine("*********************************\n");
        }

        private void humanPlayerTurn()
        {
            short rowInput1 = 0, rowInput2 = 0, columnInput1 = 0, columnInput2 = 0;

            Console.WriteLine("Choose an empty cell to show it's letter: ");
            char playerShownFirstChoice = getUserTurn(ref rowInput1, ref columnInput1);
            if (m_IsGameStillRunning)
            {
                Console.WriteLine("Choose another empty cell to show it's letter: ");
                char playerShownSecondChoice = getUserTurn(ref rowInput2, ref columnInput2);
                handlePlayerPostTurn((playerShownFirstChoice == playerShownSecondChoice), m_Game.GameBoard.CellBoard[rowInput1, columnInput1], m_Game.GameBoard.CellBoard[rowInput2, columnInput2]);
            }
        }

        private void handlePlayerPostTurn(bool i_PlayerSuccess, Cell i_CellInput1, Cell i_CellInput2)
        {         
            delayTheGame(k_DelayAfterTurn);
            if (i_PlayerSuccess)
            {
                if (m_Game.Player1.IsPlayerTurn)
                {
                    m_Game.Player1.Score++;
                }
                else
                {
                    m_Game.Player2.Score++;
                }
            }
            else
            {
                m_Game.GameBoard.CellBoard[i_CellInput1.Row, i_CellInput1.Column].IsShown = false;
                m_Game.GameBoard.CellBoard[i_CellInput2.Row, i_CellInput2.Column].IsShown = false;
                clearScreen();
                printGameBoardAndInfo();
                m_Game.SwitchTurnBetweenPlayers();
            }

            if (!m_Game.GameBoard.ShownCellsMemory.ContainsKey(i_CellInput1.Letter))
            {
                m_Game.GameBoard.ShownCellsMemory.Add(i_CellInput1.Letter, i_CellInput1);
            }

            if (!m_Game.GameBoard.ShownCellsMemory.ContainsKey(i_CellInput2.Letter))
            {
                m_Game.GameBoard.ShownCellsMemory.Add(i_CellInput2.Letter, i_CellInput2);
            }
        }

        private char getUserTurn(ref short io_RowInput, ref short io_ColumnInput)
        {
            char userChoiceCellLetter;

            getAndCheckUserTurnInput(ref io_RowInput, ref io_ColumnInput);
            if (m_IsGameStillRunning)
            {
                m_Game.GameBoard.CellBoard[io_RowInput, io_ColumnInput].IsShown = true;
                clearScreen();
                printGameBoardAndInfo();
                userChoiceCellLetter =  m_Game.GameBoard.CellBoard[io_RowInput, io_ColumnInput].Letter;
            }
            else
            {
                userChoiceCellLetter = k_QuitSign;
            }

            return userChoiceCellLetter;
        }

        private void getAndCheckUserTurnInput(ref short io_RowInput, ref short io_ColumnInput)
        {
            do
            {
                getValidColumn(ref io_ColumnInput);
                if (m_IsGameStillRunning)
                {
                    getValidRow(ref io_RowInput);
                }
                else
                {
                    break;
                }

                if (m_Game.GameBoard.CellBoard[io_RowInput, io_ColumnInput].IsShown)
                {
                    Console.WriteLine("\nCell had already chosen! Choose an empty cell to show it's letter:");
                }
            } while (m_Game.GameBoard.CellBoard[io_RowInput, io_ColumnInput].IsShown);
        }

        private void getValidColumn(ref short io_ColumnInput)
        {
            string  playerColumnInput;
            bool    isColumnValid = false;

            do
            {
                Console.Write("Enter cell column (A - {0}): ", (char)('A' + m_Game.GameBoard.Columns - 1));
                playerColumnInput = Console.ReadLine();

                if ((string.IsNullOrEmpty(playerColumnInput)) ||
                    (playerColumnInput.Length > 1) ||
                    (playerColumnInput[0] < 'A') ||
                    (playerColumnInput[0] != k_QuitSign &&
                    (playerColumnInput[0] > (char)('A' + m_Game.GameBoard.Columns - 1))))
                {
                    Console.WriteLine("Column value has to be between A and {0} (capital letter).\n", (char)('A' + m_Game.GameBoard.Columns - 1));
                }
                else if (playerColumnInput[0] == k_QuitSign)
                {
                    isColumnValid = true;
                    forceStopGameByPlayer();
                }
                else
                {
                    isColumnValid = true;
                    io_ColumnInput = (short)(playerColumnInput[0] - 'A');
                }
            } while (!isColumnValid);
        }

        private void getValidRow(ref short io_RowInput)
        {
            string  playerRowInput;
            bool    isRowValid = false;

            do
            {
                Console.Write("Enter cell row (1 - {0}): ", m_Game.GameBoard.Rows);
                playerRowInput = Console.ReadLine();
                if ((string.IsNullOrEmpty(playerRowInput)) || (!m_Game.GameBoard.IsRowInRange(playerRowInput) && (playerRowInput[0] != k_QuitSign)))
                {
                    Console.WriteLine("Row value has to be between 1 and {0}.\n", m_Game.GameBoard.Rows);
                }
                else if (playerRowInput[0] == k_QuitSign)
                {
                    isRowValid = true;
                    forceStopGameByPlayer();
                }
                else
                {
                    isRowValid = true;
                    io_RowInput = (short)(playerRowInput[0] - k_FirstRowLetter);
                }
            } while (!isRowValid);
        }

        private void forceStopGameByPlayer()
        {
            m_IsGameStillRunning = false;
            clearScreen();
            Console.WriteLine("The game stopped due to pressing 'Q'");
            Console.WriteLine("Goodbye!\n");
        }

        private void computerTurn()
        {
            Cell firstCellChoice = m_Game.GameBoard.GetRandomEmptyCellInBoard();
            Cell secondCellChoice;

            m_Game.GameBoard.CellBoard[firstCellChoice.Row, firstCellChoice.Column].IsShown = true;
            printComputerTurn(firstCellChoice, firstCellChoice, false);
            do
            {
                secondCellChoice = m_Game.GameBoard.GetSmartEmptyCell(firstCellChoice);
            } while (firstCellChoice.AreSameCellsInBoard(secondCellChoice));

            m_Game.GameBoard.CellBoard[secondCellChoice.Row, secondCellChoice.Column].IsShown = true;
            printComputerTurn(firstCellChoice, secondCellChoice, true);
            handlePlayerPostTurn(firstCellChoice.Letter == secondCellChoice.Letter, firstCellChoice, secondCellChoice);
        }

        private void printComputerTurn(Cell i_ComputerCellChoice1, Cell i_ComputerCellChoice2, bool i_IsSecondChoice)
        {
            clearScreen();
            printWhoPlaysNow();
            printGameBoard();
            Console.WriteLine("\nComputer first cell choice: [{0}, {1}]", (char)(i_ComputerCellChoice1.Column + 'A'), i_ComputerCellChoice1.Row + 1);
            if (i_IsSecondChoice)
            {
                Console.WriteLine("Computer second cell choice: [{0}, {1}]", (char)(i_ComputerCellChoice2.Column + 'A'), i_ComputerCellChoice2.Row + 1);
                printPlayersInfo();
            }
            else
            {
                printPlayersInfo();
                delayTheGame(k_DelayAfterTurn);
            }
        }

        private void printGameBoardAndInfo()
        {
            printWhoPlaysNow();
            printGameBoard();
            printPlayersInfo();
        }

        private void printGameBoard()
        {
            printBoardUpperFrame();
            printLineOfEqualSign();
            for (short i = 0; i < m_Game.GameBoard.Rows; i++)
            {
                printRowsOfBoardWithFrame(i);
                printLineOfEqualSign();
            }
        }

        private void printBoardUpperFrame()
        {
            string toPrint;

            Console.Write(k_SpaceSign.ToString() + k_SpaceSign.ToString());
            for (short i = 0; i < m_Game.GameBoard.Columns; i++)
            {
                toPrint = string.Concat(k_SpaceSign, k_SpaceSign, (char)(k_FirstColumnLetter + i), k_SpaceSign);
                Console.Write(toPrint);
            }

            Console.WriteLine();
        }

        private void printLineOfEqualSign()
        {
            Console.Write(k_SpaceSign.ToString() + k_SpaceSign.ToString());
            Console.Write(k_EqualSign);
            for (short i = 0; i < m_Game.GameBoard.Columns; i++)
            {
                for (short j = 0; j < k_NumOfEqualSingToPrintInBoard; j++)
                {
                    Console.Write(k_EqualSign);
                }

                Console.Write(k_EqualSign);
            }

            Console.WriteLine();
        }

        private void printRowsOfBoardWithFrame(short i_RowNumber)
        {
            Console.Write((i_RowNumber + 1).ToString() + k_SpaceSign.ToString());
            for (short column = 0; column < m_Game.GameBoard.Columns; column++)
            {
                Console.Write(k_SeperateCellSign.ToString() + k_SpaceSign.ToString());
                if (m_Game.GameBoard.CellBoard[i_RowNumber, column].IsShown)
                {
                    Console.Write(m_Game.GameBoard.CellBoard[i_RowNumber, column].Letter);
                }
                else
                {
                    Console.Write(k_SpaceSign);
                }

                Console.Write(k_SpaceSign);
            }

            Console.WriteLine(k_SeperateCellSign);
        }

        private void printWhoPlaysNow()
        {
            if (m_Game.Player1.IsPlayerTurn)
            {
                Console.Write(m_Game.Player1.Name);
            }
            else
            {
                Console.Write(m_Game.Player2.Name);
            }

            Console.WriteLine("'s turn:");
            Console.WriteLine();
        }

        private void printPlayersInfo()
        {
            Console.WriteLine();
            Console.WriteLine(m_Game.Player1.Name.ToString() + "'s score: " + m_Game.Player1.Score.ToString());
            Console.WriteLine(m_Game.Player2.Name.ToString() + "'s score: " + m_Game.Player2.Score.ToString());
            Console.WriteLine("\nAt any stage enter '{0}' to quit.\n", k_QuitSign);
        }

        private void delayTheGame(int i_TimeToDelay)
        {
            Thread.Sleep(i_TimeToDelay);
        }
    }
}
