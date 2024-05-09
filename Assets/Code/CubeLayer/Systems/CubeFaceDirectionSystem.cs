using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch.Arch.System;
using Code.ViewSyncLayer;
using UnityEngine;

namespace Code.CubeLayer.Systems
{
    public class CubeFaceDirectionSystem : ISystem
    {
        private readonly QueryDescription _description = new QueryDescription().WithAll<Rotation>().WithAll<MoveDirection>();

        public void Execute(World world)
        {
            world.InlineQuery<FaceDirection>(_description);
        }

        private readonly struct FaceDirection : IForEach
        {
            public void Update(Entity entity)
            {
                entity.Get<Rotation>().Value = Quaternion.FromToRotation(Vector3.up, entity.Get<MoveDirection>().Value);
            }
        }
    }
}