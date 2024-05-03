using System;
using System.Collections.Generic;

namespace Code.Arch.Arch.View
{
    public class ViewPool<T> : IViewPool<T>
    {
        private Func<T> _factory;
        private Action<T> _onAllocate;
        private Action<T> _onReturn;
        private readonly Queue<T> _pool;

        private readonly int _incrementSize;

        public ViewPool(int incrementSize = 8)
        {
            _incrementSize = incrementSize;
            _pool = new Queue<T>();
        }

        public void PreAllocate(int size)
        {
            for (int i = 0; i < _incrementSize; i++)
            {
                T instance = _factory();
                _pool.Enqueue(instance);
                _onReturn.Invoke(instance);
            }
        }

        public PooledView<T> Allocate()
        {
            if (_pool.Count == 0)
            {
                PreAllocate(_incrementSize);
            }

            T result = _pool.Dequeue();
            _onAllocate.Invoke(result);

            return new PooledView<T>(result, this);
        }

        public void Return(T view)
        {
            _pool.Enqueue(view);
            _onReturn.Invoke(view);
        }
    }
}