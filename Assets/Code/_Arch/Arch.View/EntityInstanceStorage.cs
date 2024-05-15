using System.Collections.Generic;

namespace Code._Arch.Arch.View
{
    public class EntityInstanceStorage<T>
    {
        private readonly ResourceStorage<T> _resourceStorage;
        private readonly SortedList<int, T> _map;

        public EntityInstanceStorage(ResourceStorage<T> resourceStorage)
        {
            _resourceStorage = resourceStorage;
            _map = new();
        }

        public void Add(int entityId, int resourceId)
        {
            T instance = _resourceStorage[resourceId];
            _map.Add(entityId, instance);
        }

        public T this[int entityId] => _map[entityId];
    }
}