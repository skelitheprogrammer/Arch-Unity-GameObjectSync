using Arch.Core.Utils;
using Code._Arch.Arch.EntityHandling;
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
            typeof(Rotation),
            typeof(MoveDirection),
            typeof(MoveSpeed),

            typeof(ViewReference)
        };

        public static readonly ComponentType[] CubeSinWave =
        {
            typeof(Cube),

            typeof(Position),
            typeof(Rotation),
            typeof(MoveDirection),

            typeof(SineWave),
            typeof(MoveSpeed),

            typeof(ViewReference)
        };
    }
}