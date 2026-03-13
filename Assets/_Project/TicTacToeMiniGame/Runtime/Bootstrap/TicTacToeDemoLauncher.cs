using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TicTacToeMiniGame.Runtime.Contracts;
using TicTacToeMiniGame.Runtime.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TicTacToeMiniGame.Runtime.Bootstrap
{
    public sealed class TicTacToeDemoLauncher : MonoBehaviour
    {
        [SerializeField] private Button _launchButton;
        [SerializeField] private TMP_Text _lastResultText;
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private int _rewardAmount = 100;
        [SerializeField] private int _seed;

        private ITicTacToeMiniGameRunner _miniGameRunner;
        private bool _isRunning;

        [Inject]
        public void Construct(ITicTacToeMiniGameRunner miniGameRunner)
        {
            _miniGameRunner = miniGameRunner;
        }

        private void Awake()
        {
            if (_launchButton != null)
            {
                _launchButton.onClick.AddListener(OnLaunchClicked);
            }

            SetResultText("Last Result: -");
            SetRewardText("Reward: -");
            SetLaunchInteractable(true);
        }

        private void OnDestroy()
        {
            if (_launchButton != null)
            {
                _launchButton.onClick.RemoveListener(OnLaunchClicked);
            }
        }

        private void OnLaunchClicked()
        {
            if (_isRunning)
            {
                return;
            }

            RunMiniGameAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid RunMiniGameAsync(CancellationToken cancellationToken)
        {
            if (_miniGameRunner == null)
            {
                SetResultText("Last Result: runner missing");
                return;
            }

            _isRunning = true;
            SetLaunchInteractable(false);
            SetResultText("Last Result: playing...");
            SetRewardText("Reward: -");

            try
            {
                TicTacToeMiniGameRequest request = new TicTacToeMiniGameRequest(_rewardAmount, _seed);
                TicTacToeMiniGameResult result = await _miniGameRunner.RunAsync(request, cancellationToken);
                SetResultText("Last Result: " + GetOutcomeLabel(result));
                SetRewardText("Reward: " + result.Reward.Amount + " (" + result.Reward.Rarity + ")");
            }
            catch (OperationCanceledException)
            {
                SetResultText("Last Result: cancelled");
                SetRewardText("Reward: -");
            }
            finally
            {
                _isRunning = false;
                SetLaunchInteractable(true);
            }
        }

        private void SetLaunchInteractable(bool isInteractable)
        {
            if (_launchButton != null)
            {
                _launchButton.interactable = isInteractable;
            }
        }

        private void SetResultText(string value)
        {
            if (_lastResultText != null)
            {
                _lastResultText.text = value;
            }
        }

        private void SetRewardText(string value)
        {
            if (_rewardText != null)
            {
                _rewardText.text = value;
            }
        }

        private string GetOutcomeLabel(TicTacToeMiniGameResult result)
        {
            if (result.Outcome.IsDraw)
            {
                return "Draw";
            }

            if (result.Outcome.Winner == TicTacToeMark.X)
            {
                return "Victory";
            }

            if (result.Outcome.Winner == TicTacToeMark.O)
            {
                return "Defeat";
            }

            return "Unknown";
        }
    }
}
