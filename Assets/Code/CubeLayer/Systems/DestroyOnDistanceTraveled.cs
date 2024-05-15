using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.EntityHandling.Components;
using Code._Arch.Arch.System;
using Code._Common;
using Code.CubeLayer.Components;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    public class DestroyOnDistanceTraveled : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Cube>().WithAll<DestroyDistance>().WithAll<DistanceTraveled>();

        public void Execute(World world)
        {
            world.InlineQuery<Destroy>(_description);
        }

        private readonly struct Destroy : IForEach
        {
            public void Update(Entity entity)
            {
                float destroyDistance = entity.Get<DestroyDistance>().Value;
                float distance = entity.Get<DistanceTraveled>().Distance;

                if (distance < destroyDistance)
                {
                    return;
                }

                Entity destroyEntity = entity.GetWorld().Create(CubeArchetypes.Destroy);

                destroyEntity.Set(new Owner
                {
                    Value = entity.Reference()
                });
            }
        }
    }
}