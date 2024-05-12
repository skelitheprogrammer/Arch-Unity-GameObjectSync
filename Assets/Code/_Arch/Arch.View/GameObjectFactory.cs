using UnityEngine;

namespace Code._Arch.Arch.View
{
    public class GameObjectFactory
    {
        private GameObjectResourcesRegistry _registry;

        public GameObjectFactory(GameObjectResourcesRegistry registry)
        {
            _registry = registry;
        }

        public GameObject Create(int id)
        {
            return Object.Instantiate(_registry.Get(id));
        }
    }
}