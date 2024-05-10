using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch;
using Code._Arch.Arch.System;

namespace Code.CubeLayer.Systems
{
    public class DestroyCubeOnDistanceTraveledRange : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Cube>().WithAll<DistanceTraveled>();
        private readonly IEntityHandler _entityHandler;

        public DestroyCubeOnDistanceTraveledRange(IEntityHandler entityHandler)
        {
            _entityHandler = entityHandler;
        }

        public void Execute(World world)
        {
            DestroyOnDistanceReach destroy = new(_entityHandler);
            world.InlineQuery(_description, ref destroy);
        }

        private readonly struct DestroyOnDistanceReach : IForEach
        {
            private readonly IEntityHandler _entityHandler;

            public DestroyOnDistanceReach(IEntityHandler entityHandler)
            {
                _entityHandler = entityHandler;
            }

            public void Update(Entity entity)
            {
                float distanceTraveled = entity.Get<DistanceTraveled>().Value;
                float destroyDistance = entity.Get<DestroyDistance>().Value;

                if (distanceTraveled > destroyDistance)
                {
                    _entityHandler.Destroy(entity);
                }
            }
        }
    }
}