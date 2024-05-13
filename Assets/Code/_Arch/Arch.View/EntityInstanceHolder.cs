using System.Collections.Generic;

namespace Code._Arch.Arch.View
{
    public class EntityInstanceHolder<T>
    {
        private readonly Dictionary<int, (T, IViewHandler<T>)> _entityToInstanceMap;

        public EntityInstanceHolder()
        {
            _entityToInstanceMap = new();
        }

        public void Register(int entityId, IViewHandler<T> viewHandler)
        {
            if (_entityToInstanceMap.ContainsKey(entityId))
            {
                return;
            }

            _entityToInstanceMap.Add(entityId, (viewHandler.Get(), viewHandler));
        }

        public void Remove(int entityId)
        {
            if (!_entityToInstanceMap.TryGetValue(entityId, out (T instance, IViewHandler<T> handler) result))
            {
                return;
            }

            result.handler.Remove(result.instance);
            _entityToInstanceMap.Remove(entityId);
        }

        public T this[int entityId] => _entityToInstanceMap[entityId].Item1;
    }
}