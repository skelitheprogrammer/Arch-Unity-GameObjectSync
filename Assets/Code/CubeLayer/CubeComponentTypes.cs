using Arch.Core.Utils;
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

        public static readonly ComponentType[] CubeArchetype =
        {
            typeof(Position),
            typeof(Rotation),
            typeof(MoveDirection),
            typeof(MoveSpeed),
            typeof(ViewReference)
        };
    }
}