using Arch.Core;
using Code.Spawning;
using Code.System;
using UnityEngine;

namespace Code.TestSystems
{
    public class AddCubeSystem : ISystem
    {
        public void Execute(World world)
        {
            world.Create(new CubeSpawnInitializer
            {
                Direction = Random.insideUnitSphere,
                Position = new Vector3(0, 0, 0) + Random.insideUnitSphere,
                Speed = Random.Range(10, 20),
                Key = Constants.SPHERE_KEY
            });
        }
    }
}