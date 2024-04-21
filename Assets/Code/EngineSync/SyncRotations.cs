using Arch.Core;
using Arch.Core.Extensions;
using Code.MoveEntity;
using UnityEngine;

namespace Code.EngineSync
{
    public readonly struct SyncRotations : IForEach
    {
        private readonly GameObjectResourceManager _manager;

        public SyncRotations(GameObjectResourceManager manager)
        {
            _manager = manager;
        }

        public void Update(Entity entity)
        {
            Transform transform = _manager[entity].GameObject.transform;
            transform.rotation = Quaternion.LookRotation(entity.Get<Direction>().Value, transform.up);
        }
    }
}