using Code._Arch.Arch.View;
using Code._Arch.Arch.View.Pool;
using UnityEngine;

namespace Code.UtilityLayer
{
    public class CubeViewHandler : IViewHandler<GameObject>
    {
        public GameObject Resource { get; }

        private readonly IViewPool<GameObject> _pool;

        public CubeViewHandler(GameObject resource, int allocationSize)
        {
            Resource = resource;
            _pool = new CubeViewPool(Object.Instantiate, Resource, allocationSize);
        }
        
        public GameObject GetInstance()
        {
            return _pool.Rent();
        }

        public void RemoveInstance(GameObject instance)
        {
            _pool.Recycle(instance);
        }
    }
}