using Arch.Core;
using Arch.Core.Extensions;
using Code.System;

namespace Code.Arch.Arch.View.Systems
{
    public class AllocatePoolViewsSystem<T> : ISystem
    {
        private readonly IViewPool<T> _pool;
        private readonly QueryDescription _description = new QueryDescription().WithExclusive<AllocateSize>();

        public AllocatePoolViewsSystem(IViewPool<T> pool)
        {
            _pool = pool;
        }

        public void Execute(World world)
        {
            AllocatePoolViews allocatePoolViews = new()
            {
                ViewPool = _pool
            };

            world.InlineQuery(_description, ref allocatePoolViews);
        }

        public struct AllocatePoolViews : IForEach
        {
            public IViewPool<T> ViewPool;

            public void Update(Entity entity)
            {
                int size = entity.Get<AllocateSize>().Value;
                ViewPool.PreWarm(size);
            }
        }
    }

    public struct AllocateSize
    {
        public int Value;
    }
}