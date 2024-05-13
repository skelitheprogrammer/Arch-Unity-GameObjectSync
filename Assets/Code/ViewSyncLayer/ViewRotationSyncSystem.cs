using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using Code.MovableLayer;
using UnityEngine;

namespace Code.ViewSyncLayer
{
    public class ViewRotationSyncSystem : ISystem
    {
        private EntityInstanceHolder<GameObject> _instanceHolder;

        public ViewRotationSyncSystem(EntityInstanceHolder<GameObject> instanceHolder)
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
            private readonly EntityInstanceHolder<GameObject> _instanceHolder;

            public Sync(EntityInstanceHolder<GameObject> instanceHolder)
            {
                _instanceHolder = instanceHolder;
            }

            public void Update(Entity entity)
            {
                ViewReference reference = entity.Get<ViewReference>();
                _instanceHolder[entity.Id].transform.rotation = entity.Get<Rotation>().Value;
            }
        }
    }
}