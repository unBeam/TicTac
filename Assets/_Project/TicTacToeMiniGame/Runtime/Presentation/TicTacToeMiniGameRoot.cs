using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TicTacToeMiniGame.Runtime.Application;
using TicTacToeMiniGame.Runtime.Contracts;
using UnityEngine;
using Zenject;

namespace TicTacToeMiniGame.Runtime.Presentation
{
    public sealed class TicTacToeMiniGameRoot : MonoBehaviour
    {
        [SerializeField] private TicTacToeBoardView _boardView;
        [SerializeField] private TicTacToeResultView _resultView;
        [SerializeField] private bool _autoInitializeOnStart = true;
        [SerializeField] private int _rewardAmount = 100;
        [SerializeField] private int _seed;
        [SerializeField] private int _resultCloseDelayMilliseconds = 1000;

        private TicTacToeBoardPresenter _boardPresenter;
        private TicTacToeMiniGameSession _session;
        private bool _isInitialized;

        [Inject]
        public void Construct(TicTacToeBoardPresenter boardPresenter, TicTacToeMiniGameSession session)
        {
            _boardPresenter = boardPresenter;
            _session = session;
        }

        public void Initialize()
        {
            TicTacToeMiniGameRequest request = new TicTacToeMiniGameRequest(_rewardAmount, _seed);
            Initialize(request);
        }

        public void Initialize(TicTacToeMiniGameRequest request)
        {
            if (_boardPresenter == null || _session == null)
            {
                throw new InvalidOperationException("TicTacToe mini game dependencies are not initialized.");
            }

            _boardPresenter.Initialize(_boardView, _resultView, _session);
            _boardPresenter.StartSession(request);
            _isInitialized = true;
        }

        public async UniTask<TicTacToeMiniGameResult> RunAsync(TicTacToeMiniGameRequest request, CancellationToken cancellationToken)
        {
            Initialize(request);

            try
            {
                TicTacToeMiniGameResult result = await _session.AwaitCompletionAsync().AttachExternalCancellation(cancellationToken);
                _session = null;
                return _session.Result;
            }
            catch (OperationCanceledException)
            {
                _session.Cancel(cancellationToken);
                throw;
            }
        }

        private void Start()
        {
            if (!_autoInitializeOnStart)
            {
                return;
            }

            if (_isInitialized)
            {
                return;
            }

            Initialize();
        }
    }
}