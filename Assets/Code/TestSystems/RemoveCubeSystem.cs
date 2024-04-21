using Arch.Core;
using Code.EngineSync;
using Code.System;

namespace Code.TestSystems
{
    public class RemoveCubeSystem : ISystem
    {
        public void Execute(World world)
        {
            QueryDescription description = new QueryDescription().WithAll<ViewReference>();
            foreach (ref Chunk chunk in world.Query(in description))
            {
                ref Entity entityFirstElement = ref chunk.Entity(0);
                world.Destroy(entityFirstElement);
            }
        }
    }
}