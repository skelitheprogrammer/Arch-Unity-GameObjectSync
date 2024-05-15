using System;
using UnityEngine;

namespace Code._Arch.Arch.View
{
    public class GameObjectPool : ViewPool<GameObject>
    {
        private readonly Action<GameObject> _onRent;
        private readonly Action<GameObject> _onRecycle;

        public GameObjectPool(Action<GameObject> onRent, Action<GameObject> onRecycle, Func<GameObject, GameObject> factory, GameObject resource, int allocationSize = 16) : base(factory, resource, allocationSize)
        {
            _onRent = onRent;
            _onRecycle = onRecycle;
        }

        protected override void OnRent(GameObject view)
        {
            _onRent.Invoke(view);
        }

        protected override void OnRecycle(GameObject view)
        {
            _onRecycle.Invoke(view);
        }
    }
}