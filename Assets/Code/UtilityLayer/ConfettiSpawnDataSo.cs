using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.UtilityLayer
{
    [CreateAssetMenu(menuName = "Project/Create ConfettiSpawnDataSo", fileName = "ConfettiSpawnDataSo", order = 0)]
    public class ConfettiSpawnDataSo : ScriptableObject
    {
        [field: SerializeField] public ConfettiSpawnData SpawnData { get; private set; }
    }

    [System.Serializable]
    public class ConfettiSpawnData
    {
        public float LifeTime;
        public AssetReference Prefab;
    }
}