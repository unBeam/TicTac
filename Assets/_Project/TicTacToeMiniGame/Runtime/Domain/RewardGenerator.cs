using TicTacToeMiniGame.Runtime.Configs;
using TicTacToeMiniGame.Runtime.Contracts;

namespace TicTacToeMiniGame.Runtime.Domain
{
    public sealed class RewardGenerator
    {
        private readonly TicTacToeRewardConfig _rewardConfig;

        public RewardGenerator(TicTacToeRewardConfig rewardConfig)
        {
            _rewardConfig = rewardConfig;
        }

        public RewardData Generate(TicTacToeMiniGameRequest request, TicTacToeOutcome outcome)
        {
            int baseAmount = request == null ? 0 : request.RewardAmount;
            int amount;
            RewardRarity rarity;

            if (outcome.IsWinFor(TicTacToeMark.X))
            {
                amount = baseAmount;
                rarity = _rewardConfig == null ? RewardRarity.Rare : _rewardConfig.GetDefaultRarity();
            }
            else if (outcome.IsDraw)
            {
                amount = baseAmount / 2;
                rarity = RewardRarity.Common;
            }
            else
            {
                amount = 0;
                rarity = RewardRarity.Common;
            }

            return new RewardData(amount, rarity);
        }
    }
}
