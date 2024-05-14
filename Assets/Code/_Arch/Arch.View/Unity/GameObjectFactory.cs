using UnityEngine;

namespace Code._Arch.Arch.View.Unity
{
    public class GameObjectFactory : IViewFactory<GameObject>
    {
        private readonly IResourceStorage _storage;

        public GameObjectFactory(IResourceStorage storage)
        {
            _storage = storage;
        }

        public GameObject Create(int resourceId)
        {
            GameObject resource = _storage.Get<GameObject>(resourceId);
            return Object.Instantiate(resource);
        }
    }
}