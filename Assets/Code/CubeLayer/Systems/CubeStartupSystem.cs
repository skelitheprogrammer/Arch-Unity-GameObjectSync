using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code.UtilityLayer;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    internal sealed class CubeStartupSystem : ISystem
    {
        private readonly CubeSpawnData _spawnData;

        public CubeStartupSystem(CubeSpawnData spawnData)
        {
            _spawnData = spawnData;
        }

        public void Execute(World world)
        {
            world.Reserve(CubeComponentTypes.CubeInitializerArchetype, _spawnData.Count);
            world.Reserve(CubeComponentTypes.CubeArchetype, _spawnData.Count);

            for (int i = 0; i < _spawnData.Count; i++)
            {
                Entity entity = world.Create(CubeComponentTypes.CubeInitializerArchetype);

                Vector3 onUnitSphere = Random.onUnitSphere;
                entity.Set(new CubeInitializer
                {
                    Position = onUnitSphere * _spawnData.PositionOffset,
                    Direction = onUnitSphere,
                    Speed = Random.Range(_spawnData.MinSpeed, _spawnData.MaxSpeed),
                });
            }
        }
    }
}