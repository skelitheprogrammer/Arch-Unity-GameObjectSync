using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using UnityEngine;

namespace Code.ViewSyncLayer
{
    public class ViewRotationSyncSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<ViewReference>().WithAll<Rotation>();

        public void Execute(World world)
        {
            world.InlineQuery<Sync>(_description);
        }


        private readonly struct Sync : IForEach
        {
            public void Update(Entity entity)
            {
                GameObject gameObject = entity.Get<ViewReference>().Value;
                gameObject.transform.rotation = entity.Get<Rotation>().Value;
            }
        }
    }
}