using System;
using UnityEngine;

namespace Code._Arch.Arch.View.Unity
{
    public class GameObjectViewHandler : IViewHandler<GameObject>
    {
        private readonly Func<GameObject> _getter;
        private readonly Action<GameObject> _remover;

        public GameObjectViewHandler(Func<GameObject> getter, Action<GameObject> remover)
        {
            _getter = getter;
            _remover = remover;
        }

        public GameObject Get()
        {
            return _getter();
        }

        public void Remove(GameObject view)
        {
            _remover(view);
        }
    }
}