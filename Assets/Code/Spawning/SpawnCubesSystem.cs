using Arch.Core;
using Arch.Core.Extensions;
using Code.EngineSync;
using Code.MoveEntity;
using Code.System;

namespace Code.Spawning
{
    public class SpawnCubesSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<CubeSpawnInitializer>();
        private SpawnCubes _spawnCubes;

        public void Execute(World world)
        {
            world.InlineQuery(_description, ref _spawnCubes);
        }

        private readonly struct SpawnCubes : IForEach
        {
            public void Update(Entity entity)
            {
                CubeSpawnInitializer initializer = entity.Get<CubeSpawnInitializer>();

                entity.GetWorld().Create(
                    new Position
                    {
                        Value = initializer.Position
                    },
                    new Direction
                    {
                        Value = initializer.Direction
                    },
                    new Speed
                    {
                        Value = initializer.Speed
                    },
                    new ViewReference
                    {
                        Key = initializer.Key,
                        IsLoaded = false
                    });

                entity.Destroy();
            }
        }
    }
}