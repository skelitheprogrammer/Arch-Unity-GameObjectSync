using Arch.Core;
using Code._Arch.Arch.System;
using Code.UtilityLayer.DataSources;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    internal sealed class CubeStartupSystem : ISystem
    {
        private readonly CubeSpawnData _spawnData;
        private readonly CubeEntityFactory _factory;
        private readonly int _resourceId;

        private int Count => _spawnData.Count;

        public CubeStartupSystem(CubeSpawnData spawnData, CubeEntityFactory factory, int resourceId)
        {
            _spawnData = spawnData;
            _factory = factory;
            _resourceId = resourceId;
        }

        public void Execute(World world)
        {
            world.Reserve(CubeArchetypes.Default, Count);

            for (int i = 0; i < Count; i++)
            {
                Vector3 onUnit = Random.onUnitSphere;

                _factory.Create(world, new CubeInitializer
                {
                    Position = onUnit,
                    Direction = onUnit,
                    Speed = Random.Range(_spawnData.MinSpeed, _spawnData.MaxSpeed),
                    ResourceId = _resourceId
                });
            }
        }
    }
}