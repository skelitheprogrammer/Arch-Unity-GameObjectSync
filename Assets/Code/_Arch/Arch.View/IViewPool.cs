using System;

namespace Code._Arch.Arch.View
{
    public interface IViewPool<T> : IDisposable
    {
        T Rent();
        void Return(T view);
    }
}