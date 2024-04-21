using Arch.Core;
using Arch.Core.Extensions;
using Code.System;
using UnityEngine;

namespace Code.MoveEntity
{
    public class UpdatePositionSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Position>().WithAll<Direction>().WithAll<Speed>();

        public void Execute(World world)
        {
            world.InlineQuery<UpdatePosition>(_description);
        }

        private readonly struct UpdatePosition : IForEach
        {
            public void Update(Entity entity)
            {
                ref Position position = ref entity.Get<Position>();
                position.Value += entity.Get<Direction>().Value * (entity.Get<Speed>().Value * Time.deltaTime);
            }
        }
    }
    
}