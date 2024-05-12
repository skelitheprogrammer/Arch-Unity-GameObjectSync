using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code._Arch.Arch.View
{
    public class GameObjectPool
    {
        private readonly Func<int, GameObject> _factory;

        private readonly Action<GameObject> _onRent;
        private readonly Action<GameObject> _onReturn;

        private readonly int _allocationSize;
        private readonly int _resourceId;
        private readonly Queue<GameObject> _pool;

        public GameObjectPool(int allocationSize, Func<int, GameObject> factory, Action<GameObject> onRent, Action<GameObject> onReturn)
        {
            _allocationSize = allocationSize;
            _factory = factory;
            _onRent = onRent;
            _onReturn = onReturn;

            _pool = new();
        }

        public void Allocate(int size)
        {
            for (int i = 0; i < size; i++)
            {
                GameObject instance = _factory(_resourceId);
                Return(instance);
            }
        }

        public GameObject Rent(int resourceId)
        {
            if (_pool.Count == 0)
            {
                Allocate(_allocationSize);
            }

            return Get();
        }

        private GameObject Get()
        {
            GameObject gameObject = _pool.Dequeue();
            _onRent(gameObject);
            return gameObject;
        }

        public void Return(GameObject view)
        {
            _pool.Enqueue(view);
            _onReturn(view);
        }
    }
}