using System.Collections.Generic;
using UnityEngine;

namespace Code._Arch.Arch.View
{
    public class EntityInstanceHolder
    {
        private Dictionary<int, (GameObject, IViewHandler<GameObject>)> _instances = new();

        public void TryRegisterEntity(int entityId, IViewHandler<GameObject> handler)
        {
            GameObject gameObject = handler.Get();
            _instances.Add(entityId, (gameObject, handler));
        }

        public void RemoveEntity(int entityId)
        {
            (GameObject instance, IViewHandler<GameObject> handler) = _instances[entityId];
            handler.Remove(instance);
            _instances.Remove(entityId);
        }

        public GameObject Get(int entityId)
        {
            return _instances[entityId].Item1;
        }
    }
}