using System.Collections.Generic;

namespace Code._Arch.Arch.View
{
    public class ResourceStorage : IResourceStorage
    {
        private readonly Dictionary<int, object> _map;
        private int Count => _map.Count;

        public ResourceStorage()
        {
            _map = new();
        }

        public int Register<T>(T resource)
        {
            int id = Count;
            _map.Add(id, resource);

            return id;
        }

        public void Remove(int resourceId)
        {
            _map.Remove(resourceId);
        }

        public T Get<T>(int resourceId) => _map[resourceId] is T
            ? (T) _map[resourceId]
            : default;

        public void Dispose()
        {
        }
    }
}