using TicTacToeMiniGame.Runtime.Contracts;
using TicTacToeMiniGame.Runtime.Domain;
using TMPro;
using UnityEngine;

namespace TicTacToeMiniGame.Runtime.Presentation
{
    public sealed class TicTacToeResultView : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _rewardAmountText;
        [SerializeField] private TMP_Text _rewardRarityText;

        public void Show(TicTacToeOutcome outcome)
        {
            SetVisible(true);

            if (_titleText != null)
            {
                _titleText.text = GetOutcomeTitle(outcome);
            }

            if (_rewardAmountText != null)
            {
                _rewardAmountText.text = string.Empty;
            }

            if (_rewardRarityText != null)
            {
                _rewardRarityText.text = string.Empty;
            }
        }

        public void Show(TicTacToeMiniGameResult result)
        {
            if (result == null)
            {
                Hide();
                return;
            }

            SetVisible(true);

            if (_titleText != null)
            {
                _titleText.text = GetOutcomeTitle(result.Outcome);
            }

            if (_rewardAmountText != null)
            {
                _rewardAmountText.text = "Reward: " + result.Reward.Amount;
            }

            if (_rewardRarityText != null)
            {
                _rewardRarityText.text = "Rarity: " + result.Reward.Rarity;
            }
        }

        public void Hide()
        {
            SetVisible(false);
        }

        private void SetVisible(bool isVisible)
        {
            if (_container != null)
            {
                _container.SetActive(isVisible);
                return;
            }

            gameObject.SetActive(isVisible);
        }

        private string GetOutcomeTitle(TicTacToeOutcome outcome)
        {
            if (outcome.IsDraw)
            {
                return "Draw";
            }

            if (outcome.Winner == TicTacToeMark.X)
            {
                return "Victory";
            }

            if (outcome.Winner == TicTacToeMark.O)
            {
                return "Defeat";
            }

            return string.Empty;
        }
    }
}
