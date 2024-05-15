using Arch.Core.Utils;
using Code._Arch.Arch.EntityHandling.Components;
using Code._Arch.Arch.View;
using Code.CubeLayer.Components;
using Code.MovableLayer;

namespace Code.CubeLayer
{
    internal static class CubeArchetypes
    {
        public static readonly ComponentType[] Destroy =
        {
            typeof(Cube),
            typeof(Destroy),
            typeof(Owner)
        };

        public static readonly ComponentType[] Default =
        {
            typeof(Cube),

            typeof(Position),
            typeof(MoveDirection),
            typeof(MoveSpeed),

            typeof(Rotation),

            typeof(HasView)
        };

        public static readonly ComponentType[] CubeWithDistanceDestroy =
        {
            typeof(Cube),

            typeof(Position),
            typeof(MoveDirection),
            typeof(MoveSpeed),

            typeof(Rotation),

            typeof(DestroyDistance),
            typeof(DistanceTraveled),
            
            typeof(HasView)
        };

        public static readonly ComponentType[] CubeSineWave =
        {
            typeof(Cube),

            typeof(Position),
            typeof(Rotation),
            typeof(MoveDirection),

            typeof(SineWave),
            typeof(MoveSpeed),

            typeof(HasView)
        };
    }
}