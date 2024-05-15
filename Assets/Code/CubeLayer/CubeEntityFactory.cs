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

        public void Create(World world, in CubeInitializer initializer)
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
            });

            _instanceStorage.Add(entity.Id, _resourceId);
        }
    }
}