using System;
using System.Collections.Generic;

namespace Code._Arch.Arch.View
{
    public class ViewPool<T> : IViewPool<T>
    {
        private readonly IViewFactory<T> _factory;

        private readonly Action<T> _onRent;
        private readonly Action<T> _onRecycle;
        private readonly Action<IEnumerable<T>> _onDispose;

        private readonly Queue<T> _pool;

        private readonly int _allocationSize;

        public ViewPool(IViewFactory<T> factory, Action<T> onRent, Action<T> onRecycle, Action<IEnumerable<T>> onDispose, int allocationSize = 8)
        {
            _factory = factory;
            _onRent = onRent;
            _onRecycle = onRecycle;
            _onDispose = onDispose;
            _allocationSize = allocationSize;
            _pool = new();
        }

        public void Allocate(int size)
        {
            for (int i = 0; i < size; i++)
            {
                T instance = _factory.Create();
                Recycle(instance);
            }
        }

        public T Rent()
        {
            if (_pool.Count == 0)
            {
                Allocate(_allocationSize);
            }

            return Get();
        }

        private T Get()
        {
            T instance = _pool.Dequeue();
            _onRent(instance);

            return instance;
        }

        public void Recycle(T instance)
        {
            _pool.Enqueue(instance);
            _onRecycle(instance);
        }

        void IDisposable.Dispose()
        {
            _onDispose(_pool);
        }
    }
}