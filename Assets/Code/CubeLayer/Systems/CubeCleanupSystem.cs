using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using Code._Common;
using Code.ViewSyncLayer;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    public class CubeCleanupSystem : ISystem
    {
        private IViewHandler<GameObject> _viewHandler;
        private readonly QueryDescription _description = new QueryDescription().WithAll<Destroy>().WithAll<Owner>();

        public CubeCleanupSystem(IViewHandler<GameObject> viewHandler)
        {
            _viewHandler = viewHandler;
        }

        public void Execute(World world)
        {
            Cleanup cleanup = new(_viewHandler);
            world.InlineQuery(_description, ref cleanup);
        }

        private readonly struct Cleanup : IForEach
        {
            private readonly IViewHandler<GameObject> _viewHandler;

            public Cleanup(IViewHandler<GameObject> viewHandler)
            {
                _viewHandler = viewHandler;
            }

            public void Update(Entity entity)
            {
                ref Owner owner = ref entity.Get<Owner>();
                _viewHandler.Remove(owner.Value.Entity.Get<ViewReference>().Value);
                owner.Value.Entity.Destroy();
                entity.Destroy();
            }
        }
    }
}