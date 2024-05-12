using System;
using UnityEngine;

namespace Code._Arch.Arch.View
{
    public class GameObjectViewHandler : IViewHandler<GameObject>
    {
        private readonly int _resourceId;

        private readonly Func<int, GameObject> _getter;
        private readonly Action<GameObject> _remover;

        public GameObjectViewHandler(int resourceId, Func<int, GameObject> getter, Action<GameObject> remover)
        {
            _resourceId = resourceId;
            _getter = getter;
            _remover = remover;
        }

        public GameObject Get()
        {
            return _getter(_resourceId);
        }

        public void Remove(GameObject view)
        {
            _remover(view);
        }
    }
}