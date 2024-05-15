using Arch.Buffer;
using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.EntityHandling.Components;
using Code._Arch.Arch.System;

namespace Code.CubeLayer.Systems
{
    public class DestroyEntitySystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Destroy>().WithAll<Owner>();
        private readonly CommandBuffer _buffer;

        public DestroyEntitySystem(CommandBuffer buffer)
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
                ref Owner owner = ref entity.Get<Owner>();

                _buffer.Destroy(owner.Value.Entity);
                _buffer.Destroy(entity);
            }
        }
    }
}