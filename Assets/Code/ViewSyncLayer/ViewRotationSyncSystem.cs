using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using Code.MovableLayer;

namespace Code.ViewSyncLayer
{
    public class ViewRotationSyncSystem : ISystem
    {
        private EntityInstanceHolder _instanceHolder;

        public ViewRotationSyncSystem(EntityInstanceHolder instanceHolder)
        {
            _instanceHolder = instanceHolder;
        }

        private readonly QueryDescription _description = new QueryDescription().WithAll<ViewReference>().WithAll<Rotation>();

        public void Execute(World world)
        {
            Sync sync = new(_instanceHolder);
            world.InlineQuery(_description, ref sync);
        }

        private readonly struct Sync : IForEach
        {
            private readonly EntityInstanceHolder _instanceHolder;

            public Sync(EntityInstanceHolder instanceHolder)
            {
                _instanceHolder = instanceHolder;
            }

            public void Update(Entity entity)
            {
                ViewReference reference = entity.Get<ViewReference>();
                _instanceHolder.Get(entity.Id).transform.rotation = entity.Get<Rotation>().Value;
            }
        }
    }
}