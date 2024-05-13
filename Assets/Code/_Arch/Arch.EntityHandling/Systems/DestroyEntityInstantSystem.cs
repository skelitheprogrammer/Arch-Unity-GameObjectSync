using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.EntityHandling.Components;
using Code._Arch.Arch.System;
using Code._Common;

namespace Code._Arch.Arch.EntityHandling.Systems
{
    public class DestroyEntityInstantSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Components.Destroy>().WithAll<Owner>();

        public void Execute(World world)
        {
            world.InlineQuery<Destroy>(_description);
        }

        private readonly struct Destroy : IForEach
        {
            public void Update(Entity entity)
            {
                entity.Get<Owner>().Value.Entity.Destroy();
                entity.Destroy();
            }
        }
    }
}