using UnityEngine;

namespace TicTacToeMiniGame.Runtime.Configs
{
    [CreateAssetMenu(fileName = "TicTacToeMiniGameAssetConfig", menuName = "TicTacToeMiniGame/AssetConfig")]
    public sealed class TicTacToeMiniGameAssetConfig : ScriptableObject
    {
        [SerializeField] private string _prefabAddress;
        [SerializeField] private Transform _defaultParent;

        public string PrefabAddress => _prefabAddress;

        public Transform DefaultParent => _defaultParent;
    }
}
