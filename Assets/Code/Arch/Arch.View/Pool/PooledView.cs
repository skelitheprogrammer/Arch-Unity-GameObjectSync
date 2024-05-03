using System;

namespace Code.Arch.Arch.View
{
    public readonly struct PooledView<T> : IDisposable
    {
        public readonly T View;
        private readonly ViewPool<T> _pool;

        internal PooledView(T view, ViewPool<T> pool)
        {
            View = view;
            _pool = pool;
        }

        public void Dispose()
        {
            _pool.Return(View);
        }
    }
}