using Arch.Core;
using Arch.Core.Utils;
using Code.EngineSync;
using Code.MoveEntity;
using Code.Spawning;
using Code.System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Code
{
    public static class Constants
    {
        public const string CUBE_KEY = "Cube";
        public const string SPHERE_KEY = "Sphere";
        public const string SPAWN_DATA_KEY = "CubeSpawnData";
    }

    public class StartupSystem : IAsyncSystem
    {
        private readonly GameObjectResourceManager _manager;

        public StartupSystem(GameObjectResourceManager manager)
        {
            _manager = manager;
        }

        public async UniTask Execute(World world)
        {
            GameObject prefab = await Addressables.LoadAssetAsync<GameObject>(Constants.CUBE_KEY).ToUniTask();
            GameObject prefabEnemy = await Addressables.LoadAssetAsync<GameObject>(Constants.SPHERE_KEY).ToUniTask();
            CubeSpawnDataSo spawnAsset = await Addressables.LoadAssetAsync<CubeSpawnDataSo>(Constants.SPAWN_DATA_KEY).ToUniTask();

            _manager.RegisterPrefab(Constants.CUBE_KEY, prefab);
            _manager.RegisterPrefab(Constants.SPHERE_KEY, prefabEnemy);

            CubeSpawnData spawnData = spawnAsset.Data;
            int dataSize = spawnData.Size;

            world.Reserve(new ComponentType[]
            {
                typeof(Position),
                typeof(Direction),
                typeof(Speed),
                typeof(ViewReference)
            }, dataSize);

            _manager.Allocate(Constants.CUBE_KEY, dataSize);

            for (int i = 0; i < dataSize; i++)
            {
                world.Create(new CubeSpawnInitializer
                {
                    Direction = Random.insideUnitSphere,
                    Position = Random.insideUnitSphere * spawnData.PositionRangeOffset,
                    Speed = Random.Range(spawnData.MinSpeed, spawnData.MaxSpeed),
                    Key = Constants.CUBE_KEY
                });
            }
        }

        void ISystem.Execute(World world)
        {
        }
    }
}