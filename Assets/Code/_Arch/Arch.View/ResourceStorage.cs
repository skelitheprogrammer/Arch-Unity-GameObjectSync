using System.Collections.Generic;
using Arch.Core;
using Arch.Core.Extensions;

namespace Code._Arch.Arch.View
{
    public class ResourceStorage<T>
    {
        private readonly SortedList<int, IViewHandler<T>> _map;

        public ResourceStorage()
        {
            _map = new();
        }

        public int Add(in IViewHandler<T> handler)
        {
            int count = _map.Count;
            _map.Add(count, handler);
            return count;
        }

        public T this[int id] => _map[id].GetInstance();
    }


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