using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code.ViewSyncLayer;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    internal sealed class CubeMoveSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Position>().WithAll<MoveDirection>().WithAll<MoveSpeed>();

        public void Execute(World world)
        {
            world.InlineQuery<Move>(_description);
        }

        private readonly struct Move : IForEach
        {
            public void Update(Entity entity)
            {
                ref Position position = ref entity.Get<Position>();
                position.Value += entity.Get<MoveDirection>().Value * (entity.Get<MoveSpeed>().Value * Time.deltaTime);
            }
        }
    }
}