using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Arch.Arch.View
{
    public class GameObjectPool : IPool<GameObject>, IDisposable
    {
        private readonly Func<GameObject, GameObject> _factory;
        private readonly Action<GameObject> _onRent;
        private readonly Action<GameObject> _onReturn;
        private readonly int _allocationSize;
        private readonly GameObject _prefab;

        private readonly Queue<GameObject> _pool;

        public GameObjectPool(Func<GameObject, GameObject> factory, Action<GameObject> onRent, Action<GameObject> onReturn, GameObject prefab, int allocationSize = 8)
        {
            _factory = factory;
            _onRent = onRent;
            _onReturn = onReturn;
            _allocationSize = allocationSize;
            _prefab = prefab;
            _pool = new();
        }

        public void Allocate(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject instance = _factory(_prefab);
                _pool.Enqueue(instance);
                instance.SetActive(false);
            }
        }

        public GameObject Rent()
        {
            if (_pool.Count == 0)
            {
                Allocate(_allocationSize);
            }

            GameObject instance = _pool.Dequeue();

            _onRent?.Invoke(instance);

            return instance;
        }

        public void Return(GameObject view)
        {
            _pool.Enqueue(view);
            _onReturn?.Invoke(view);
        }

        public void Dispose()
        {
            _pool.Clear();
        }
    }
}