using TicTacToeMiniGame.Runtime.Contracts;
using UnityEngine;

namespace TicTacToeMiniGame.Runtime.Configs
{
    [CreateAssetMenu(fileName = "TicTacToeRewardConfig", menuName = "TicTacToeMiniGame/RewardConfig")]
    public sealed class TicTacToeRewardConfig : ScriptableObject
    {
        [SerializeField] private RewardRarity _defaultRarity = RewardRarity.Common;

        public RewardRarity GetDefaultRarity()
        {
            return _defaultRarity;
        }
    }
}
