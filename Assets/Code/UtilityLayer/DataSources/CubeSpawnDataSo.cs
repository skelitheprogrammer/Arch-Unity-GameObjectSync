using UnityEngine;

namespace Code.UtilityLayer.DataSources
{
    [CreateAssetMenu(menuName = "Create CubeSpawnDataSo", fileName = "CubeSpawnDataSo", order = 0)]
    public class CubeSpawnDataSo : ScriptableObject
    {
        [field: SerializeField] public CubeSpawnData SpawnData { get; private set; }
    }

    [System.Serializable]
    public class CubeSpawnData
    {
        public int Count;

        public float MinSpeed;
        public float MaxSpeed;

        public GameObject Prefab;
    }
}