using UnityEngine;

namespace Code.CubeLayer
{
    public struct MoveSpeed
    {
        public float Value;
    }

    public struct MoveDirection
    {
        public Vector3 Value;
    }
    
    public struct CubeInitializer
    {
        public Vector3 Position;
        public Vector3 Direction;
        public float Speed;
    }
}