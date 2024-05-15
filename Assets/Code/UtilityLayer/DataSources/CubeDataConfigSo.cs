using UnityEngine;

namespace Code.UtilityLayer.DataSources
{
    [CreateAssetMenu(menuName = "Create CubeSpawnDataSo", fileName = "CubeSpawnDataSo", order = 0)]
    public class CubeDataConfigSo : ScriptableObject
    {
        [field: SerializeField] public CubeDataConfig DefaultConfig { get; private set; }
    }

    [System.Serializable]
    public class CubeDataConfig
    {
        public int Count;

        public float MinSpeed;
        public float MaxSpeed;

        public CubeWithDestroyDistanceConfig CubeWithDestroyDistanceConfig;

        public GameObject Prefab;
    }

    [System.Serializable]
    public class CubeWithDestroyDistanceConfig
    {
        public int Count;

        public float MinDestroyDistance;
        public float MaxDestroyDistance;
    }
}