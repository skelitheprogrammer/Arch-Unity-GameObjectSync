using Arch.Core;
using Arch.Core.Extensions;
using Arch.Core.Utils;
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
        private EntityInstanceHolder _instanceHolder;
        private IViewHandler<GameObject> _viewHandler;

        public CubeEntityFactory(EntityInstanceHolder instanceHolder, IViewHandler<GameObject> viewHandler)
        {
            _instanceHolder = instanceHolder;
            _viewHandler = viewHandler;
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

            _instanceHolder.TryRegisterEntity(entity.Id, _viewHandler);

            return entity;
        }
    }
}