using TicTacToeMiniGame.Runtime.Configs;
using TicTacToeMiniGame.Runtime.Contracts;
using TicTacToeMiniGame.Runtime.Infrastructure;
using UnityEngine;
using Zenject;

namespace TicTacToeMiniGame.Runtime.Bootstrap
{
    public sealed class TicTacToeHostInstaller : MonoInstaller
    {
        [SerializeField] private TicTacToeMiniGameAssetConfig _assetConfig;

        public override void InstallBindings()
        {
            Container.Bind<TicTacToeMiniGameAssetConfig>().FromInstance(_assetConfig).AsSingle();
            Container.Bind<ITicTacToeMiniGameRunner>().To<AddressableTicTacToeMiniGameRunner>().AsSingle();
        }
    }
}