using System;
using UnityEngine;

namespace Code.Spawning
{
    [CreateAssetMenu(menuName = "Create CubeSpawnDataSO", fileName = "CubeSpawnDataSO", order = 0)]
    public class CubeSpawnDataSo : ScriptableObject
    {
        [field: SerializeField] public CubeSpawnData Data { get; private set; }
    }

    [Serializable]
    public class CubeSpawnData
    {
        public int Size;

        public float MinSpeed;
        public float MaxSpeed;

        public float PositionRangeOffset;
    }
}