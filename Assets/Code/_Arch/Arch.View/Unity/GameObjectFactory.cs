using UnityEngine;

namespace Code._Arch.Arch.View.Unity
{
    public class GameObjectFactory : IViewFactory<GameObject>
    {
        private readonly IResourceStorage _storage;
        private readonly int _resourceId;

        public GameObjectFactory(IResourceStorage storage, int resourceId)
        {
            _storage = storage;
            _resourceId = resourceId;
        }

        public GameObject Create()
        {
            GameObject resource = _storage.Get<GameObject>(_resourceId);
            return Object.Instantiate(resource);
        }
    }
}