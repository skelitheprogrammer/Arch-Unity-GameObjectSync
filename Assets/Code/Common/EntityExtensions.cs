using Arch.Core;

namespace Code
{
    public static class EntityExtensions
    {
        public static World GetWorld(this in Entity entity)
        {
            return World.Worlds[entity.WorldId];
        }

        public static void Destroy(this in Entity entity)
        {
            entity.GetWorld().Destroy(entity);
        }
    }
}