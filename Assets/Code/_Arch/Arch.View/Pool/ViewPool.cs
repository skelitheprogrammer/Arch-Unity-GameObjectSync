using System;
using System.Collections.Generic;

namespace Code._Arch.Arch.View
{
    public abstract class ViewPool<T> : IViewPool<T>
    {
        private readonly Func<T, T> _factory;
        private readonly T _resource;
        private readonly int _allocationSize;

        private readonly Queue<T> _pool;

        protected ViewPool(Func<T, T> factory, T resource, int allocationSize = 16)
        {
            _factory = factory;
            _resource = resource;
            _allocationSize = allocationSize;
            _pool = new();
        }

        public virtual void Allocate(int size)
        {
            for (int i = 0; i < size; i++)
            {
                T instance = _factory(_resource);
                Recycle(instance);
            }
        }

        public T Rent()
        {
            if (_pool.Count == 0)
            {
                Allocate(_allocationSize);
            }

            T instance = _pool.Dequeue();
            OnRent(instance);

            return instance;
        }

        public void Recycle(T view)
        {
            _pool.Enqueue(view);
            OnRecycle(view);
        }

        protected abstract void OnRent(T view);
        protected abstract void OnRecycle(T view);
    }
}