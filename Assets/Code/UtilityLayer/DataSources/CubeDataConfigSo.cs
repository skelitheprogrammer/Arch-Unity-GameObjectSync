using UnityEngine;

namespace Code.UtilityLayer.DataSources
{
    [CreateAssetMenu(menuName = "Create CubeSpawnDataSo", fileName = "CubeSpawnDataSo", order = 0)]
    public class CubeDataConfigSo : ScriptableObject
    {
        [field: SerializeField] public CubeDataConfig DataConfig { get; private set; }
    }

    [System.Serializable]
    public class CubeDataConfig
    {
        public int Count;

        public float MinSpeed;
        public float MaxSpeed;

        public GameObject Prefab;
    }
}