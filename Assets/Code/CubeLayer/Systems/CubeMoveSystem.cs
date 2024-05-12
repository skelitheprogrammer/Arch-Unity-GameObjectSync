using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code.CubeLayer.Components;
using Code.MovableLayer;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    public class CubeMoveSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Cube>().WithAll<Position>().WithAll<MoveDirection>().WithAll<MoveSpeed>();

        public void Execute(World world)
        {
            world.InlineQuery<Move>(_description);
        }

        private readonly struct Move : IForEach
        {
            public void Update(Entity entity)
            {
                ref Position position = ref entity.Get<Position>();

                position.Value += entity.Get<MoveDirection>().Value * entity.Get<MoveSpeed>().Value * Time.deltaTime;
            }
        }
    }
}