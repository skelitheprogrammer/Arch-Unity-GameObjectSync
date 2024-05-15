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

        private int Count => _dataConfig.Count;

        public CubeStartupSystem(CubeDataConfig dataConfig, CubeEntityFactory factory)
        {
            _dataConfig = dataConfig;
            _factory = factory;
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
                    Speed = Random.Range(_dataConfig.MinSpeed, _dataConfig.MaxSpeed)
                });
            }
        }
    }
}