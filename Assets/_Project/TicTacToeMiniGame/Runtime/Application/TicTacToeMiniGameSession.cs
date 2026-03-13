using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TicTacToeMiniGame.Runtime.Contracts;
using TicTacToeMiniGame.Runtime.Domain;

namespace TicTacToeMiniGame.Runtime.Application
{
    public sealed class TicTacToeMiniGameSession
    {
        private readonly TicTacToeRulesService _rulesService;
        private readonly RewardGenerator _rewardGenerator;
        private UniTaskCompletionSource<TicTacToeMiniGameResult> _completionSource;
        private TicTacToeMiniGameRequest _request;
        private Random _random;

        public TicTacToeMiniGameSession(TicTacToeRulesService rulesService, RewardGenerator rewardGenerator)
        {
            _rulesService = rulesService;
            _rewardGenerator = rewardGenerator;
            Board = new TicTacToeBoard();
            Outcome = TicTacToeOutcome.InProgress();
            _completionSource = new UniTaskCompletionSource<TicTacToeMiniGameResult>();
            _random = new Random();
        }

        public TicTacToeBoard Board { get; }
        public TicTacToeOutcome Outcome { get; private set; }
        public TicTacToeMiniGameResult Result { get; private set; }
        public bool IsCompleted { get; private set; }
        public TicTacToeMark PlayerMark => TicTacToeMark.X;
        public TicTacToeMark OpponentMark => TicTacToeMark.O;

        public void Initialize(TicTacToeMiniGameRequest request)
        {
            _request = request;
            Board.Reset();
            Outcome = TicTacToeOutcome.InProgress();
            Result = null;
            IsCompleted = false;
            _completionSource = new UniTaskCompletionSource<TicTacToeMiniGameResult>();
            _random = request == null ? new Random() : new Random(request.Seed);
        }

        public UniTask<TicTacToeMiniGameResult> AwaitCompletionAsync()
        {
            return _completionSource.Task;
        }

        public bool TryMakePlayerMove(int cellIndex)
        {
            if (IsCompleted)
            {
                return false;
            }

            if (!_rulesService.IsMoveValid(Board, cellIndex, PlayerMark))
            {
                return false;
            }

            bool isApplied = Board.TryApplyMove(cellIndex, PlayerMark);

            if (!isApplied)
            {
                return false;
            }

            Outcome = _rulesService.Evaluate(Board);

            if (Outcome.IsFinished)
            {
                Complete();
            }

            return true;
        }

        public bool TryMakeOpponentMove()
        {
            if (IsCompleted)
            {
                return false;
            }

            if (Board.CurrentTurn != OpponentMark)
            {
                return false;
            }

            int opponentMoveIndex = GetOpponentMoveIndex();

            if (opponentMoveIndex < 0)
            {
                Outcome = _rulesService.Evaluate(Board);

                if (Outcome.IsFinished)
                {
                    Complete();
                }

                return false;
            }

            bool isApplied = Board.TryApplyMove(opponentMoveIndex, OpponentMark);

            if (!isApplied)
            {
                return false;
            }

            Outcome = _rulesService.Evaluate(Board);

            if (Outcome.IsFinished)
            {
                Complete();
            }

            return true;
        }

        public void Complete()
        {
            if (IsCompleted)
            {
                return;
            }

            RewardData reward = _rewardGenerator.Generate(_request, Outcome);
            TicTacToeMiniGameResult result = new TicTacToeMiniGameResult(Outcome, reward);
            Result = result;
            IsCompleted = true;
            _completionSource.TrySetResult(result);
        }

        public void Cancel(CancellationToken cancellationToken)
        {
            if (IsCompleted)
            {
                return;
            }

            Result = null;
            IsCompleted = true;
            _completionSource.TrySetCanceled(cancellationToken);
        }

        private int GetOpponentMoveIndex()
        {
            int winningMoveIndex = FindCriticalMoveIndex(OpponentMark);

            if (winningMoveIndex >= 0)
            {
                return winningMoveIndex;
            }

            int blockingMoveIndex = FindCriticalMoveIndex(PlayerMark);

            if (blockingMoveIndex >= 0)
            {
                return blockingMoveIndex;
            }

            IReadOnlyList<int> emptyCellIndices = Board.GetEmptyCellIndices();

            if (emptyCellIndices.Count == 0)
            {
                return -1;
            }

            int randomIndex = _random.Next(0, emptyCellIndices.Count);
            return emptyCellIndices[randomIndex];
        }

        private int FindCriticalMoveIndex(TicTacToeMark mark)
        {
            IReadOnlyList<int> emptyCellIndices = Board.GetEmptyCellIndices();

            for (int index = 0; index < emptyCellIndices.Count; index++)
            {
                int cellIndex = emptyCellIndices[index];

                if (_rulesService.WouldCompleteLine(Board, cellIndex, mark))
                {
                    return cellIndex;
                }
            }

            return -1;
        }
    }
}