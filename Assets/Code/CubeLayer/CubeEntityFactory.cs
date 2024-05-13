using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.EntityHandling;
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

    public class CubeEntityFactory
    {
        private EntityInstanceHolder<GameObject> _instanceHolder;

        public CubeEntityFactory(EntityInstanceHolder<GameObject> instanceHolder)
        {
            _instanceHolder = instanceHolder;
        }

        public Entity Create(World world, in CubeInitializer initializer)
        {
            Entity entity = world.Create(CubeArchetypes.Default);

            entity.SetRange(new Position
            {
                Value = initializer.Position
            }, new MoveSpeed
            {
                Value = initializer.Speed
            }, new MoveDirection
            {
                Value = initializer.Direction
            }, new ViewReference
            {
                ResourceId = initializer.ResourceId
            });

            _instanceHolder.Register(entity.Id);

            return entity;
        }
    }
}