using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using Code.MovableLayer;
using UnityEngine;

namespace Code.ViewSyncLayer
{
    public class ViewPositionSyncSystem : ISystem
    {
        private readonly EntityInstanceHolder<GameObject> _instanceHolder;
        private readonly QueryDescription _description = new QueryDescription().WithAll<ViewReference>().WithAll<Position>();

        public ViewPositionSyncSystem(EntityInstanceHolder<GameObject> instanceHolder)
        {
            _instanceHolder = instanceHolder;
        }

        public void Execute(World world)
        {
            ViewSync sync = new(_instanceHolder);
            world.InlineQuery(_description, ref sync);
        }

        private readonly struct ViewSync : IForEach
        {
            private readonly EntityInstanceHolder<GameObject> _instanceHolder;

            public ViewSync(EntityInstanceHolder<GameObject> instanceHolder)
            {
                _instanceHolder = instanceHolder;
            }

            public void Update(Entity entity)
            {
                _instanceHolder[entity.Id].transform.position = entity.Get<Position>().Value;
            }
        }
    }
}