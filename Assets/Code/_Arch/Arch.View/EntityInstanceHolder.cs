using System.Collections.Generic;

namespace Code._Arch.Arch.View
{
    public class EntityInstanceHolder<T>
    {
        private readonly IViewHandler<T> _viewHandler;
        private readonly Dictionary<int, T> _entityToInstanceMap;

        public EntityInstanceHolder(IViewHandler<T> viewHandler)
        {
            _viewHandler = viewHandler;
            _entityToInstanceMap = new Dictionary<int, T>();
        }

        public void Register(int entityId)
        {
            if (_entityToInstanceMap.ContainsKey(entityId))
            {
                return;
            }

            _entityToInstanceMap.Add(entityId, _viewHandler.Get());
        }

        public void Remove(int entityId)
        {
            if (!_entityToInstanceMap.TryGetValue(entityId, out T result))
            {
                return;
            }

            _viewHandler.Remove(result);
            _entityToInstanceMap.Remove(entityId);
        }

        public T this[int entityId] => _entityToInstanceMap[entityId];
    }
}