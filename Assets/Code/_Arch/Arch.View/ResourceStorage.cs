using System.Collections.Generic;

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
}