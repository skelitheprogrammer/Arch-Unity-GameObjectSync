using UnityEngine;

namespace Code.CubeLayer
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
        public float Value;
    }

    public struct DestroyDistance
    {
        public float Value;
    }

    public struct CubeInitializer
    {
        public Vector3 SpawnPosition;
        public Vector3 Direction;
        public float DestroyDistance;
        public float Speed;
    }
}