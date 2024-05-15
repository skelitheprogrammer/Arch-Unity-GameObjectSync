using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code.CubeLayer.Components;
using Code.MovableLayer;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    public class CalculateDistanceTraveledSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Position>().WithAll<DistanceTraveled>();

        public void Execute(World world)
        {
            world.InlineQuery<Calculate>(_description);
        }

        private readonly struct Calculate : IForEach
        {
            public void Update(Entity entity)
            {
                ref DistanceTraveled distanceTraveled = ref entity.Get<DistanceTraveled>();

                distanceTraveled.Distance = Vector3.Distance(entity.Get<Position>().Value, distanceTraveled.StartPosition);
            }
        }
    }
}