namespace TicTacToeMiniGame.Runtime.Domain
{
    public readonly struct TicTacToeOutcome
    {
        public TicTacToeOutcome(bool isFinished, bool isDraw, TicTacToeMark winner)
        {
            IsFinished = isFinished;
            IsDraw = isDraw;
            Winner = winner;
        }

        public bool IsFinished { get; }

        public bool IsDraw { get; }

        public TicTacToeMark Winner { get; }

        public bool IsWinFor(TicTacToeMark mark)
        {
            return IsFinished && !IsDraw && Winner == mark;
        }

        public bool IsLossFor(TicTacToeMark mark)
        {
            return IsFinished && !IsDraw && Winner != TicTacToeMark.None && Winner != mark;
        }

        public static TicTacToeOutcome InProgress()
        {
            return new TicTacToeOutcome(false, false, TicTacToeMark.None);
        }

        public static TicTacToeOutcome Draw()
        {
            return new TicTacToeOutcome(true, true, TicTacToeMark.None);
        }

        public static TicTacToeOutcome Win(TicTacToeMark winner)
        {
            return new TicTacToeOutcome(true, false, winner);
        }
    }
}
