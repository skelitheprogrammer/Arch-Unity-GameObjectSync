using Arch.Core;
using Arch.Core.Extensions;
using Code.UtilityLayer;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    public sealed class CubeEntityFactory
    {
        public Entity Create(in World world, in CubeSpawnData spawnData)
        {
            Entity entity = world.Create(CubeComponentTypes.CubeInitializerArchetype);

            Vector3 onUnitSphere = Random.onUnitSphere;
            entity.Set(new CubeInitializer
            {
                SpawnPosition = onUnitSphere * spawnData.PositionOffset,
                Direction = onUnitSphere,
                Speed = Random.Range(spawnData.MinSpeed, spawnData.MaxSpeed),
                DestroyDistance = Random.Range(spawnData.MinDistanceDestroy, spawnData.MaxDistanceDestroy)
            });

            return entity;
        }
    }
}