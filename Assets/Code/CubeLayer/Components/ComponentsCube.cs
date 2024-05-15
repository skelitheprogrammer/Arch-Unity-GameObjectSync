using UnityEngine;

namespace Code.CubeLayer.Components
{
    public struct Cube
    {
    }

    public struct MoveSpeed
    {
        public float Value;
    }

    public struct MoveDirection
    {
        public Vector3 Value;
    }

    public struct DistanceTraveled
    {
        public Vector3 StartPosition;

        public float Distance;
    }

    public struct DestroyDistance
    {
        public float Value;
    }
}