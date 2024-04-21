using Arch.Core;
using Code.EngineSync;
using Code.System;

namespace Code.MoveEntity
{
    public class SyncRotationSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Direction>().WithAll<ViewReference>();
        private SyncRotations _syncRotations;

        public SyncRotationSystem(GameObjectResourceManager manager)
        {
            _syncRotations = new SyncRotations(manager);
        }

        public void Execute(World world)
        {
            world.InlineQuery(_description, ref _syncRotations);
        }
    }
}