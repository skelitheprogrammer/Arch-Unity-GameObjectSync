using Arch.Core;
using Arch.Core.Extensions;
using Code.EngineSync;
using Code.System;

namespace Code.MoveEntity
{
    public class SyncPositionsSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Position>().WithAll<ViewReference>();
        private SyncPositions _syncPositions;

        public SyncPositionsSystem(GameObjectResourceManager manager)
        {
            _syncPositions = new SyncPositions(manager);
        }

        public void Execute(World world)
        {
            world.InlineQuery(_description, ref _syncPositions);
        }

        private readonly struct SyncPositions : IForEach
        {
            private readonly GameObjectResourceManager _manager;

            public SyncPositions(GameObjectResourceManager manager)
            {
                _manager = manager;
            }

            public void Update(Entity entity)
            {
                _manager[entity].GameObject.transform.position = entity.Get<Position>().Value;
            }
        }
    }
}