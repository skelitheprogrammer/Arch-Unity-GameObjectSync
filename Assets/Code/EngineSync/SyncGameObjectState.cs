using Arch.Core;
using Code.System;

namespace Code.EngineSync
{
    public class SyncGameObjectState : ISystem
    {
        private readonly GameObjectResourceManager _manager;

        public SyncGameObjectState(GameObjectResourceManager manager)
        {
            _manager = manager;
        }

        public void Execute(World world)
        {
            for (int index = 0; index < _manager.Pooled.Count; index++)
            {
                GameObjectResourceManager.PooledObject pooledObject = _manager.Pooled[index];
                if (!pooledObject.Reference.IsAlive())
                {
                    _manager.Return(pooledObject);
                }
            }
        }
    }
}