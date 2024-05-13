using System;

namespace Code._Arch.Arch.View
{
    public interface IResourceStorage : IDisposable
    {
        int Register<T>(T resource);
        void Remove(int resourceId);

        T Get<T>(int resourceId);
    }
}