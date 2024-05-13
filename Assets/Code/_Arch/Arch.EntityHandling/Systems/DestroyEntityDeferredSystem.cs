using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.EntityHandling.Components;
using Code._Arch.Arch.System;

namespace Code._Arch.Arch.EntityHandling.Systems
{
    public class DestroyEntityDeferredSystem : ISystem
    {
        private readonly CommandBuffer _buffer;
        private readonly QueryDescription _description = new QueryDescription().WithAll<Components.Destroy>().WithAll<Owner>();

        public DestroyEntityDeferredSystem(CommandBuffer buffer)
        {
            _buffer = buffer;
        }

        public void Execute(World world)
        {
            Destroy destroy = new(_buffer);
            world.InlineQuery(_description, ref destroy);
        }

        private readonly struct Destroy : IForEach
        {
            private readonly CommandBuffer _buffer;

            public Destroy(CommandBuffer buffer)
            {
                _buffer = buffer;
            }

            public void Update(Entity entity)
            {
                _buffer.Destroy(entity.Get<Owner>().Value.Entity);
                _buffer.Destroy(entity);
            }
        }
    }
}