using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code.UtilityLayer
{
    [CreateAssetMenu(menuName = "Project/Create CubeSpawnData", fileName = "CubeSpawnDataSo", order = 0)]
    public class CubeSpawnDataSo : ScriptableObject
    {
        [field: SerializeField] public CubeSpawnData SpawnData { get; private set; }
    }

    [Serializable]
    public class CubeSpawnData
    {
        public int Count;
        public float MinSpeed;
        public float MaxSpeed;
        public float PositionOffset;
        public AssetReference Prefab;
    }
}