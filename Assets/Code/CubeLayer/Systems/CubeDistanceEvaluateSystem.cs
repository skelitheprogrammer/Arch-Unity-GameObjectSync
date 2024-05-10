using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code.ViewSyncLayer;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    public class CubeDistanceEvaluateSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Position>().WithAll<SpawnPosition>().WithAll<DistanceTraveled>();

        public void Execute(World world)
        {
            world.InlineQuery<EvaluateDifference>(_description);
        }

        private readonly struct EvaluateDifference : IForEach
        {
            public void Update(Entity entity)
            {
                Vector3 startPosition = entity.Get<SpawnPosition>().Value;
                Vector3 position = entity.Get<Position>().Value;

                float distance = Vector3.Distance(startPosition, position);
                entity.Get<DistanceTraveled>().Value = distance;
            }
        }
    }
}