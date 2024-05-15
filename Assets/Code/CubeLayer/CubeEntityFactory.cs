using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.View;
using Code.CubeLayer.Components;
using Code.MovableLayer;
using UnityEngine;

namespace Code.CubeLayer
{
    public struct CubeInitializer
    {
        public Vector3 Position;
        public Vector3 Direction;
        public float Speed;
        public int ResourceId;
    }

    public struct CubeDestroyDistanceInitializer
    {
        public Vector3 StartPosition;
        public float DestroyDistance;
    }

    public class CubeEntityFactory
    {
        private readonly int _resourceId;
        private readonly EntityInstanceStorage<GameObject> _instanceStorage;

        public CubeEntityFactory(int resourceId, EntityInstanceStorage<GameObject> instanceStorage)
        {
            _resourceId = resourceId;
            _instanceStorage = instanceStorage;
        }

        public Entity Create(World world, in CubeInitializer initializer)
        {
            Entity entity = world.Create(CubeArchetypes.Default);

            entity.SetRange(new Position
            {
                Value = initializer.Position
            }, new MoveDirection
            {
                Value = initializer.Direction
            }, new MoveSpeed
            {
                Value = initializer.Speed
            }, new HasView
            {
                ResourceId = _resourceId
            });

            _instanceStorage.Add(entity.Id, _resourceId);

            return entity;
        }

        public Entity Create(World world, in CubeInitializer cubeInitializer, in CubeDestroyDistanceInitializer destroyDistanceInitializer)
        {
            Entity entity = world.Create(CubeArchetypes.CubeWithDistanceDestroy);

            Debug.Log(entity.Id);

            entity.SetRange(new Position
            {
                Value = cubeInitializer.Position
            }, new MoveDirection
            {
                Value = cubeInitializer.Direction
            }, new MoveSpeed
            {
                Value = cubeInitializer.Speed
            }, new DestroyDistance
            {
                Value = destroyDistanceInitializer.DestroyDistance
            }, new DistanceTraveled
            {
                StartPosition = destroyDistanceInitializer.StartPosition
            });

            _instanceStorage.Add(entity.Id, _resourceId);

            return entity;
        }
    }
}