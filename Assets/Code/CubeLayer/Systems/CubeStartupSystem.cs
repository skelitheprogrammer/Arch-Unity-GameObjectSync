using Arch.Core;
using Code._Arch;
using Code._Arch.Arch.System;
using Code.UtilityLayer;

namespace Code.CubeLayer.Systems
{
    internal sealed class CubeStartupSystem : ISystem
    {
        private readonly CubeSpawnData _spawnData;
        private readonly IEntityHandler _entityHandler;

        public CubeStartupSystem(CubeSpawnData spawnData, IEntityHandler entityHandler)
        {
            _spawnData = spawnData;
            _entityHandler = entityHandler;
        }

        public void Execute(World world)
        {
            world.Reserve(CubeComponentTypes.CubeInitializerArchetype, _spawnData.Count);
            world.Reserve(CubeComponentTypes.CubeArchetype, _spawnData.Count);

            for (int i = 0; i < _spawnData.Count; i++)
            {
                _entityHandler.Create(world);
            }
        }
    }
}