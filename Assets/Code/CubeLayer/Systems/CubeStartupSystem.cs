using Arch.Core;
using Code._Arch.Arch.System;
using Code.UtilityLayer.DataSources;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    internal sealed class CubeStartupSystem : ISystem
    {
        private readonly CubeDataConfig _dataConfig;
        private readonly CubeEntityFactory _factory;
        
        public CubeStartupSystem(CubeDataConfig dataConfig, CubeEntityFactory factory)
        {
            _dataConfig = dataConfig;
            _factory = factory;
        }
        
        public void Execute(World world)
        {
            if (_dataConfig.Count > 0)
            {
                world.Reserve(CubeArchetypes.Default, _dataConfig.Count);

                for (int i = 0; i < _dataConfig.Count; i++)
                {
                    Vector3 onUnit = Random.onUnitSphere;
                    _factory.Create(world, new CubeInitializer
                    {
                        Position = onUnit,
                        Direction = onUnit,
                        Speed = Random.Range(_dataConfig.MinSpeed, _dataConfig.MaxSpeed)
                    });
                }
            }

            if (_dataConfig.CubeWithDestroyDistanceConfig.Count > 0)
            {
                world.Reserve(CubeArchetypes.Destroy, _dataConfig.CubeWithDestroyDistanceConfig.Count);
                world.Reserve(CubeArchetypes.CubeWithDistanceDestroy, _dataConfig.CubeWithDestroyDistanceConfig.Count);

                for (int i = 0; i < _dataConfig.CubeWithDestroyDistanceConfig.Count; i++)
                {
                    Vector3 onUnit = Random.onUnitSphere;
                    _factory.Create(world, new CubeInitializer
                    {
                        Position = onUnit,
                        Direction = onUnit,
                        Speed = Random.Range(_dataConfig.MinSpeed, _dataConfig.MaxSpeed)
                    }, new CubeDestroyDistanceInitializer
                    {
                        StartPosition = onUnit,
                        DestroyDistance = Random.Range(_dataConfig.CubeWithDestroyDistanceConfig.MinDestroyDistance, _dataConfig.CubeWithDestroyDistanceConfig.MaxDestroyDistance)
                    });
                }
            }
        }
    }
}