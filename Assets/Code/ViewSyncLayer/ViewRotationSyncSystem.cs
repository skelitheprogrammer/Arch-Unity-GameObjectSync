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
        private readonly EntityInstanceStorage<GameObject> _instanceHolder;

        public ViewRotationSyncSystem(EntityInstanceStorage<GameObject> instanceHolder)
        {
            _instanceHolder = instanceHolder;
        }

        private readonly QueryDescription _description = new QueryDescription().WithAll<HasView>().WithAll<Rotation>();

        public void Execute(World world)
        {
            Sync sync = new(_instanceHolder);
            world.InlineQuery(_description, ref sync);
        }

        private readonly struct Sync : IForEach
        {
            private readonly EntityInstanceStorage<GameObject> _instanceHolder;

            public Sync(EntityInstanceStorage<GameObject> instanceHolder)
            {
                _instanceHolder = instanceHolder;
            }

            public void Update(Entity entity)
            {
                _instanceHolder[entity.Id].transform.rotation = entity.Get<Rotation>().Value;
            }
        }
    }
}