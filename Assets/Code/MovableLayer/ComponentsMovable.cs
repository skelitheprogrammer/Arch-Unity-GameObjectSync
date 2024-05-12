using UnityEngine;

namespace Code.MovableLayer
{
    public struct Position
    {
        public Vector3 Value;
    }

    public struct Rotation
    {
        public Quaternion Value;
    }
    
    public struct SineWave
    {
        public float Amplitude;
        public float Frequency;
        public Vector3 Axis;

        public float Value;
    }
    
}