using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using Code.MovableLayer;

namespace Code.ViewSyncLayer
{
    public class ViewPositionSyncSystem : ISystem
    {
        private readonly EntityInstanceHolder _instanceHolder;
        private readonly QueryDescription _description = new QueryDescription().WithAll<ViewReference>().WithAll<Position>();

        public ViewPositionSyncSystem(EntityInstanceHolder instanceHolder)
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
            private readonly EntityInstanceHolder _instanceHolder;

            public ViewSync(EntityInstanceHolder instanceHolder)
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