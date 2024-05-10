using Arch.Core.Utils;
using Code._Common;
using Code.ViewSyncLayer;

namespace Code.CubeLayer
{
    internal static class CubeComponentTypes
    {
        public static readonly ComponentType[] CubeInitializerArchetype =
        {
            typeof(CubeInitializer),
            typeof(ViewRequested)
        };

        public static readonly ComponentType[] DestroyArchetype =
        {
            typeof(Destroy),
            typeof(Cube),

            typeof(Owner),
        };

        public static readonly ComponentType[] CubeArchetype =
        {
            typeof(Cube),

            typeof(SpawnPosition),
            typeof(DestroyDistance),
            typeof(DistanceTraveled),

            typeof(Position),
            typeof(Rotation),
            typeof(MoveDirection),

            typeof(MoveSpeed),

            typeof(ViewReference)
        };
    }
}