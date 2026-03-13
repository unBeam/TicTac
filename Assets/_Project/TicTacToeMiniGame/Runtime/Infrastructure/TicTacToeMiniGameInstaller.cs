using TicTacToeMiniGame.Runtime.Application;
using TicTacToeMiniGame.Runtime.Configs;
using TicTacToeMiniGame.Runtime.Domain;
using TicTacToeMiniGame.Runtime.Presentation;
using UnityEngine;
using Zenject;

namespace TicTacToeMiniGame.Runtime.Infrastructure
{
    public sealed class TicTacToeMiniGameInstaller : MonoInstaller
    {
        [SerializeField] private TicTacToeRewardConfig _rewardConfig;

        public override void InstallBindings()
        {
            Container.Bind<TicTacToeRewardConfig>().FromInstance(_rewardConfig).AsSingle();
            Container.Bind<TicTacToeRulesService>().AsSingle();
            Container.Bind<RewardGenerator>().AsSingle();
            Container.Bind<TicTacToeMiniGameSession>().AsSingle();
            Container.Bind<TicTacToeBoardPresenter>().AsSingle();
        }
    }
}