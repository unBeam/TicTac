namespace TicTacToeMiniGame.Runtime.Domain
{
    public sealed class TicTacToeRulesService
    {
        private static readonly int[,] WinLines =
        {
            { 0, 1, 2 },
            { 3, 4, 5 },
            { 6, 7, 8 },
            { 0, 3, 6 },
            { 1, 4, 7 },
            { 2, 5, 8 },
            { 0, 4, 8 },
            { 2, 4, 6 }
        };

        public bool IsMoveValid(TicTacToeBoard board, int cellIndex, TicTacToeMark mark)
        {
            if (board == null)
            {
                return false;
            }

            if (cellIndex < 0 || cellIndex > 8)
            {
                return false;
            }

            if (mark == TicTacToeMark.None)
            {
                return false;
            }

            if (board.CurrentTurn != mark)
            {
                return false;
            }

            return board.GetCell(cellIndex) == TicTacToeMark.None;
        }

        public TicTacToeOutcome Evaluate(TicTacToeBoard board)
        {
            if (board == null)
            {
                return TicTacToeOutcome.InProgress();
            }

            for (int lineIndex = 0; lineIndex < WinLines.GetLength(0); lineIndex++)
            {
                int firstIndex = WinLines[lineIndex, 0];
                int secondIndex = WinLines[lineIndex, 1];
                int thirdIndex = WinLines[lineIndex, 2];
                TicTacToeMark firstMark = board.GetCell(firstIndex);

                if (firstMark == TicTacToeMark.None)
                {
                    continue;
                }

                if (board.GetCell(secondIndex) == firstMark && board.GetCell(thirdIndex) == firstMark)
                {
                    return TicTacToeOutcome.Win(firstMark);
                }
            }

            if (!board.HasEmptyCells())
            {
                return TicTacToeOutcome.Draw();
            }

            return TicTacToeOutcome.InProgress();
        }

        public bool WouldCompleteLine(TicTacToeBoard board, int cellIndex, TicTacToeMark mark)
        {
            if (board == null)
            {
                return false;
            }

            if (cellIndex < 0 || cellIndex > 8)
            {
                return false;
            }

            if (mark == TicTacToeMark.None)
            {
                return false;
            }

            if (!board.IsCellEmpty(cellIndex))
            {
                return false;
            }

            for (int lineIndex = 0; lineIndex < WinLines.GetLength(0); lineIndex++)
            {
                int firstIndex = WinLines[lineIndex, 0];
                int secondIndex = WinLines[lineIndex, 1];
                int thirdIndex = WinLines[lineIndex, 2];

                if (firstIndex != cellIndex && secondIndex != cellIndex && thirdIndex != cellIndex)
                {
                    continue;
                }

                TicTacToeMark firstMark = firstIndex == cellIndex ? mark : board.GetCell(firstIndex);
                TicTacToeMark secondMark = secondIndex == cellIndex ? mark : board.GetCell(secondIndex);
                TicTacToeMark thirdMark = thirdIndex == cellIndex ? mark : board.GetCell(thirdIndex);

                if (firstMark == mark && secondMark == mark && thirdMark == mark)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
