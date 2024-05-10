using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;

namespace Code.ViewSyncLayer
{
    public class ViewPositionSyncSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<ViewReference>().WithAll<Position>();

        public void Execute(World world)
        {
            world.InlineQuery<ViewSync>(_description);
        }

        private readonly struct ViewSync : IForEach
        {
            public void Update(Entity entity)
            {
                ViewReference viewReference = entity.Get<ViewReference>();
                viewReference.Value.transform.position = entity.Get<Position>().Value;
            }
        }
    }
}