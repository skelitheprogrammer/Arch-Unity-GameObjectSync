using Arch.Core;
using Arch.Core.Extensions;
using Code._Arch;
using Code._Common;
using Code.CubeLayer.Systems;
using Code.UtilityLayer;

namespace Code.CubeLayer
{
    public class CubeEntityHandler : IEntityHandler
    {
        private CubeEntityFactory _factory;
        private CubeSpawnData _spawnData;

        public CubeEntityHandler(CubeEntityFactory factory, CubeSpawnData spawnData)
        {
            _factory = factory;
            _spawnData = spawnData;
        }
        
        public Entity Create(in World world)
        {
            return _factory.Create(world, _spawnData);
        }

        public void Destroy(in Entity entity)
        {
            Entity destroyEntity = entity.GetWorld().Create(CubeComponentTypes.DestroyArchetype);
            destroyEntity.Set(new Owner
            {
                Value = entity.Reference()
            });
        }
    }
}