using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.EntityHandling.Components;
using Code._Arch.Arch.System;
using Code._Arch.Arch.View;
using UnityEngine;

namespace Code.ViewSyncLayer
{
    public class ViewStateSync : ISystem
    {
        private readonly EntityInstanceStorage<GameObject> _instanceStorage;
        private readonly QueryDescription _description = new QueryDescription().WithAll<Destroy>().WithAll<Owner>();

        public ViewStateSync(EntityInstanceStorage<GameObject> instanceStorage)
        {
            _instanceStorage = instanceStorage;
        }

        public void Execute(World world)
        {
            SyncState syncState = new(_instanceStorage);
            world.InlineQuery(_description, ref syncState);
        }

        private readonly struct SyncState : IForEach
        {
            private readonly EntityInstanceStorage<GameObject> _instanceStorage;

            public SyncState(EntityInstanceStorage<GameObject> instanceStorage)
            {
                _instanceStorage = instanceStorage;
            }

            public void Update(Entity entity)
            {
                Owner owner = entity.Get<Owner>();

                Entity valueEntity = owner.Value.Entity;
                if (!valueEntity.Has<HasView>())
                {
                    return;
                }

                HasView hasView = valueEntity.Get<HasView>();

                _instanceStorage.Remove(valueEntity.Id, hasView.ResourceId);
            }
        }
    }
}