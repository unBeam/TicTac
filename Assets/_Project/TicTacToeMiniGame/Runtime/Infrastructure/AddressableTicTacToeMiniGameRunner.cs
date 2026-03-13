using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TicTacToeMiniGame.Runtime.Configs;
using TicTacToeMiniGame.Runtime.Contracts;
using TicTacToeMiniGame.Runtime.Presentation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace TicTacToeMiniGame.Runtime.Infrastructure
{
    public sealed class AddressableTicTacToeMiniGameRunner : ITicTacToeMiniGameRunner
    {
        private readonly DiContainer _container;
        private readonly TicTacToeMiniGameAssetConfig _assetConfig;

        public AddressableTicTacToeMiniGameRunner(DiContainer container, TicTacToeMiniGameAssetConfig assetConfig)
        {
            _container = container;
            _assetConfig = assetConfig;
        }

        public async UniTask<TicTacToeMiniGameResult> RunAsync(TicTacToeMiniGameRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_assetConfig == null)
            {
                throw new InvalidOperationException("TicTacToeMiniGameAssetConfig is not assigned.");
            }

            if (_container == null)
            {
                throw new InvalidOperationException("DiContainer is not available.");
            }

            AsyncOperationHandle<GameObject> prefabHandle = default;
            GameObject instance = null;

            try
            {
                prefabHandle = await LoadPrefabAsync(cancellationToken);

                if (!prefabHandle.IsValid() || prefabHandle.Result == null)
                {
                    throw new InvalidOperationException("Failed to load TicTacToe mini game prefab from Addressables.");
                }

                instance = _container.InstantiatePrefab(prefabHandle.Result, _assetConfig.DefaultParent);

                if (instance == null)
                {
                    throw new InvalidOperationException("Failed to instantiate TicTacToe mini game prefab.");
                }

                TicTacToeMiniGameRoot root = instance.GetComponentInChildren<TicTacToeMiniGameRoot>(true);

                if (root == null)
                {
                    throw new InvalidOperationException("TicTacToeMiniGameRoot was not found on instantiated prefab.");
                }

                return await root.RunAsync(request, cancellationToken);
            }
            finally
            {
                await ReleaseAsync(instance, prefabHandle);
            }
        }

        public async UniTask<AsyncOperationHandle<GameObject>> LoadPrefabAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_assetConfig == null)
            {
                throw new InvalidOperationException("TicTacToeMiniGameAssetConfig is not assigned.");
            }

            if (string.IsNullOrWhiteSpace(_assetConfig.PrefabAddress))
            {
                throw new InvalidOperationException("PrefabAddress is empty in TicTacToeMiniGameAssetConfig.");
            }

            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(_assetConfig.PrefabAddress);
            await handle.ToUniTask(cancellationToken: cancellationToken);
            return handle;
        }

        public async UniTask ReleaseAsync(GameObject instance, AsyncOperationHandle<GameObject> prefabHandle)
        {
            if (instance != null)
            {
                UnityEngine.Object.Destroy(instance);
                await UniTask.Yield();
            }

            if (prefabHandle.IsValid())
            {
                Addressables.Release(prefabHandle);
            }
        }
    }
}
