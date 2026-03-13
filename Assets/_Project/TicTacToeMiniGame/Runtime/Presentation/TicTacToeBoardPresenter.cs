using Cysharp.Threading.Tasks;
using TicTacToeMiniGame.Runtime.Application;
using TicTacToeMiniGame.Runtime.Contracts;

namespace TicTacToeMiniGame.Runtime.Presentation
{
    public sealed class TicTacToeBoardPresenter
    {
        private const int OpponentMoveDelayMilliseconds = 500;

        private TicTacToeBoardView _boardView;
        private TicTacToeResultView _resultView;
        private TicTacToeMiniGameSession _session;
        private bool _isProcessingMove;

        public void Initialize(TicTacToeBoardView boardView, TicTacToeResultView resultView, TicTacToeMiniGameSession session)
        {
            if (_boardView != null)
            {
                _boardView.CellSelected -= OnCellSelected;
            }

            _boardView = boardView;
            _resultView = resultView;
            _session = session;
            _isProcessingMove = false;

            if (_boardView != null)
            {
                _boardView.CellSelected += OnCellSelected;
            }
        }

        public void StartSession(TicTacToeMiniGameRequest request)
        {
            if (_session == null)
            {
                return;
            }

            _session.Initialize(request);
            _isProcessingMove = false;

            if (_resultView != null)
            {
                _resultView.Hide();
            }

            Refresh();
        }

        public void Refresh()
        {
            if (_boardView == null || _session == null)
            {
                return;
            }

            bool isInteractable = !_session.IsCompleted
                && !_isProcessingMove
                && _session.Board.CurrentTurn == _session.PlayerMark;

            _boardView.Render(_session.Board, isInteractable);
        }

        public void ShowResult()
        {
            if (_resultView == null || _session == null)
            {
                return;
            }

            TicTacToeMiniGameResult result = _session.Result;

            if (result != null)
            {
                _resultView.Show(result);
                return;
            }

            _resultView.Show(_session.Outcome);
        }

        private void OnCellSelected(int cellIndex)
        {
            ProcessTurnAsync(cellIndex).Forget();
        }

        private async UniTaskVoid ProcessTurnAsync(int cellIndex)
        {
            if (_session == null)
            {
                return;
            }

            if (_isProcessingMove)
            {
                return;
            }

            _isProcessingMove = true;

            bool isAccepted = _session.TryMakePlayerMove(cellIndex);

            if (!isAccepted)
            {
                _isProcessingMove = false;
                Refresh();
                return;
            }

            Refresh();

            if (_session.IsCompleted)
            {
                _isProcessingMove = false;
                ShowResult();
                Refresh();
                return;
            }

            await UniTask.Delay(OpponentMoveDelayMilliseconds);

            if (!_session.IsCompleted)
            {
                _session.TryMakeOpponentMove();
            }

            _isProcessingMove = false;
            Refresh();

            if (_session.IsCompleted)
            {
                ShowResult();
            }
        }
    }
}