using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using Code.ViewSyncLayer;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    internal sealed class CubeSpawnSystem : ISystem
    {
        private readonly CommandBuffer _buffer;
        private readonly IViewHandler<GameObject> _cubeViewHandler;

        private readonly QueryDescription _description = new QueryDescription().WithAll<CubeInitializer>().WithAll<ViewRequested>();

        public CubeSpawnSystem(CommandBuffer buffer, IViewHandler<GameObject> viewHandler)
        {
            _buffer = buffer;
            _cubeViewHandler = viewHandler;
        }

        public void Execute(World world)
        {
            Spawn spawn = new(_buffer, _cubeViewHandler);
            world.InlineQuery(_description, ref spawn);
        }

        private readonly struct Spawn : IForEach
        {
            public readonly IViewHandler<GameObject> CubeViewHandler;
            public readonly CommandBuffer Buffer;

            public Spawn(CommandBuffer buffer, IViewHandler<GameObject> getter)
            {
                Buffer = buffer;
                CubeViewHandler = getter;
            }

            public void Update(Entity entity)
            {
                CubeInitializer cubeInitializer = entity.Get<CubeInitializer>();

                Entity bufferEntity = Buffer.Create(CubeComponentTypes.CubeArchetype);

                Buffer.Set(bufferEntity, new Position
                {
                    Value = cubeInitializer.SpawnPosition
                });

                Buffer.Set(bufferEntity, new SpawnPosition
                {
                    Value = cubeInitializer.SpawnPosition
                });

                Buffer.Set(bufferEntity, new MoveDirection
                {
                    Value = cubeInitializer.Direction
                });

                Buffer.Set(bufferEntity, new MoveSpeed
                {
                    Value = cubeInitializer.Speed
                });

                Buffer.Set(bufferEntity, new DestroyDistance
                {
                    Value = cubeInitializer.DestroyDistance
                });
                
                Buffer.Set(bufferEntity, new ViewReference
                {
                    Value = CubeViewHandler.Get()
                });
                
                Buffer.Destroy(entity);
            }
        }
    }
}